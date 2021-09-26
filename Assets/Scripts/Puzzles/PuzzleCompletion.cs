using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Keeps tabs on puzzle completion and handles events
/// </summary>
public class PuzzleCompletion : NetworkBehaviour
{
    /// <summary>
    /// Called on puzzle completion. Called when anyone finishes, not just a specific player
    /// </summary>
    /// <remarks>
    /// <strong>Only applicable on the server</strong>
    /// </remarks>
    public static event Action<PuzzleId, NetworkIdentity> OnPuzzleCompleted;

    /// <summary>
    /// Called when all puzzles are solved
    /// </summary>
    public static event Action OnFinishedAllPuzzles;

    public SyncList<PuzzleCompletionInfo> puzzlesCompleted = new SyncList<PuzzleCompletionInfo>();

    [Serializable]
    public class PuzzleCompletionInfo
    {
        public PuzzleId Id;
        public NetworkIdentity netIdentity;
    }

    //private void Start()
    //{
    //    puzzlesCompleted.Callback += OnPuzzleComplete;
    //}

    //private void OnPuzzleComplete(SyncList<PuzzleId>.Operation op, int itemIndex, PuzzleId oldItem, PuzzleId newItem)
    //{
    //    if(op == SyncList<PuzzleId>.Operation.OP_ADD)
    //    {

    //    }
    //}

    /// <summary>
    /// 
    /// </summary>
    public int PuzzlesAvailable
    {
        get
        {
            var players = GameObject.FindGameObjectsWithTag(Tags.Player);
            var programmersInGame = players.ToList().Count(x =>
            {
                var killingComponent = x.GetComponent<Killing>();
                return killingComponent && !killingComponent.IsAssasin;
            });

            return Enum.GetNames(typeof(PuzzleId)).Length * programmersInGame;
        }
    }

    public static PuzzleCompletion instance = null;

    public override void OnStartServer()
    {
        base.OnStartServer();

        if(instance == null)
        {
            instance = this;
        }
    }

    public override void OnStopServer()
    {
        base.OnStopServer();

        instance = null;
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        if (instance == null)
        {
            instance = this;
        }
    }

    public override void OnStopClient()
    {
        base.OnStopClient();
        instance = null;
    }

    public int PuzzlesDone { get => puzzlesCompleted.Count; }

    /// <summary>
    /// Register puzzle as completed
    /// </summary>
    /// <param name="id"></param>
    [Server]
    public void MarkCompleted(PuzzleId id, NetworkIdentity doneByPlayer)
    {
        OnPuzzleCompleted?.Invoke(id, doneByPlayer);
        puzzlesCompleted.Add(new PuzzleCompletionInfo
        {
            Id = id,
            netIdentity = doneByPlayer
        });

        if(PuzzlesDone == PuzzlesAvailable)
        {
            OnFinishedAllPuzzles?.Invoke();
        }
    }
}
