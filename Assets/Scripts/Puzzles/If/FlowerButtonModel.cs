using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FlowerButtonModel : MonoBehaviour
{
    public Button button;

    [SerializeField]
    Image sprite;

    public Sprite ButtonSprite { 
        get => sprite.sprite; 
        set => sprite.sprite = value; 
    }
}
