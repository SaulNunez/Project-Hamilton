using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// To be appended to a Nanabot NPC gameobject. Provides an interface to talk back to the user.
/// Has text floating on top of her head.
/// </summary>
public class NanaBot : MonoBehaviour
{
    [SerializeField]
    TextMesh worldText;

    [SerializeField]
    int showForSeconds = 4;

    /// <summary>
    /// Talk something to the user. Will be written on top of Nanabot. As she would have said so.
    /// </summary>
    /// <param name="text">The text to show to the user</param>
    public void Talk(string text)
    {
        worldText.text = text;
        worldText.gameObject.SetActive(true);
        Invoke(nameof(CloseWorldText), showForSeconds);
    }

    void CloseWorldText()
    {
        worldText.gameObject.SetActive(false);
    }
}
