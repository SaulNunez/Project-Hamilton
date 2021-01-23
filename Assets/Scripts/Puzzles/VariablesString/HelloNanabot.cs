using Mirror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HelloNanabot : NetworkBehaviour
{
    public TMP_InputField textbox;

    public void UpdateNanabot()
    {
        var nanaBot = GameObject.Find("Nanabot");
        var controls = nanaBot.GetComponent<NanaBot>();
        controls.Talk($"Hola {textbox.text}");
    }
}
