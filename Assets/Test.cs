using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class Test : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void ShowEditor(string initialStateXml, int checkType, Dictionary<string, object> variablesExpected);

}
