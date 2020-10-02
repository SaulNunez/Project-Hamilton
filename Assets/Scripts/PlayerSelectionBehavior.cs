using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class PlayerSelectionBehavior : MonoBehaviour
{
    [HideInInspector]
    public string playerName;

    [HideInInspector]
    public string characterSelection;

    public GameObject characterGrid;
    public GameObject characterButtonPrefab;
    
    
    public Text errorTextbox;
    public GameObject errorPanel;

    List<CharacterData> charactersAvailable;

    public void OnScreenNameEdit(string newVal)
    {
        playerName = newVal;
    }

    private async void OnEnable()
    {
        charactersAvailable = await HamiltonHub.Instance.GetAvailableCharactersInLobby();
    }

    private void OnDestroy()
    {
        //Socket.AvailableCharactersUpdate -= NewCharactersUpdate;
        //Socket.PlayerSelectedCharacter -= CharacterSelected;
    }

    //private void NewCharactersUpdate(AvailableCharactersData data, string error)
    //{

    //    print("Update received to player information");

    //    if(data != null)
    //    {
    //        charactersAvailable = data.charactersAvailable;

    //        //Restablecer lista de jugadores en pantalla
    //        foreach (Transform gmTransform in GetComponentInChildren<Transform>())
    //        {
    //            Destroy(gmTransform.gameObject);
    //        }

    //        foreach (var characterData in charactersAvailable)
    //        {
    //            var gameObject = Instantiate(characterButtonPrefab, transform);
    //            var button = gameObject.GetComponent<Button>();

    //            if(button != null)
    //            {
    //                button.onClick.AddListener(() =>
    //                {
    //                    characterSelection = characterData.prototypeId;
    //                });
    //            }

    //            var text = gameObject.GetComponent<Text>();
    //            if(text != null)
    //            {
    //                text.text = characterData.name;
    //            }
    //        }
    //    }
    //    else
    //    {
    //        Debug.LogError(error);
    //    }
    //}

    private void CharacterSelected(string error)
    {
        if(error != null)
        {
            errorTextbox.text = error;
            errorPanel.SetActive(true);
        }

    }

    public void SendSelectedCharacterToServer()
    {
        //Socket.Instance.SelectCharacter(new SelectCharacterPayload
        //{
        //    displayName = playerName,
        //    character = characterSelection
        //});
    }

    public void CloseErrorBox()
    {
        errorPanel.SetActive(false);

        //Reactualizar lista, por si el error fue que ese jugador ya se ha ocupado
        //Socket.Instance.GetAvailableCharacters();
    }
}
