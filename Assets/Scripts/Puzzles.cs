using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class Puzzles : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void ShowEditor(string initialStateXml, 
        string instructions, string documentation, 
        string gameobjectWithCallback, string functionCallbackName);

    [DllImport("__Internal")]
    private static extern void SetErrorInCodeEditor(string errors);

    [DllImport("__Internal")]
    private static extern void SetOutputInEditor(string output);

    [DllImport("__Internal")]
    private static extern void CloseCodeEditor();
}
