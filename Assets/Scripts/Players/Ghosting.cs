using Extensions;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghosting : NetworkBehaviour
{
    [SerializeField]
    public int ghostingDurationInSeconds = 20;

    /// <summary>
    /// Use int so they don't loose a ghost mode invocation because they did two puzzles before using them
    /// </summary>
    [SyncVar]
    int ghostModeUsesAvailable = 0;

    [SyncVar(hook = nameof(SetGhostModeOnPlayer))]
    bool isOnGhostMode = false;

    new SpriteRenderer renderer;
    readonly Color transparent = new Color(0, 0, 0, 0.1f);
    readonly Color ghostVisibleColor = new Color(1, 1, 1, 0.75f);

    Killing assasinInformationComponent;

    public override void OnStartClient()
    {
        base.OnStartClient();

        renderer = GetComponent<SpriteRenderer>();

        GameUI.OnGhostingInvoked += CmdTryToBeGhost;
    }

    public override void OnStopClient()
    {
        base.OnStopClient();

        GameUI.OnGhostingInvoked -= CmdTryToBeGhost;
    }

    public override void OnStartServer()
    {
        base.OnStartServer();

        PuzzleCompletion.OnPuzzleCompleted += AddToAvailableGhostModesOnCompleted;
        assasinInformationComponent = GetComponent<Killing>();
    }

    public override void OnStopServer()
    {
        base.OnStopServer();

        PuzzleCompletion.OnPuzzleCompleted -= AddToAvailableGhostModesOnCompleted;
    }

    private void AddToAvailableGhostModesOnCompleted(PuzzleId id, NetworkIdentity solvedBy)
    {
        bool puzzleWasSolvedByPlayer = netIdentity == solvedBy;
        bool isAssasin = assasinInformationComponent.IsAssasin;

        //If something is solved by this player, add
        if (isAssasin && puzzleWasSolvedByPlayer)
        {
            ghostModeUsesAvailable++;
        }
    }

    void SetGhostModeOnPlayer(bool oldValue, bool newValue)
    {
        var isOnGhostMode = newValue == true;
        if (isOnGhostMode)
        {
            if (hasAuthority)
            {
                renderer.color = ghostVisibleColor;
            } else
            {
                renderer.color = transparent;
            }
        } else
        {
            renderer.color = Color.white;
        }
    }

    void GhostModesAvailableChanged(int oldValue, int newValue)
    {
        if (hasAuthority)
        {
            bool hasAvailableGhosting = newValue > 0;
            GameUI.Instance.CanInteractWithGhostingButton = hasAvailableGhosting;
        }
    }

    [Command]
    void CmdTryToBeGhost()
    {
        bool canUseGhostMode = ghostModeUsesAvailable > 0;

        if (!canUseGhostMode)
        {
            this.SuperPrint("This player doesn't have any ghost mode available");
        }

        if (!isOnGhostMode && canUseGhostMode)
        {
            ghostModeUsesAvailable--;
            isOnGhostMode = true;
            Invoke(nameof(ServerTurnOffGhosting), ghostingDurationInSeconds);
        }
    }

    [Server]
    void ServerTurnOffGhosting()
    {
        isOnGhostMode = false;
    }
}
