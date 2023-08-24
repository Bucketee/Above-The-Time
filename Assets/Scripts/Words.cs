using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Words", menuName = "Words")]
public class Words : ScriptableObject
{
    public List<List<(Character leftCharacter, Character rightCharacter, string tellerName, bool leftIsTeller, string words)>> wordbundle = new()
    {
        new List<(Character leftCharacter, Character rightCharacter, string tellerName, bool leftIsTeller, string words)>()
        {
            (Character.Player, Character.Empty, "Player", true, "test1"),
            (Character.Player, Character.Empty, "???", false, "test2"),
        }
    };
}
