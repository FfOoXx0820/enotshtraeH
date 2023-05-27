using UnityEngine;
using TMPro;

[CreateAssetMenu(fileName = "New Minion", menuName = "Minion")]
public class Minion : ScriptableObject
{
    public new string name;
    public string description;

    public int type;
    public int attack;
    public int health;
    public int manaCost;
    public int tier;
    //BattleCry Deathrattle DivineShield Reborn Taunt Windfury
    public bool[] traits = { false, false, false, false, false, false };
    //public Dictionary<string, bool> traits = new Dictionary<string, bool>() { { "Reborn", false }, { "Taunt", false }, { "Battlecry", false }, { "Windfury", false }, { "Deathrattle", false }, { "Divine Shield", false } };

	public Sprite artwork;
}
