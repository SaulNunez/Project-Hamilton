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

    [Command]
    void CmdCerezasClick()
    {
        if (isCake)
        {
            PuzzleCompletion.instance.MarkCompleted(PuzzleId.IfElse);
        }
    }

    public void OnChispitasClick() => CmdChispitasClick();

    [Command]
    void CmdChispitasClick()
    {
        if (!isCake)
        {
            PuzzleCompletion.instance.MarkCompleted(PuzzleId.IfElse);
        }
    }
}
