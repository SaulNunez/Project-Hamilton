using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class DiceAnimation : MonoBehaviour
{
    public Text text;
    public GameObject canvas;

    private async void OnDiceThrow(int result)
    {
        canvas.SetActive(true);
        text.text = result.ToString();
        Thread.Sleep(5000);
        canvas.SetActive(false);
    }
}
