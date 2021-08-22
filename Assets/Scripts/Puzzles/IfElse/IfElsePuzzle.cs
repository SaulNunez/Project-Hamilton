using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IfElsePuzzle : NetworkBehaviour
{
    [SyncVar]
    bool isCake;

    [Header("Puzzle setup")]
    [SerializeField]
    Image toDecoratedImage;
    
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
        if (isCake)
        {
            toDecoratedImage.sprite = cake;
        } else
        {
            toDecoratedImage.sprite = cupcake;
        }
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
        }
    }

    [TargetRpc]
    void TargetClosePuzzle(NetworkConnection target)
    {
        PuzzleUI.instance.ClosePuzzles();
    }
}
