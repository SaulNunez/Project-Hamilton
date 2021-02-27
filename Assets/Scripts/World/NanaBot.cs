using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NanaBot : MonoBehaviour
{
    [SerializeField]
    TextMesh worldText;

    [SerializeField]
    int showForSeconds = 4;

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
