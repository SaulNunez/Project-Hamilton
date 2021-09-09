using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionPuzzle : PuzzleBase
{
    [SerializeField]
    PuzzleId puzzleId;

    [SerializeField]
    List<AnswersForPuzzle> answers = new List<AnswersForPuzzle>();

    public override void OnStartClient()
    {
        base.OnStartClient();

        for(int i=0; i < answers.Count; i++)
        {
            int index = i;
            answers[index].buttonForSelectingAnswer.onClick.AddListener(() => {
                CmdCheckAnswer(index);
            });
        }
    }

    [Command(requiresAuthority = false)]
    void CmdCheckAnswer(int answerIndex, NetworkConnectionToClient sender = null)
    {
        if (answers[answerIndex].isACorrectAnswer)
        {
            PuzzleCompletion.instance.MarkCompleted(puzzleId, sender.identity);
            TargetClosePuzzle(sender);
            TargetRunCorrectFeedback(sender);
        } else
        {
            TargetRunWrongFeedback(sender);
        }
    }
}
