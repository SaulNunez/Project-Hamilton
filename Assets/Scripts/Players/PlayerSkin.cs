using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class PlayerSkin : NetworkBehaviour
{
    [Tooltip("How to map character to setting up sprite and visuals specific to their character")]
    [Header("Character mapping")]
    public AvailableCharacters charactersMapping;

    [SyncVar(hook = nameof(SetPlayerSkin))]
    public CharacterTypes characterSelected;

    void SetPlayerSkin(CharacterTypes oldValue, CharacterTypes newValue)
    {
        var mapping = charactersMapping.characters.Find(x => x.characterType == newValue);

        var spriteComponent = GetComponent<SpriteRenderer>();
        spriteComponent.sprite = mapping.playerCharacter;
    }
}
