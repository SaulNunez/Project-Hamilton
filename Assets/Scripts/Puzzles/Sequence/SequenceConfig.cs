using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Sequence", menuName = "ScriptableObjects/Sequence", order = 1)]
public class SequenceConfig : ScriptableObject
{   
    /// <summary>
    /// Position where the player starts to move to
    /// </summary>
    public Vector2Int startPosition;
    /// <summary>
    /// Position the player must get to to complete
    /// </summary>
    public Vector2Int endPosition;
    //---SEQUENCE CONFIGURATION
    /// <summary>
    /// Horizontal size of game area
    /// </summary>
    [Header("Sequence configuration")]
    public int horizontalSize;
    /// <summary>
    /// Vertical size of game area
    /// </summary>
    public int verticalSize;

    /// <summary>
    /// The walkable blocks in this sequence puzzle. Represents two dimensions, the array elements with true mean that it can be walked upon.
    /// Can map to needed index with (currentPosition.y * horizontalSize) + currentPosition.x]
    /// </summary>
    /// <remarks>
    /// If changed on runtime, it will run into unforseen consequences. Only to set in editor.
    /// </remarks>
    [Header("Floor")]
    public bool[] floor;
}
