using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Mirror;
using Extensions;

/// <summary>
/// Shows in UI location of not yet solved puzzles and ocurring emergencies. Listens to puzzle completion and emergencies started to update list.  
/// </summary>
public class PuzzlesListUi : NetworkBehaviour
{
    [Tooltip("Text used for representing a list of available tasks to do")]
    [SerializeField]
    TMP_Text taskText;

    public override void OnStartServer()
    {
        base.OnStartServer();

        PuzzleCompletion.OnPuzzleCompleted += OnPuzzleCompletionChanged;
        Emergency.OnEmergencyStarted += OnEmergencyStarted;
        Emergency.OnEmergencyResolved += OnEmergencyResolved;
        Emergency.OnTimeRemaingForEmergencyChanged += OnEmergencyTimeLeftChanged;
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        taskText.text = $"{CreateTasksList()}";
    }

    /**
     * PORQUE EL DELAY?
     * 
     * Para hacer que synvar espere lo suficiente para actualizar las variables del cliente
     * Porque eso hace que no se actualice a la lista de tareas al acabarse la emergencia
     */
    private void OnEmergencyResolved()
    {
        this.SuperPrint("Waiting to update list of puzzles");
        Invoke(nameof(OnEmergencyResolvedDelayed), 1f);
    }

    private void OnEmergencyResolvedDelayed()
    {
        RpcRecreateText();
    }

    private void OnEmergencyStarted(Emergency.EmergencyType _) => RpcRecreateText();

    private void OnEmergencyTimeLeftChanged(int _) => RpcRecreateText();

    public override void OnStopServer()
    {
        base.OnStopServer();

        PuzzleCompletion.OnPuzzleCompleted -= OnPuzzleCompletionChanged;
        Emergency.OnEmergencyStarted -= OnEmergencyStarted;
        Emergency.OnEmergencyResolved -= OnEmergencyResolved;
        Emergency.OnTimeRemaingForEmergencyChanged -= OnEmergencyTimeLeftChanged;
    }

    private void OnPuzzleCompletionChanged(PuzzleId id, NetworkIdentity doneBy) => RpcRecreateText();

    [ClientRpc]
    void RpcRecreateText()
    {
        if (Emergency.instance == null
    ||  Emergency.instance.CurrentActiveSabotage == Emergency.EmergencyType.None)
        {
            taskText.text = $"{CreateTasksList()}";
        } 
        else
        {
            taskText.text = $"{CreateEmergencyCoundown()}\n{CreateEmergencyInformation()}";
        }
    }

    private string CreateEmergencyCoundown()
    {
        if (Emergency.instance == null 
            || Emergency.instance.CurrentActiveSabotage == Emergency.EmergencyType.None)
        {
            return string.Empty;
        }

        return $"Emergencia, {Emergency.instance.TimeRemainingEmergency} segundos restantes";
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [Client]
    string CreateTasksList()
    {
        var taskList = "";
        foreach(var task in AvailablePuzzles())
        {
            switch (task) {
                case PuzzleId.BoilersVariableInteger:
                    taskList += "Boilers: Registra temperatura del termometro \n";
                    break;
                case PuzzleId.Sequence1:
                    taskList += "Garage: Pasa un laberinto de cajas \n";
                    break;
                case PuzzleId.VariableString:
                    taskList += "Cocina: Hablar con nanabot \n";
                    break;
                case PuzzleId.VariableBoolean:
                    taskList += "Electricidad: Encender generador \n";
                    break;
                case PuzzleId.DoWhileMotorStarter:
                    taskList += "Garage: Enciende el automovil \n";
                    break;
                case PuzzleId.VariableFloat:
                    taskList += "Boilers: Registra temperatura del termometro(2) \n";
                    break;
                case PuzzleId.ForWashingBucket:
                    taskList += "Lavandería: Lava la ropa \n";
                    break;
                case PuzzleId.WhileFillingBucket:
                    taskList += "Saguán: Carga agua a la cubeta \n";
                    break;
                case PuzzleId.IfFlowerPicking:
                    taskList += "Saguán: Recoge el tipo de flor correcto \n";
                    break;
                case PuzzleId.Substring:
                    taskList += "Telegrafo: Obten la subcadena de una cadena \n";
                    break;
                case PuzzleId.IfElse:
                    taskList += "Cocina: Decora el cupcake o el pastel \n";
                    break;
            }
        }

        return taskList;
    }

    string CreateEmergencyInformation()
    {
        var emergencyList = "";
        if(Emergency.instance != null)
        {
            switch (Emergency.instance.CurrentActiveSabotage)
            {
                case Emergency.EmergencyType.ChangeBoilerPressure:
                    emergencyList += "<color=#ff0000ff>Emergencia\nBoilers: Controlar la presión</color>";
                    break;
                case Emergency.EmergencyType.TurnDownGenerator:
                    emergencyList += "<color=#ff0000ff>Emergencia\nElectricidad: Enciende el generador de emergencia</color>";
                    break;
                case Emergency.EmergencyType.QuestionSabotage:
                    emergencyList += "<color=#ff0000ff>Emergencia\nLibreria: Consigue a un compañero para contestar unas preguntas</color>";
                    break;
            }
        }

        return emergencyList;
    }

    [Client]
    List<PuzzleId> AvailablePuzzles() => 
        new List<PuzzleId>((IEnumerable<PuzzleId>)Enum.GetValues(typeof(PuzzleId)))
            .Where(pId =>
            {
                //Cuando el juego apenas esta iniciando, puede que se inicie ligeramente mas tarde, simplemente retornar todos
                if(PuzzleCompletion.instance == null)
                {
                    return true;
                }
                return !PuzzleCompletion.instance.puzzlesCompleted.Where(x => x.netIdentity == NetworkClient.connection.identity).Any(x => x.Id == pId);
            })
            .ToList();
}
