using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Events;

public class PuzzleManager : NetworkBehaviour
{
    #region InteropJsEnCliente
    [Client]
    [DllImport("__Internal")]
    private static extern void ShowEditor(string initialStateXml,
        string instructions, string documentation,
        string gameobjectWithCallback, string functionCallbackName);

    [Client]
    [DllImport("__Internal")]
    private static extern void SetErrorInCodeEditor(string errors);

    [Client]
    [DllImport("__Internal")]
    private static extern void SetOutputInEditor(string output);

    [Client]
    [DllImport("__Internal")]
    private static extern void CloseCodeEditor();
    #endregion

    private Guid currentPuzzleId;

    public UnityEvent onPuzzleCorrect;


    public SyncDictionary<Guid, PuzzlePlayerInfo> puzzlesInSession = new SyncDictionary<Guid, PuzzlePlayerInfo>();

    public static PuzzleManager Instance = null;

    public override void OnStartClient()
    {
        base.OnStartClient();

        if (Instance == null) Instance = this;
    }

    [Command]
    public void CmdCheckPuzzle(string code, Guid puzzleInstance)
    {
        var puzzleId = puzzlesInSession[puzzleInstance].puzzleId;
        var puzzle = PuzzleCache.Instance.puzzles.Find(x => x.id == puzzleId);
        var puzzleCheckGameObject = GameObject.FindGameObjectWithTag("PuzzleCheck");
        var client = puzzlesInSession[puzzleInstance].involved.GetComponent<NetworkIdentity>().connectionToClient;

        if (puzzleCheckGameObject != null)
        {
            var puzzleCheckSystem = puzzleCheckGameObject.GetComponent<PuzzleChecking>();
            StartCoroutine(
                puzzleCheckSystem.CheckPuzzle(puzzle, code,
                    (puzzleAction) =>
                    {
                        TargetOnPuzzleResult(client, puzzleAction);
                        PuzzleResult(puzzleAction);
                    })
           );
        }
        else
        {
            Debug.LogError("Puzzle couldn't be checked");
        }
    }

    [TargetRpc]
    public void TargetOnPuzzleResult(NetworkConnection target, PuzzleCheckResult result)
    {
        SetOutputInEditor(string.Join("\n", result.runOutput));
        //TODO: Mostrar errores de puzzle del jugador
    }

    [Server]
    void PuzzleResult(PuzzleCheckResult result)
    {
        onPuzzleCorrect.Invoke();
    }

    public override void OnStopClient()
    {
        base.OnStopClient();

        Instance = null;
    }

    public List<string> GetSolvedPuzzleIds(PuzzleInformation puzzlesOfPlayer) =>
        //Como no podemos retornar la lista, buscar todos los que no sean null
        puzzlesOfPlayer.usedPuzzles.FindAll(s => s != null);

    public List<string> GetNotPassedPuzzleIds(PuzzleInformation puzzlesOfPlayer) =>
        //Como no podemos retornar la lista, buscar todos los que no sean null
        puzzlesOfPlayer.failedPuzzles.FindAll(s => s != null);

    //Algo pesado el metodo, pero solo corre de vez en cuando
    [Server]
    public Puzzle GetPuzzle(List<Puzzle> puzzles, List<string> usedPuzzles, List<string> notPassed)
    {
        var passedPuzzles = usedPuzzles.Select(pp => puzzles.Find(p => p.id == pp));
        int variables = passedPuzzles.Count(x => x.type == Puzzle.Type.Variables);
        int conditionals = passedPuzzles.Count(x => x.type == Puzzle.Type.Conditionals);
        int cycles = passedPuzzles.Count(x => x.type == Puzzle.Type.Cycles);
        int functions = passedPuzzles.Count(x => x.type == Puzzle.Type.Functions);

        //Porcentajes de fallo
        var puzzlesSolved = usedPuzzles.Count();
        var failedPuzzles = notPassed.Select(fp => puzzles.Find(p => p.id == fp));
        float variableFailed = (failedPuzzles.Count(x => x.type == Puzzle.Type.Variables) / puzzlesSolved) * 100;
        float conditionalsFailed = (failedPuzzles.Count(x => x.type == Puzzle.Type.Conditionals) / puzzlesSolved) * 100;
        float cyclesFailed = (failedPuzzles.Count(x => x.type == Puzzle.Type.Cycles) / puzzlesSolved) * 100;
        float functionsFailed = (failedPuzzles.Count(x => x.type == Puzzle.Type.Functions) / puzzlesSolved) * 100;

        //Si usuario se equivoca, entonces poner otro puzzle similar para que pueda practicar
        Puzzle selection = null;
        if (variableFailed > 50)
        {
            selection = puzzles.FindAll(p => p.type == Puzzle.Type.Variables && !usedPuzzles.Contains(p.id))
                               .OrderBy(p => p.puzzleClass)
                               .First();
        }
        else if (conditionalsFailed > 50)
        {
            selection = puzzles.FindAll(p => p.type == Puzzle.Type.Conditionals && !usedPuzzles.Contains(p.id))
                               .OrderBy(p => p.puzzleClass)
                               .First();
        }
        else if (cyclesFailed > 50)
        {
            selection = puzzles.FindAll(p => p.type == Puzzle.Type.Cycles && !usedPuzzles.Contains(p.id))
                               .OrderBy(p => p.puzzleClass)
                               .First();
        }
        else if (functionsFailed > 50)
        {
            selection = puzzles.FindAll(p => p.type == Puzzle.Type.Functions && !usedPuzzles.Contains(p.id))
                               .OrderBy(p => p.puzzleClass)
                               .First();
        }

        if (selection == null)
        {
            if (variables == 0)
            {
                selection = puzzles.FindAll(p => p.type == Puzzle.Type.Variables && !usedPuzzles.Contains(p.id))
                               .OrderBy(p => p.puzzleClass)
                               .First();
            }
            else if (conditionals == 0)
            {
                selection = puzzles.FindAll(p => p.type == Puzzle.Type.Conditionals && !usedPuzzles.Contains(p.id))
                               .OrderBy(p => p.puzzleClass)
                               .First();
            }
            else if (cycles == 0)
            {
                selection = puzzles.FindAll(p => p.type == Puzzle.Type.Cycles && !usedPuzzles.Contains(p.id))
                               .OrderBy(p => p.puzzleClass)
                               .First();
            }
            else
            {
                selection = puzzles.FindAll(p => p.type == Puzzle.Type.Functions && !usedPuzzles.Contains(p.id))
                               .OrderBy(p => p.puzzleClass)
                               .First();
            }
        }

        return selection;
    }

    /// <summary>
    /// Pide un puzzle para mostrar en el cliente. Nota: Llamese desde un command
    /// o de cualquier forma que corra en el servidor.
    /// </summary>
    [Server]
    public void RequestPuzzle(GameObject involved)
    {
        var puzzleInfo = involved.GetComponent<PuzzleInformation>();

        if (puzzleInfo == null)
        {
            //En caso que gameobject no tenga componente con la informacion de que puzzles ha hecho
            Debug.LogError($"GameObject {involved.name} doesn't contain progress component. Will not serve puzzle");
            return;
        }

        var puzzleToDo = GetPuzzle(
            PuzzleCache.Instance.puzzles,
            GetSolvedPuzzleIds(puzzleInfo),
            GetNotPassedPuzzleIds(puzzleInfo)
            );

        var newPuzzleId = new Guid();

        puzzlesInSession.Add(newPuzzleId, new PuzzlePlayerInfo
        {
            puzzleId = puzzleToDo.id,
            involved = involved
        });

        var networkConnection = involved.GetComponent<NetworkIdentity>();

        TargetShowPuzzle(networkConnection.connectionToClient, puzzleToDo, newPuzzleId);
    }

    [TargetRpc]
    void TargetShowPuzzle(NetworkConnection target, Puzzle puzzle, Guid puzzleAttemptId)
    {
        ShowEditor(puzzle.defaultWorkspace.text, puzzle.instructions, "", this.gameObject.name, "CommandCheckPuzzle");
        currentPuzzleId = puzzleAttemptId;
    }

    public class PuzzlePlayerInfo
    {
        public string puzzleId;
        public GameObject involved;
    }
}
