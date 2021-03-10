using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Logic for playground for sequence puzzle
/// </summary>
[RequireComponent(typeof(GridLayoutGroup))]
public class SequenceGrid : MonoBehaviour
{
    //---EVENTS
    public static event Action<Vector2Int> OnPositionUpdate;

    [SerializeField]
    Vector2Int startPosition;

    //---STATE
    Vector2Int currentPosition;

    //--INSTANCED PREFABS
    GameObject[] tilesSpawned;

    //---PREFABS
    [SerializeField]
    GameObject tilePrefab;

    //---SPRITES CUSTOMIZABLES
    /// <summary>
    /// Sprite to use in boxes that if you walk into, you'd fall
    /// </summary>
    [Header("Sprites")]
    public Sprite voidBackground;

    /// <summary>
    /// Sprite to use in walkable boxes
    /// </summary>
    public Sprite floorBackground;

    //---CONTROLLED LAYOUT GROUP

    GridLayoutGroup grid;

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

    private void Start()
    {
        currentPosition = startPosition;

        tilesSpawned = new GameObject[horizontalSize * verticalSize];

        for(var i = 0; i < tilesSpawned.Length; i++)
        {
            tilesSpawned[i] = Instantiate(tilePrefab, transform);
        }

        grid = GetComponent<GridLayoutGroup>();
        grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        grid.constraintCount = horizontalSize;

        UpdateBkgSprite();
        UpdateOverlaySprite();
    }

    private void Update()
    {
        var updated = false;
        if(Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            updated = true;
            currentPosition -= Vector2Int.up;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            updated = true;
            currentPosition -= Vector2Int.down;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            updated = true;
            currentPosition -= Vector2Int.left;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            updated = true;
            currentPosition -= Vector2Int.right;
        }

        if (!updated)
        {
            return;
        }

        currentPosition.Clamp(Vector2Int.zero, new Vector2Int(horizontalSize, verticalSize));

        if (!floor[(currentPosition.y * horizontalSize) + currentPosition.x])
        {
            currentPosition = startPosition;
        }

        OnPositionUpdate?.Invoke(currentPosition);

        UpdateOverlaySprite();
    }

    private void UpdateBkgSprite()
    {
        for(var i = 0; i < tilesSpawned.Length; i++)
        {
            tilesSpawned[i].GetComponent<Image>().sprite = floor[i] ? floorBackground : voidBackground;
        }
    }

    private void UpdateOverlaySprite()
    {
        var currentPlayerPos = (currentPosition.y * currentPosition.x) + currentPosition.x;
        for (var i = 0; i < tilesSpawned.Length; i++)
        {
            //TODO: Set player sprite here
            tilesSpawned[i].GetComponent<Image>().sprite = currentPlayerPos == i ? floorBackground : null;
        }
    }
}
