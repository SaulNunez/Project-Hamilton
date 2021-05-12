using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles setting a character type for current player, to be connected with lobby UI, before game starts.
/// 
/// Can be used anywhere in the hierarchy.
/// </summary>
public class PlayerSelectionInLobby : MonoBehaviour
{
    /// <summary>
    /// Tiny container class that matches toggle with a character type
    /// </summary>
    [Serializable]
    public class ToggleAndCharacter
    {
        public CharacterTypes characterType;
        public Toggle toggle;
    }

    [SerializeField]
    HamiltonNetworkPlayer player;

    [Tooltip("The relationship between toggles and the character they select")]
    [SerializeField]
    List<ToggleAndCharacter> toggles;

    public CharacterTypes currentCharacterType = CharacterTypes.AndreaLewis;

    void Start()
    {
        foreach(var toggle in toggles)
        {
            toggle.toggle.onValueChanged.AddListener((isOn) =>
            {
                if (isOn)
                {
                    currentCharacterType = toggle.characterType;
                }
            });
        }
    }

    /// <summary>
    /// Called when "saving and close", updates variables in network player with updated values
    /// </summary>
    public void Commit()
    {
        player.characterType = currentCharacterType;
    }
}
