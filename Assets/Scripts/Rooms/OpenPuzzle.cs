using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using System;

public class OpenPuzzle : InteractuableBehavior
{
    HubConfig hubConfig;

    [SerializeField]
    LayerMask playerLayer;

    public override void OnStartServer()
    {
        var lobbyConfigs = GameObject.FindGameObjectWithTag(Tags.HubConfig);
        hubConfig = lobbyConfigs.GetComponent<HubConfig>();
    }

    [Server]
    public override void OnApproach(GameObject approachedBy)
    {   
        print("aaa");
        //Checar en el servidor que el jugador este cerca
        if(Vector2.Distance(approachedBy.transform.position,transform.position) <= hubConfig.actDistance){
            //var puzzlesOfPlayer = approachedBy.GetComponent<PuzzleInformation>()
            print("aaaaaaa");
        }
    }

    public List<string> GetSolvedPuzzleIds(PuzzleInformation puzzlesOfPlayer) =>
        //Como no podemos retornar la lista, buscar todos los que no sean null
        puzzlesOfPlayer.usedPuzzles.FindAll(s => s != null);

    public List<string> GetNotPassedPuzzleIds(PuzzleInformation puzzlesOfPlayer) =>
        //Como no podemos retornar la lista, buscar todos los que no sean null
        puzzlesOfPlayer.failedPuzzles.FindAll(s => s != null);

    [Server]
    public void GetPuzzle(List<string> passed, List<string> notPassed){
        
    }

    [ClientRpc]
    public void RpcOpenPuzzle() {

    }
}
