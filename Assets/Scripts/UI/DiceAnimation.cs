using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class DiceAnimation : MonoBehaviour
{
    public Text text;
    public GameObject canvas;

    void Start()
    {
        HamiltonHub.Instance.OnThrowDice += Instance_OnThrowDice;
    }

    private async void Instance_OnThrowDice(int result)
    {
        canvas.SetActive(true);
        text.text = result.ToString();
        Thread.Sleep(5000);
        canvas.SetActive(false);
    }

    // Update is called once per frame
    void OnDestroy()
    {
        HamiltonHub.Instance.OnThrowDice -= Instance_OnThrowDice;
    }
}
