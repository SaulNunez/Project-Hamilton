using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenUrl : MonoBehaviour
{
    [Tooltip("URL to open")]
    [TextArea]
    public string url;

    public void RedirectToUrl()
    {
        Application.OpenURL(url);
    }
}
