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
        if(Application.platform == RuntimePlatform.WebGLPlayer)
        {
            string script = $"window.open(\"{url}\")";
#pragma warning disable CS0618 // El tipo o el miembro están obsoletos
            Application.ExternalEval(script);
#pragma warning restore CS0618 // El tipo o el miembro están obsoletos
        } else
        {
            Application.OpenURL(url);
        }
    }
}
