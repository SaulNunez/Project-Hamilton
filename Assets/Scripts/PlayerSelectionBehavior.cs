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

    private void Start()
    {
        HamiltonHub.Instance.OnCharacterAvailableChanged += Instance_onCharacterAvailableChanged;
    }

    private void Instance_onCharacterAvailableChanged(List<CharacterData> characters)
    {
        charactersAvailable = characters;

        //Restablecer lista de jugadores en pantalla
        foreach (Transform gmTransform in GetComponentInChildren<Transform>())
        {
            Destroy(gmTransform.gameObject);
        }

        foreach (var characterData in charactersAvailable)
        {
            var gameObject = Instantiate(characterButtonPrefab, transform);
            var button = gameObject.GetComponent<Button>();

            if (button != null)
            {
                button.onClick.AddListener(() =>
                {
                    characterSelection = characterData.id;
                });
            }

            var text = gameObject.GetComponent<Text>();
            if (text != null)
            {
                text.text = characterData.name;
            }
        }
    }

    private void OnDestroy()
    {
        HamiltonHub.Instance.OnCharacterAvailableChanged -= Instance_onCharacterAvailableChanged;
    }

    public async void SendSelectedCharacterToServer()
    {
        if(await HamiltonHub.Instance.SelectCharacter(playerName, characterSelection) != null)
        {
            errorTextbox.text = "Algo ocurrio mal, porfavor selecciona tu personaje nuevamente";
            errorPanel.SetActive(true);

            charactersAvailable = await HamiltonHub.Instance.GetAvailableCharactersInLobby();
        }
    }

    public void CloseErrorBox()
    {
        errorPanel.SetActive(false);
    }
}
