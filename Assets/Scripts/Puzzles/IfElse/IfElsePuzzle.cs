using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IfElsePuzzle : NetworkBehaviour
{
    [SyncVar(hook = nameof(SetupSprite))]
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

    void SetupSprite(bool oldValue, bool newValue)
    {
        toDecoratedImage.sprite = newValue? cake: cupcake;
    }

    //Pastel se decora con cerezas, cupcake con chispitas

    public void OnCerezasClick() => CmdCerezasClick();

    //TODO: Future work, do check in only one function

    [Command(ignoreAuthority = true)]
    void CmdCerezasClick(NetworkConnectionToClient sender = null)
    {
        if (isCake)
        {
            PuzzleCompletion.instance.MarkCompleted(PuzzleId.IfElse);
            TargetClosePuzzle(sender);
        }
    }

    public void OnChispitasClick() => CmdChispitasClick();

    [Command(ignoreAuthority = true)]
    void CmdChispitasClick(NetworkConnectionToClient sender = null)
    {
        if (!isCake)
        {
            PuzzleCompletion.instance.MarkCompleted(PuzzleId.IfElse);
            TargetClosePuzzle(sender);
        }
    }

    [TargetRpc]
    void TargetClosePuzzle(NetworkConnection target)
    {
        ShowPuzzle.instance.ClosePuzzles();
    }
}
