using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    public int turns;

    public enum CurrentState
    {
        OTHER_PLAYER,
        CURRENT_TURN
    }

    public enum OnTurn
    {
        WAIT_FOR_INPUT,
        MOVEMENT,
        DOING_SOMETHING,
        ON_THROW
    }
}
