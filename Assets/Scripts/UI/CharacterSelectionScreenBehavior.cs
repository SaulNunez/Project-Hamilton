using Assets.Scripts.Multiplayer.ResultPayload;
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

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();

        HamiltonHub.Instance.OnEnteredLobby += OpenPlayerSelection;
        HamiltonHub.Instance.OnMoveRequest += Instance_OnMoveRequest;
        HamiltonHub.Instance.OnMoveUpdate += Instance_OnMoveUpdate;
        HamiltonHub.Instance.OnCurrentPlayerSelectedCharacter += Instance_OnCurrentPlayerSelectedCharacter;
        HamiltonHub.Instance.OnGameHasStarted += Instance_OnGameHasStarted;

        //joinLobbyScreen.SetActive(true);
    }

    private void Instance_OnGameHasStarted(GameStartPayload gameStartInfo)
    {
        //loadingScreen.SetActive(false);
        //normalGameUI.SetActive(true);

        animator.SetBool("GameHasStarted", true);
    }

    private void Instance_OnCurrentPlayerSelectedCharacter()
    {
        //playerSelectionScreen.SetActive(false);
        //loadingScreen.SetActive(true);

        animator.SetBool("SelectedCharacter", true);
    }

    private void Instance_OnMoveUpdate(Assets.Scripts.Multiplayer.ServerRequestsPayload.MovementRequest moveInfo)
    {
        if (movementUI.activeSelf && moveInfo.Character == HamiltonHub.Instance.SelectedCharacter)
        {
            movementUI.SetActive(false);
            normalGameUI.SetActive(true);
        }
    }

    private void Instance_OnMoveRequest(AvailableMovementOptions options)
    {
        //normalGameUI.SetActive(false);
        //movementUI.SetActive(true);

        animator.SetTrigger("MakeMovement");
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
        animator.SetBool("EnteredLobby", true);

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
