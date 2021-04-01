using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Mirror;

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

        RpcRecreateText();
    }

    private void OnEmergencyResolved() => RpcRecreateText();

    private void OnEmergencyStarted(Emergency.EmergencyType _) => RpcRecreateText();

    public override void OnStopServer()
    {
        base.OnStopServer();

        PuzzleCompletion.OnPuzzleCompleted -= OnPuzzleCompletionChanged;
    }

    private void OnPuzzleCompletionChanged(PuzzleId _) => RpcRecreateText();

    [ClientRpc]
    void RpcRecreateText()
    {
        taskText.text = $"{CreateEmergencyList()}\n{CreateTasksList()}";
    }

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
                case PuzzleId.TheoryVariety:
                    taskList += "aaaaa \n";
                    break;
            }
        }

        return taskList;
    }

    string CreateEmergencyList()
    {
        var emergencyList = "";

        switch (Emergency.instance.CurrentActiveSabotage)
        {
            case Emergency.EmergencyType.ChangeBoilerPressure:
                emergencyList += "<color=#ff0000ff>Emergencia\nBoilers: Controlar la presión</color>";
                break;
            case Emergency.EmergencyType.TurnDownGenerator:
                emergencyList += "<color=#ff0000ff>Emergencia\nElectricidad: Enciende el generador de emergencia</color>";
                break;
        }

        return emergencyList;
    }

    List<PuzzleId> AvailablePuzzles() => 
        new List<PuzzleId>((IEnumerable<PuzzleId>)Enum.GetValues(typeof(PuzzleId)))
            .Where(pId => !PuzzleCompletion.instance.puzzlesCompleted.Contains(pId))
            .ToList();
}
