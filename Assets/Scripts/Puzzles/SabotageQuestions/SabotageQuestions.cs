using Mirror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SabotageQuestions : NetworkBehaviour
{
    // --- STATE
    [SyncVar]
    int correctAnswers = 0;

    [SyncVar(hook = nameof(AnswerDoneSet))]
    int answersDone = 0;

    int currentAnswer;

    [SerializeField]
    SabotageQuestionList questions;

    [SerializeField]
    int correctAnswersNeeded;

    [Header("UI components")]
    [SerializeField]
    TextMeshProUGUI question;

    [SerializeField]
    GameObject answersParent;

    [SerializeField]
    Slider progressSlider;

    [SerializeField]
    TextMeshProUGUI answersDoneTextbox;

    [Header("Prefabs")]
    [SerializeField]
    GameObject answerButtonPrefab;

    private void AnswerDoneSet(int oldValue, int newValue)
    {
        answersDoneTextbox.text = newValue.ToString();
    }

    private void OnEnable()
    {
        if (hasAuthority)
        {
            CmdStartQuestions();
        }
    }

    [Command]
    void CmdStartQuestions()
    {
        correctAnswers = 0;
        answersDone = 0;
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
        progressSlider.value = correctAnswers;
        progressSlider.maxValue = correctAnswersNeeded;
    }

    [ClientRpc]
    void RpcOnQuestionsEnded()
    {
        gameObject.SetActive(false);
        PuzzleCompletion.instance.MarkCompleted(PuzzleId.TheoryVariety);
    }

    [Command]
    void CmdSendAnswer(int answerIndex)
    {
        answersDone++;
        if(questions.questions[currentAnswer].correctAnswerIndex == answerIndex)
        {
            correctAnswers++;
            if(correctAnswers >= correctAnswersNeeded)
            {
                RpcOnQuestionsEnded();
            }
        }
        SetNewQuestion();
        RpcSetProgress();
    }
}
