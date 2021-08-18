using Extensions;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SabotageQuestions : SabotagePuzzle
{
    /// <summary>
    /// Used for the state machine of the sabotage questions
    /// </summary>
    public enum QuestionState
    {
        WaitingPlayers,
        OnQuestion,
        Finished
    }

    [Header("UI components")]
    [Tooltip("GameObject containing the UI to show when waiting enough players to respond to emergency")]
    [SerializeField]
    GameObject waitingPlayersUi;

    [Tooltip("GameObject containing the UI when waiting other players to solve their question")]
    [SerializeField]
    GameObject waitingOnUsersToSolveUi;

    [Tooltip("GameObject containing the UI for showing the question")]
    [SerializeField]
    GameObject onQuestionUi;

    [Tooltip("Text to show question")]
    [SerializeField]
    TextMeshProUGUI question;

    [Tooltip("GameObject to append answers")]
    [SerializeField]
    GameObject answersParent;

    [SerializeField]
    [Tooltip("Text format to use for answers done by player, use {0} for input there the number of answers done")]
    [TextArea]
    string answersDoneFormat;

    [Tooltip("A texbox showing the amount of questions solved by player")]
    [SerializeField]
    TextMeshProUGUI answersDoneTextbox;

    [Tooltip("Shows how many questions have been answered by all players")]
    [SerializeField]
    TextMeshProUGUI progressText;

    [SerializeField]
    [Tooltip("Text format used for all questions done by all the players, use {0} for the number of questions done, {1} for the number of questions to do.")]
    [TextArea]
    string progressTextFormat;

    [Header("Prefabs")]
    [Tooltip("Prefab used for buttons for answers")]
    [SerializeField]
    GameObject answerButtonPrefab;

    [Header("Questions")]
    [Tooltip("List of possible questions to make")]
    [SerializeField]
    SabotageQuestionList questions;

    [Tooltip("How many questions ask to players before marking as solved")]
    [SerializeField]
    int questionsNeededToSolve;

    /// <summary>
    /// Players status in question results
    /// </summary>
    /// <remarks>
    /// Only used on server
    /// </remarks>
    readonly Dictionary<NetworkConnection,int> playerProgress = new Dictionary<NetworkConnection, int>();

    /// <summary>
    /// Current question for players in sabotage
    /// </summary>
    readonly Dictionary<NetworkIdentity, int> currentQuestion = new Dictionary<NetworkIdentity, int>();

    /// <summary>
    /// Amount of questions that all players have answered
    /// </summary>
    [SyncVar(hook = nameof(AnswerDoneSet))]
    int questionsSolved;

   

    // We are going to ignore states on this moment, only show questions 

    //[SyncVar(hook = nameof(OnStateChanged))]
    //QuestionState currentSabotageState;

    //private void OnStateChanged(QuestionState oldValue, QuestionState newValue)
    //{
    //    // Reseting before settings new value
    //    waitingPlayersUi.SetActive(false);
    //    waitingOnUsersToSolveUi.SetActive(false);

    //    switch (newValue)
    //    {
    //        case QuestionState.WaitingPlayers:
    //            waitingPlayersUi.SetActive(true);
    //            break;
    //        case QuestionState.OnQuestion:
    //            waitingOnUsersToSolveUi.SetActive(true);
    //            break;
    //    }
    //}

    protected override bool AreEmergencyConditionsEnough(Emergency.EmergencyType type) =>
    type == Emergency.EmergencyType.QuestionSabotage;

    private void AnswerDoneSet(int oldValue, int newValue)
    {
        answersDoneTextbox.text = newValue.ToString();
    }

    protected override void OnPuzzleActivated()
    {
        base.OnPuzzleActivated();
        questionsSolved = 0;

        playerProgress.Clear();
        currentQuestion.Clear();

        //currentSabotageState = QuestionState.WaitingPlayers;

        SetNewQuestion();
    }

    /// <summary>
    /// Gets a random question and sends the question and multiple answer options to show to the user
    /// </summary>
    [Server]
    private void SetNewQuestion(NetworkConnection target = null)
    {
        var index = Random.Range(0, questions.questions.Count);

        var question = questions.questions[index];

        if(target != null)
        {
            if (currentQuestion.ContainsKey(target.identity))
            {
                currentQuestion[target.identity] = index;
            }
            else
            {
                currentQuestion.Add(target.identity, index);
            }

            TargetRpcSetQuestion(target, question.question, question.answers);
        }
        else
        {
            var players = GameObject.FindGameObjectsWithTag(Tags.Player);
            foreach(var player in players)
            {
                var identity = player.GetComponent<NetworkIdentity>();
                if(identity != null)
                {
                    currentQuestion.Add(identity, index);
                }
            }
            RpcSetQuestion(question.question, question.answers);
        }
    }

    /// <summary>
    /// Used by UI. Sets on server as one of the players to solve questions
    /// </summary>
    [Client]
    public void SetAsResponder() => CmdSetAsResponder();

    [Command]
    void CmdSetAsResponder(NetworkConnectionToClient sender = null)
    {
        playerProgress.Add(sender, 0);

        var areEnoughPlayers = playerProgress.Count == questionsNeededToSolve;

        if (areEnoughPlayers)
        {
            SetNewQuestion(sender);
        }
    }

    /// <summary>
    /// Setting new question on clients
    /// </summary>
    /// <param name="question">Question instructions</param>
    /// <param name="answers">List of available answers</param>
    [TargetRpc]
    void TargetRpcSetQuestion(NetworkConnection target, string question, List<string> answers) => 
        SetQuestion(question, answers);

    [ClientRpc]
    void RpcSetQuestion(string question, List<string> answers) => SetQuestion(question, answers);

    [Client]
    void SetQuestion(string question, List<string> answers)
    {
        print($"Setting question, {question}");
        foreach (Transform obj in answersParent.transform)
        {
            if (obj != answersParent.transform)
            {
                Destroy(obj.gameObject);
            }
        }

        this.question.text = question;

        for (int i = 0; i < answers.Count; i++)
        {
            int iCopy = i;
            var button = Instantiate(answerButtonPrefab, answersParent.transform);

            var text = button.GetComponentInChildren<TextMeshProUGUI>();
            text.text = answers[i];

            var buttonItem = button.GetComponent<Button>();
            buttonItem.onClick.AddListener(() => SendAnswer(iCopy));
        }
    }

    void SendAnswer(int answerIndex) => CmdSendAnswer(answerIndex);

    [ClientRpc]
    void RpcSetProgress()
    {
        progressText.text = string.Format(progressTextFormat, questionsSolved, questionsNeededToSolve);
    }

    [ClientRpc]
    void RpcOnQuestionsEnded()
    {
        gameObject.SetActive(false);
        
    }

    [Command(ignoreAuthority = true)]
    void CmdSendAnswer(int answerIndex, NetworkConnectionToClient sender = null)
    {
        var isCorrectAnswer = questions.questions[currentQuestion[sender.identity]].correctAnswerIndex == answerIndex;
        if (isCorrectAnswer)
        {
            this.SuperPrint($"Correct answer by {sender.connectionId}");
            if (playerProgress.ContainsKey(sender))
            {
                playerProgress[sender] = playerProgress[sender] + 1;
            } else
            {
                playerProgress.Add(sender, 1);
            }
            questionsSolved++;

            if(questionsSolved >= questionsNeededToSolve)
            {
                foreach(var player in playerProgress.Keys)
                {
                    SetPuzzleAsCompleted(player);
                }
            }
        }
        SetNewQuestion(sender);
        RpcSetProgress();
    }

    protected override bool ArePuzzleCompletionConditionsReached()
    {
        return questionsSolved >= questionsNeededToSolve;
    }
}
