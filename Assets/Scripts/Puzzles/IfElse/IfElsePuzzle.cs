using Mirror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IfElsePuzzle : PuzzleBase
{
    [SyncVar]
    bool isCake;

    [Header("Puzzle setup")]
    [SerializeField]
    TMP_Text isCakeTextValue;
    
    [Header("Sprites")]
    [SerializeField]
    Sprite cake;

    [SerializeField]
    Sprite cupcake;

    public override void OnStartServer()
    {
        base.OnStartServer();

        //Nota, desconozco si la distribucion es igual entre todos los numeros
        //Si es 0.5 para arriba, es un pastel, menor es un cupcake
        isCake = Mathf.RoundToInt(Random.value) == 1;
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        isCakeTextValue.text = isCake ? "Verdero" : "Falso";
    }

    //Pastel se decora con cerezas, cupcake con chispitas

    public void OnCerezasClick() => CmdCerezasClick();

    //TODO: Future work, do check in only one function

    [Command(requiresAuthority = false)]
    void CmdCerezasClick(NetworkConnectionToClient sender = null)
    {
        if (isCake)
        {
            PuzzleCompletion.instance.MarkCompleted(PuzzleId.IfElse, sender.identity);
            TargetClosePuzzle(sender);
            TargetRunCorrectFeedback(sender);
        } else
        {
            TargetRunWrongFeedback(sender);
        }
    }

    public void OnChispitasClick() => CmdChispitasClick();

    [Command(requiresAuthority = false)]
    void CmdChispitasClick(NetworkConnectionToClient sender = null)
    {
        if (!isCake)
        {
            PuzzleCompletion.instance.MarkCompleted(PuzzleId.IfElse, sender.identity);
            TargetClosePuzzle(sender);
            TargetRunCorrectFeedback(sender);
        }
        else
        {
            TargetRunWrongFeedback(sender);
        }
    }
}
