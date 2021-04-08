using Mirror;
using System.Collections;
using System.Collections.Generic;
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
    /// Amount of questions that all players have answered
    /// </summary>
    [SyncVar(hook = nameof(AnswerDoneSet))]
    int questionsSolved;

    [SyncVar]
    int onQuestionIndex;

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

    private void AnswerDoneSet(int oldValue, int newValue)
    {
        answersDoneTextbox.text = newValue.ToString();
    }

    protected override void OnPuzzleActivated()
    {
        base.OnPuzzleActivated();
        questionsSolved = 0;

        playerProgress.Clear();

        //currentSabotageState = QuestionState.WaitingPlayers;

        SetNewQuestion();
    }

    /// <summary>
    /// Gets a random question and sends the question and multiple answer options to show to the user
    /// </summary>
    [Server]
    private void SetNewQuestion()
    {
        var question = questions.questions.PickRandom();

        RpcSetQuestion(question.question, question.answers);
    }

    /// <summary>
    /// Used by UI. Sets on server as one of the players to solve questions
    /// </summary>
    [Client]
    public void SetAsResponder()
    {
        CmdSetAsResponder();
    }

    [Command]
    void CmdSetAsResponder(NetworkConnectionToClient sender = null)
    {
        playerProgress.Add(sender, 0);

        var areEnoughPlayers = playerProgress.Count == questionsNeededToSolve;

        if (areEnoughPlayers)
        {
            SetNewQuestion();
        }
    }

    /// <summary>
    /// Setting new question on clients
    /// </summary>
    /// <param name="question">Question instructions</param>
    /// <param name="answers">List of available answers</param>
    [ClientRpc]
    void RpcSetQuestion(string question, List<string> answers)
    {
        foreach(Transform obj in answersParent.transform)
        {
            if(obj != answersParent.transform)
            {
                Destroy(obj.gameObject);
            }
        }

        this.question.text = question;

        for(var i = 0; i < answers.Count; i++)
        {
            var button = Instantiate(answerButtonPrefab, answersParent.transform);

            var text = button.GetComponentInChildren<TextMeshProUGUI>();
            text.text = answers[i];

            var buttonItem = button.GetComponent<Button>();
            buttonItem.onClick.AddListener(() => CmdSendAnswer(i));
        }
    }

    [ClientRpc]
    void RpcSetProgress()
    {
        progressText.text = string.Format(progressTextFormat, questionsSolved, questionsNeededToSolve);
    }

    [ClientRpc]
    void RpcOnQuestionsEnded()
    {
        gameObject.SetActive(false);
        PuzzleCompletion.instance.MarkCompleted(PuzzleId.TheoryVariety);
    }

    [Command]
    void CmdSendAnswer(int answerIndex, NetworkConnectionToClient sender = null)
    {
        var isCorrectAnswer = questions.questions[onQuestionIndex].correctAnswerIndex == answerIndex;

        if (isCorrectAnswer)
        {
            if (playerProgress.ContainsKey(sender))
            {
                playerProgress[sender] = playerProgress[sender] + 1;
            } else
            {
                playerProgress.Add(sender, 1);
            }
        }
        SetNewQuestion();
        RpcSetProgress();
    }
}
