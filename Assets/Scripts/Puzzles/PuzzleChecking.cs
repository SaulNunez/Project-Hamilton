using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class PuzzleChecking : NetworkBehaviour
{
    private const string puzzleCheckMicroservice = "https://hamilton-microservice.herokuapp.com/";

    public IEnumerator CheckPuzzle(Puzzle puzzle, string playerAnswer, Action<PuzzleCheckResult> onPuzzleCheckedCallback)
    {
        string checkType = "";
        switch (puzzle.type)
        {
            case Puzzle.Type.Variables:
                //Ahorita es inutil
                checkType = "check_for_variables";
                break;
            case Puzzle.Type.Conditionals:
                checkType = "check_for_branching";
                break;
            case Puzzle.Type.Cycles:
                checkType = "check_for_loops";
                break;
            case Puzzle.Type.Functions:
                checkType = "check_for_functions";
                break;
        }

        var json = JsonUtility.ToJson(new
        {
            code = playerAnswer,
            puzzle.expectedOutput,
            checkType,
            functionCheck = puzzle.functionChecks.Select(fc => new
            {
                fc.name,
                parameters = fc.parameters.Select(p => p.Value),
                fc.output
            })
        });

        using (UnityWebRequest request = UnityWebRequest.Post(puzzleCheckMicroservice, json))
        {
            yield return request.SendWebRequest();

            if (request.isNetworkError)
            {
                Debug.LogError(request.error);
            }
            else
            {
                var result = request.downloadHandler.text;
                var resultObj = JsonUtility.FromJson<PuzzleCheckResult>(result);
                onPuzzleCheckedCallback(resultObj);
            }
        }
    }
}
