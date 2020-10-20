using UnityEngine;
using UnityEngine.UI;

public class CharacterMovement : MonoBehaviour
{
    public GameObject ui;
    public Button rightButton;
    public Button leftButton;
    public Button upButton;
    public Button downButton;

    private Assets.Scripts.Multiplayer.ResultPayload.AvailableMovementOptions options;

    public bool Shown
    {
        get => ui.activeInHierarchy;
    }

    private void Start()
    {
        HamiltonHub.Instance.OnMoveRequest += Instance_OnMoveRequest;
    }

    private void Instance_OnMoveRequest(Assets.Scripts.Multiplayer.ResultPayload.AvailableMovementOptions options)
    {
        ShowMovementUI();
        this.options = options;
    }

    private void OnDestroy()
    {
        HamiltonHub.Instance.OnMoveRequest -= Instance_OnMoveRequest;
    }

    public async void MoveUp()
    {
        await HamiltonHub.Instance
            .SendPlayerWantedDirection(Assets.Scripts.Players.Direction.Up);
        HideMovementUI();
    }

    public async void MoveDown()
    {
        await HamiltonHub.Instance
            .SendPlayerWantedDirection(Assets.Scripts.Players.Direction.Down);
        HideMovementUI();
    }

    public async void MoveLeft()
    {
        await HamiltonHub.Instance
            .SendPlayerWantedDirection(Assets.Scripts.Players.Direction.Left);
        HideMovementUI();
    }

    public async void MoveRight()
    {
        await HamiltonHub.Instance
            .SendPlayerWantedDirection(Assets.Scripts.Players.Direction.Right);
        HideMovementUI();
    }

    private void ShowMovementUI() => ui.SetActive(true);

    private void HideMovementUI() => ui.SetActive(false);

    private void Update()
    {
        if (Shown)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                MoveLeft();
                HideMovementUI();
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                MoveRight();
                HideMovementUI();
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                MoveUp();
                HideMovementUI();
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                MoveDown();
                HideMovementUI();
            }
        }
    }
}
