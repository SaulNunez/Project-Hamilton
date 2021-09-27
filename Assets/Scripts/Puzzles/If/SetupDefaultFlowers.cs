using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controls logic for if puzzle.
/// 
/// On server a flower type is decided on start.
/// Also a random selection of six flowers is created and send to client to create buttons for user.
/// On click, these send a message on client to check they are the correct type.
/// </summary>
public class SetupDefaultFlowers: PuzzleBase {
    [SyncVar]
    string defaultFlowerType;

    [SerializeField]
    Image defaultFlowerImage;

    [Header("Sprites")]
    [SerializeField]
    Sprite sunflowerSprite;

    [SerializeField]
    Sprite daisySprite;

    [SerializeField]
    Sprite tulipSprite;

    [SerializeField]
    Sprite rosesSprite;

    public override void OnStartClient()
    {
        base.OnStartClient();

        switch (defaultFlowerType)
        {
            case FlowerTypes.Sunflower:
                defaultFlowerImage.sprite = sunflowerSprite;
                break;
            case FlowerTypes.Tulip:
                defaultFlowerImage.sprite = tulipSprite;
                break;
            case FlowerTypes.Roses:
                defaultFlowerImage.sprite = rosesSprite;
                break;
            case FlowerTypes.Daisy:
                defaultFlowerImage.sprite = daisySprite;
                break;
        }
    }

    public override void OnStartServer()
    {
        base.OnStartServer();

        defaultFlowerType = FlowerTypes.Types.PickRandom();
    }

    public void ClickSunflowerButton() => CmdOnButtonClick(FlowerTypes.Sunflower);
    public void ClickTulipButton() => CmdOnButtonClick(FlowerTypes.Tulip);
    public void ClickRosesButton() => CmdOnButtonClick(FlowerTypes.Roses);
    public void ClickDaisyButton() => CmdOnButtonClick(FlowerTypes.Daisy);


    [Command(requiresAuthority = false)]
    void CmdOnButtonClick(string clickFlowerType, NetworkConnectionToClient sender = null)
    {
        if(clickFlowerType == defaultFlowerType)
        {
            PuzzleCompletion.instance.MarkCompleted(PuzzleId.IfFlowerPicking, sender.identity);
            TargetClosePuzzle(sender);
            TargetRunCorrectFeedback(sender);
        } else
        {
            TargetRunWrongFeedback(sender);
        }
    }
}
