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

    private Vector2Int currentPosition;
    public Vector2Int CurrentPosition 
    {
        get => currentPosition;
        set {
            currentPosition = value;

            UpdateBkgSprite();
            UpdateOverlaySprite();
        }
    }

    private void Start()
    {
        tilesSpawned = new GameObject[sequence.horizontalSize * sequence.verticalSize];

        for (var i = 0; i < tilesSpawned.Length; i++)
        {
            tilesSpawned[i] = Instantiate(tilePrefab, transform);
            tilesSpawned[i].name = $"Tile_{i}";
        }

        grid = GetComponent<GridLayoutGroup>();
        grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        grid.constraintCount = sequence.horizontalSize;

        ResetSequence();
    }

    public void ResetSequence()
    {
        CurrentPosition = sequence.startPosition;

        UpdateBkgSprite();
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
        var currentPlayerPos = (CurrentPosition.y * sequence.horizontalSize) + CurrentPosition.x;
        for (int i = 0; i < tilesSpawned.Length; i++)
        {
            tilesSpawned[i].GetComponent<SequencePuzzleTileModel>().ForegroundSprite = currentPlayerPos == i ? player : null;
        }
    }
}
