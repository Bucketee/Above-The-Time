using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "CharacterSprites", menuName = "Sprites/CharacterSprites")]
public class CharacterSprites : ScriptableObject
{
    public Sprite playerSprite;
    public Sprite emptySprite;
    public Sprite senior;
    public Sprite boss;
}
