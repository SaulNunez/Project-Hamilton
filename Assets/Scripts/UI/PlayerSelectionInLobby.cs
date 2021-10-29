using Mirror;
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
public class PlayerSelectionInLobby : NetworkBehaviour
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

    public override void OnStartClient()
    {
        base.OnStartClient();

        // Por default se elige un personaje aleatorio
        var characterTypes = Enum.GetValues(typeof(CharacterTypes));

        var characterType = (CharacterTypes)characterTypes.GetValue(UnityEngine.Random.Range(0, characterTypes.Length));
        CmdSetCharacterOnServer(characterType);
        toggles.Find(x => x.characterType == characterType).toggle.isOn = true;

        foreach (var toggle in toggles)
        {
            toggle.toggle.onValueChanged.AddListener((isOn) =>
            {
                if (isOn)
                {
                    CmdSetCharacterOnServer(toggle.characterType);
                }
            });
        }
    }

    [Command]
    public void CmdSetCharacterOnServer(CharacterTypes characterType)
    {
        player.SetCharacter(characterType);
    }
}
