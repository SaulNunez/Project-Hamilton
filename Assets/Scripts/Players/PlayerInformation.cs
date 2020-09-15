using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Character", menuName = "Game Information/Character", order = 1)]
public class PlayerInformation : ScriptableObject
{
    public string id;
    public string characterName;
    [TextArea]
    public string description;
    public Sprite playerSprite;
    public int sanity;
    public int intelligence;
    public int physical;
    public int bravery;
}
