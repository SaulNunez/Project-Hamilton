using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SetupDefaultFlowers: NetworkBehaviour {
    [SyncVar(hook = nameof(OnDefaultFlowerSet))]
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


    void OnDefaultFlowerSet(string oldValue, string newValue)
    {
        switch (newValue)
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
        SetupOnClient(buttonFlower);
    }

    [Command]
    void CmdOnButtonClick(string clickFlowerType)
    {
        if(clickFlowerType == defaultFlowerType)
        {
            PuzzleCompletion.instance.MarkCompleted(PuzzleId.IfFlowerPicking);
            RpcClosePuzzle();
        }
    }

    [ClientRpc]
    void SetupOnClient(List<string> flowerTypes)
    {
        for(int i = 0; i < flowerButtons.Count; i++)
        {
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

    [ClientRpc]
    void RpcClosePuzzle()
    {
        gameObject.SetActive(false);
    }
}
