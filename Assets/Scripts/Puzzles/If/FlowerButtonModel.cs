using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// In *if* flower puzzle, serves as a weak abstraction to separate concerns.
/// Can also prevent change in button structure from breaking code that handles button setup.
/// 
/// Button component is served directly, but the sprite is a property that can be modified to acomodate new ways of displaying.
/// </summary>
public class FlowerButtonModel : MonoBehaviour
{
    /// <summary>
    /// The UI button that represents that flower type
    /// </summary>
    public Button button;

    [Tooltip("The image inside the button, representing the flower type")]
    [SerializeField]
    Image sprite;

    /// <summary>
    /// The sprite to use for the flower
    /// </summary>
    public Sprite ButtonSprite { 
        get => sprite.sprite; 
        set => sprite.sprite = value; 
    }
}
