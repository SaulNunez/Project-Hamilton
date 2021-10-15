using Mirror;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Serialization;

/// <summary>
/// Controls logic for if puzzle.
/// 
/// On server a flower type is decided on start.
/// Also a random selection of six flowers is created and send to client to create buttons for user.
/// On click, these send a message on client to check they are the correct type.
/// </summary>
public class SetupDefaultFlowers: PuzzleBase {
    [SyncVar]
    string foundFlowerType;
    [SyncVar]
    string codeFlowerType;

    [SerializeField]
    [FormerlySerializedAs("defaultFlowerImage")]
    Image foundFlowerImage;
    [SerializeField]
    TMP_Text foundFlowerText;
    [SerializeField]
    TMP_Text flowerTextInCode;
    [SerializeField]
    Button selectFlowerHappensButton;
    [SerializeField]
    Button nothingHappensButton;

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

        switch (foundFlowerType)
        {
            case FlowerTypes.Sunflower:
                foundFlowerImage.sprite = sunflowerSprite;
                break;
            case FlowerTypes.Tulip:
                foundFlowerImage.sprite = tulipSprite;
                break;
            case FlowerTypes.Roses:
                foundFlowerImage.sprite = rosesSprite;
                break;
            case FlowerTypes.Daisy:
                foundFlowerImage.sprite = daisySprite;
                break;
        }

        switch (foundFlowerType)
        {
            case FlowerTypes.Sunflower:
                foundFlowerText.text = "Girasol";
                break;
            case FlowerTypes.Tulip:
                foundFlowerText.text = "Tulipan";
                break;
            case FlowerTypes.Roses:
                foundFlowerText.text = "Rosa";
                break;
            case FlowerTypes.Daisy:
                foundFlowerText.text = "Margarita";
                break;
        }

        switch (codeFlowerType)
        {
            case FlowerTypes.Sunflower:
                flowerTextInCode.text = "Girasol";
                break;
            case FlowerTypes.Tulip:
                flowerTextInCode.text = "Tulipan";
                break;
            case FlowerTypes.Roses:
                flowerTextInCode.text = "Rosa";
                break;
            case FlowerTypes.Daisy:
                flowerTextInCode.text = "Margarita";
                break;
        }
    }

    public override void OnStartServer()
    {
        base.OnStartServer();

        foundFlowerType = FlowerTypes.Types.PickRandom();
        codeFlowerType = FlowerTypes.Types.PickRandom();
    }

    public void ClickSunflowerButton() => CmdOnButtonClick(FlowerTypes.Sunflower);
    public void ClickTulipButton() => CmdOnButtonClick(FlowerTypes.Tulip);
    public void ClickRosesButton() => CmdOnButtonClick(FlowerTypes.Roses);
    public void ClickDaisyButton() => CmdOnButtonClick(FlowerTypes.Daisy);


    [Command(requiresAuthority = false)]
    void CmdOnButtonClick(string clickFlowerType, NetworkConnectionToClient sender = null)
    {
        if(clickFlowerType == foundFlowerType)
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
