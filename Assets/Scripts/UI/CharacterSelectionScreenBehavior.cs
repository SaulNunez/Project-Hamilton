using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectionScreenBehavior : MonoBehaviour
{
    public GameObject playerSelectionScreen;
    public GameObject joinLobbyScreen;
    public GameObject normalGameUI;
    public GameObject movementUI;
    public GameObject itemUI;
    public GameObject loadingScreen;

    void Start()
    {
        HamiltonHub.Instance.OnEnteredLobby += OpenPlayerSelection;
        HamiltonHub.Instance.OnMoveRequest += Instance_OnMoveRequest;
        HamiltonHub.Instance.OnMoveUpdate += Instance_OnMoveUpdate;
        HamiltonHub.Instance.OnCurrentPlayerSelectedCharacter += Instance_OnCurrentPlayerSelectedCharacter;
        HamiltonHub.Instance.OnGameHasStarted += Instance_OnGameHasStarted;

        joinLobbyScreen.SetActive(true);
    }

    private void Instance_OnGameHasStarted()
    {
        loadingScreen.SetActive(false);
        normalGameUI.SetActive(true);
    }

    private void Instance_OnCurrentPlayerSelectedCharacter()
    {
        playerSelectionScreen.SetActive(false);
        loadingScreen.SetActive(true);
    }

    private void Instance_OnMoveUpdate(Assets.Scripts.Multiplayer.ServerRequestsPayload.MovementRequest moveInfo)
    {
        if (movementUI.activeSelf && moveInfo.Character == HamiltonHub.Instance.SelectedCharacter)
        {
            movementUI.SetActive(false);
            normalGameUI.SetActive(true);
        }
    }

    private void Instance_OnMoveRequest(Assets.Scripts.Multiplayer.ResultPayload.AvailableMovementOptions options)
    {
        normalGameUI.SetActive(false);
        movementUI.SetActive(true);
    }

    private void OnDestroy()
    {
        HamiltonHub.Instance.OnEnteredLobby -= OpenPlayerSelection;
        HamiltonHub.Instance.OnMoveRequest -= Instance_OnMoveRequest;
        HamiltonHub.Instance.OnMoveUpdate -= Instance_OnMoveUpdate;
        HamiltonHub.Instance.OnCurrentPlayerSelectedCharacter -= Instance_OnCurrentPlayerSelectedCharacter;
        HamiltonHub.Instance.OnGameHasStarted -= Instance_OnGameHasStarted;
    }

    private void OpenPlayerSelection(string lobbyCode)
    {
        print("Showing player screen");
        joinLobbyScreen.SetActive(false);
        playerSelectionScreen.SetActive(true);

    }

    public void OpenItemScreen()
    {
        normalGameUI.SetActive(false);
        itemUI.SetActive(true);
    }

    public void CloseItemScreen()
    {
        normalGameUI.SetActive(true);
        itemUI.SetActive(false);
    }
}
