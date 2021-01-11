using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PuzzleInformation : NetworkBehaviour
{
    public SyncList<string> usedPuzzles = new SyncList<string>();

    public SyncList<string> failedPuzzles = new SyncList<string>();
}
