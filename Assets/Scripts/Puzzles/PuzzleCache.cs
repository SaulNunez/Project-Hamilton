using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleCache: MonoBehaviour
{
    public readonly List<Puzzle> puzzles = new List<Puzzle>();

    [HideInInspector]
    public static PuzzleCache Instance = null;

    void Start(){
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("More than one instance of PuzzleCache found.");
            Destroy(this);
        }
        puzzles.AddRange(Resources.LoadAll<Puzzle>("Puzzles"));
    }

    void OnDestroy(){
        Instance = null;
    }
}
