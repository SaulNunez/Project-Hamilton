using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class Test : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void ShowEditor(int id, string initialStateXml, int checkType, string variablesExpected);

    [DllImport("__Internal")]
    private static extern void CloseCodeEditor();

    private bool open = false;

    public void OnClick()
    {
        if (open)
        {
            CloseCodeEditor();
        } else
        {
            ShowEditor(1, null, 0, null);
        }

        open = !open;
    }
}
