using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName ="Character mapping", menuName ="Characters/Character to sprite mapping")]
public class AvailableCharacters : ScriptableObject
{
    [System.Serializable]
    public class CharacterInfo
    {
        public CharacterTypes characterType;
        public Sprite playerCharacter;
    }

    public List<CharacterInfo> characters;
}
