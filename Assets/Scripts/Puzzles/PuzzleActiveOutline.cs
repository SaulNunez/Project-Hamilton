using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class PuzzleActiveOutline : MonoBehaviour
{
    [SerializeField]
    bool isActive = true;

    public bool IsActive { get => isActive; set
        {
            isActive = value;

            UpdateMaterial();
        }
    }

    [Header("Materials")]
    [SerializeField]
    Material activeMaterial;

    [SerializeField]
    Material inactiveMaterial;

    void UpdateMaterial()
    {
        var renderer = GetComponent<Renderer>();
        if (isActive)
        {
            renderer.material = activeMaterial;
        } else
        {
            renderer.material = inactiveMaterial;
        }
    }

    private void Start()
    {
        UpdateMaterial();
    }
}
