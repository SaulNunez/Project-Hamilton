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

    public GameObject characterListContainer;
    public GameObject characterRadioPrefab;
    public Text errorTextbox;
    public GameObject errorPanel;
    public ToggleGroup characterListToggleGroup;

    List<CharacterData> charactersAvailable;

    public void OnScreenNameEdit(string newVal)
    {
        playerName = newVal;
    }

    private async void OnEnable()
    {
        charactersAvailable = (await HamiltonHub.Instance.GetAvailableCharactersInLobby()).available;
        ShowCharactersOnScreen();
    }

    private void Start()
    {
        HamiltonHub.Instance.OnCharacterAvailableChanged += Instance_onCharacterAvailableChanged;
    }

    private void Instance_onCharacterAvailableChanged(List<CharacterData> characters)
    {
        charactersAvailable = characters;
        ShowCharactersOnScreen();
    }

    private void ShowCharactersOnScreen()
    {
        //Restablecer lista de jugadores en pantalla
        foreach (Transform gmTransform in characterListContainer.GetComponentsInChildren<Transform>())
        {
            if(gmTransform != characterListContainer.transform)
            {
                Destroy(gmTransform.gameObject);
            }
        }

        foreach (var characterData in charactersAvailable)
        {
            var itemGameObject = Instantiate(characterRadioPrefab);
            var characterItem = itemGameObject.GetComponent<CharacterRadioButton>();

            characterItem.Toggle.onValueChanged.AddListener((selected) =>
            {
                if (selected)
                {
                    characterSelection = characterData.id;
                }
            });

            print(characterData.name);
            characterItem.Toggle.group = characterListToggleGroup;
            characterItem.CharacterNameText.text = characterData.name;
            characterItem.DescriptionText.text = characterData.description;
            characterItem.StatsText.text = $"Valentía: {characterData.stats.Bravery} Inteligencia: {characterData.stats.Intelligence} Sanidad: {characterData.stats.Sanity} Fisico: {characterData.stats.Physical}";

            itemGameObject.transform.SetParent(characterListContainer.transform);
        }
    }

    private void OnDestroy()
    {
        HamiltonHub.Instance.OnCharacterAvailableChanged -= Instance_onCharacterAvailableChanged;
    }

    public async void SendSelectedCharacterToServer()
    {
        if (await HamiltonHub.Instance.SelectCharacter(playerName, characterSelection) == null)
        {
            errorTextbox.text = "Algo ocurrio mal, porfavor selecciona tu personaje nuevamente";
            errorPanel.SetActive(true);

            charactersAvailable = (await HamiltonHub.Instance.GetAvailableCharactersInLobby()).available;
            ShowCharactersOnScreen();
        }
    }

    public void CloseErrorBox()
    {
        errorPanel.SetActive(false);
    }
}
