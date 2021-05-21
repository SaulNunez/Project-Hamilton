using Mirror;
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
public class SetupDefaultFlowers: NetworkBehaviour {
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

    [Header("Buttons")]
    [SerializeField]
    List<FlowerButtonModel> flowerButtons;

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

        var buttonFlower = FlowerTypes.Types.PickRandom(6).ToList();
        RpcSetupOnClient(buttonFlower);

        print("Puzzle started");
    }

    [Command(ignoreAuthority = true)]
    void CmdOnButtonClick(string clickFlowerType, NetworkConnectionToClient sender = null)
    {
        if(clickFlowerType == defaultFlowerType)
        {
            PuzzleCompletion.instance.MarkCompleted(PuzzleId.IfFlowerPicking);
            TargetClosePuzzle(sender);
        }
    }

    [ClientRpc]
    void RpcSetupOnClient(List<string> flowerTypes)
    {
        print("Empezando configuracion");
        for(int i = 0; i < flowerButtons.Count; i++)
        {
            print($"Configurando numero {i}");
            switch (flowerTypes[i])
            {
                case FlowerTypes.Sunflower:
                    flowerButtons[i].ButtonSprite = sunflowerSprite;
                    break;
                case FlowerTypes.Tulip:
                    flowerButtons[i].ButtonSprite = tulipSprite;
                    break;
                case FlowerTypes.Roses:
                    flowerButtons[i].ButtonSprite = rosesSprite;
                    break;
                case FlowerTypes.Daisy:
                    flowerButtons[i].ButtonSprite = daisySprite;
                    break;
            }

            //En click, mandar seleccion al servidor
            flowerButtons[i].button.onClick.AddListener(() => CmdOnButtonClick(flowerTypes[i]));
        }
    }

    [TargetRpc]
    void TargetClosePuzzle(NetworkConnection target)
    {
        PuzzleUI.instance.ClosePuzzles();
    }
}
