using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Playground for sequence puzzle
/// </summary>
[RequireComponent(typeof(GridLayoutGroup))]
public class SequenceGrid : MonoBehaviour
{
    //---EVENTS
    public static event Action<Vector2Int> OnPositionUpdate;

    //---STATE
    Vector2Int currentPosition;

    //--INSTANCED PREFABS
    GameObject[] tilesSpawned;

    //---PREFABS
    [SerializeField]
    GameObject tilePrefab;

    //---SPRITES CUSTOMIZABLES
    [Header("Sprites")]
    [Tooltip("Sprite to use in boxes that if you walk into, you'd fall")]
    [SerializeField]
    Sprite voidBackground;

    /// <summary>
    /// Sprite to use in boxes that if you walk into, you'd fall
    /// </summary>
    public Sprite VoidBackground
    {
        get => voidBackground;
        set
        {
            voidBackground = value;

            UpdateBkgSprite();
        }
    }

    [Tooltip("Sprite to use in boxes player can walk in")]
    [SerializeField]
    public Sprite floorBackground;

    /// <summary>
    /// Sprite to use in boxes player can walk in
    /// </summary>
    public Sprite FloorBackground
    {
        get => floorBackground;
        set
        {
            floorBackground = value;

            UpdateBkgSprite();
        }
    }

    [Tooltip("Player sprite")]
    [SerializeField]
    Sprite player;

    /// <summary>
    /// Player sprite
    /// </summary>
    public Sprite Player
    {
        get => player;

        set
        {
            player = value;

            UpdateOverlaySprite();
        }
    } 

    //---CONTROLLED LAYOUT GROUP

    GridLayoutGroup grid;

    //---Sequence
    [Tooltip("Sequence to show in screen")]
    [SerializeField]
    SequenceConfig sequence;

    /// <summary>
    /// Sequence to show in screen. Updates screen as soon as it changes
    /// </summary>
    public SequenceConfig Sequence
    {
        get => sequence;

        set
        {
            sequence = value;

            UpdateBkgSprite();
            UpdateOverlaySprite();
        }
    }

    [SerializeField]
    SequencePuzzle sequencePuzzle;

    private void Start()
    {
        currentPosition = sequence.startPosition;

        tilesSpawned = new GameObject[sequence.horizontalSize * sequence.verticalSize];

        for (var i = 0; i < tilesSpawned.Length; i++)
        {
            tilesSpawned[i] = Instantiate(tilePrefab, transform);
            tilesSpawned[i].name = $"Tile_{i}";
        }

        grid = GetComponent<GridLayoutGroup>();
        grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        grid.constraintCount = sequence.horizontalSize;

        UpdateBkgSprite();
        UpdateOverlaySprite();
    }

    private void Update()
    {
        var updated = false;
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            print("Go up");
            updated = true;
            // No idea why down goes up
            currentPosition += Vector2Int.down;
        } 
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            print("Go down");
            updated = true;
            currentPosition += Vector2Int.up;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            print("Go left");
            updated = true;
            currentPosition += Vector2Int.left;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            print("Go right");
            updated = true;
            currentPosition += Vector2Int.right;
        }

        if (!updated)
        {
            return;
        }

        currentPosition.Clamp(Vector2Int.zero, new Vector2Int(sequence.horizontalSize, sequence.verticalSize));

        if (!sequence.floor[(currentPosition.y * sequence.horizontalSize) + currentPosition.x])
        {
            currentPosition = sequence.startPosition;
        }

        if(currentPosition == sequence.endPosition)
        {
            sequencePuzzle.SetPuzzleComplete();
        }

        OnPositionUpdate?.Invoke(currentPosition);

        UpdateOverlaySprite();
    }

    private void UpdateBkgSprite()
    {
        for (int i = 0; i < tilesSpawned.Length; i++)
        {
            tilesSpawned[i].GetComponent<SequencePuzzleTileModel>().BackgroundSprite = sequence.floor[i] ? floorBackground : voidBackground;
        }
    }

    private void UpdateOverlaySprite()
    {
        var currentPlayerPos = (currentPosition.y * sequence.horizontalSize) + currentPosition.x;
        for (int i = 0; i < tilesSpawned.Length; i++)
        {
            tilesSpawned[i].GetComponent<SequencePuzzleTileModel>().ForegroundSprite = currentPlayerPos == i ? player : null;
        }
    }
}
