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

    Lazy<AvailableCharactersMemory> memory = new Lazy<AvailableCharactersMemory>(() =>
    {
        var gameObj = GameObject.FindGameObjectWithTag(Tags.AvailablePlayerManager);
        return gameObj.GetComponent<AvailableCharactersMemory>();
    });

    public override void OnStartClient()
    {
        base.OnStartClient();

        AvailableCharactersMemory.OnCharacterAvailable += EnableCharacter;
        AvailableCharactersMemory.OnCharacterOccupied += DisableCharacter;

        // Por default se elige un personaje aleatorio
        var characterTypes = Enum.GetValues(typeof(CharacterTypes));
        while (true)
        {
            var characterType = (CharacterTypes)characterTypes.GetValue(UnityEngine.Random.Range(0, characterTypes.Length));
            if (!memory.Value.CharacterUsed(characterType))
            {
                CmdSetCharacterOnServer(characterType);
                memory.Value.CmdSetPlayerSelection((NetworkManager.singleton as HamiltonNetworkRoomManager).UniquenessId, characterType);

                toggles.Find(x => x.characterType == characterType).toggle.isOn = true;

                break;
            }
        }

        foreach (var toggle in toggles)
        {
            toggle.toggle.onValueChanged.AddListener((isOn) =>
            {
                if (isOn)
                {
                    if (!memory.Value.CharacterUsed(toggle.characterType))
                    {
                        CmdSetCharacterOnServer(toggle.characterType);
                        memory.Value.CmdSetPlayerSelection((NetworkManager.singleton as HamiltonNetworkRoomManager).UniquenessId, toggle.characterType);

                    }
                }
            });
        }
    }

    [Command]
    public void CmdSetCharacterOnServer(CharacterTypes characterType)
    {
        player.SetCharacter(characterType);
    }

    public override void OnStopClient()
    {
        base.OnStopClient();

        AvailableCharactersMemory.OnCharacterAvailable -= EnableCharacter;
        AvailableCharactersMemory.OnCharacterOccupied -= DisableCharacter;
    }

    private void DisableCharacter(CharacterTypes typeOccupied)
    {
        if (typeOccupied != player.characterType)
        {
            toggles.Find(x => x.characterType == typeOccupied).toggle.interactable = false;
            print("disabled char");
        }
    }

    private void EnableCharacter(CharacterTypes typeAvailable)
    {
        toggles.Find(x => x.characterType == typeAvailable).toggle.interactable = true;
        print("enabled char");
    }
}
