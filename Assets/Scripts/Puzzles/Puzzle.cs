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

    public string id;
    public Type type;
    [TextArea]
    public string instructions;
    public TextAsset defaultWorkspace;

    public string expectedOutput;
    
}
