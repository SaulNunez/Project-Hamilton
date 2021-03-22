using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Model for setup of sequence tiles
/// </summary>
public class SequencePuzzleTileModel : MonoBehaviour
{
    [SerializeField]
    Image background;

    [SerializeField]
    Image foreground;

    /// <summary>
    /// Sprite to use for background (floor tiles)
    /// </summary>
    public Sprite BackgroundSprite
    {
        get => background.sprite;

        set => background.sprite = value;
    }

    /// <summary>
    /// Sprite to use for foreground (player, maybe obstacles)
    /// </summary>
    public Sprite ForegroundSprite
    {
        get => foreground.sprite;

        set
        {
            foreground.sprite = value;

            if(value == null)
            {
                foreground.enabled = false;
            } else
            {
                foreground.enabled = true;
            }
        }
    }
}
