using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class PlayerVotingButton : MonoBehaviour
{
    public string Name {
        get => playerName.text; 
        set => playerName.text = value; 
    }
    public Sprite PlayerSprite { 
        get => playerSprite.sprite;
        set => playerSprite.sprite = value; 
    }
    
    public bool IsDead { get; set; }

    public UnityEvent onSelect;

    [SerializeField]
    Image playerSprite;

    [SerializeField]
    TMP_Text playerName;

    private void Start()
    {
        var button = GetComponent<Button>();

        if(button != null)
        {
            button.onClick.AddListener(CastVote);
        }
    }

    private void CastVote()
    {
        onSelect.Invoke();
    }
}
