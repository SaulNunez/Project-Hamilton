using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialog : MonoBehaviour
{
    public GameObject dialogPrefab;
    private GameObject dialogInstance;
    private Text dialogTextInstance;

    public void OpenDialog(string text)
    {
        dialogTextInstance.text = text;
        dialogInstance.SetActive(true);
    }

    public void Close()
    {
        dialogInstance.SetActive(false);
    }

    private void Start()
    {
        dialogInstance = Instantiate(dialogPrefab);
        dialogInstance.SetActive(false);
    }
}
