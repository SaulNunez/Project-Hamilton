using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "puzzle", menuName = "Puzzle/Create puzzle", order = 1)]
public class Puzzle : ScriptableObject
{
    public enum Type
        {
            Variables,
            Conditionals,
            Cycles,
            Functions
        }

    public enum Class {
        A = 1,
        B = 2,
        C = 3
    }

    public string id;
    public Type type;

    public Class puzzleClass;

    [TextArea]
    public string instructions;
    public TextAsset defaultWorkspace;

    public string expectedOutput;

    public List<FunctionCheck> functionChecks;
}
