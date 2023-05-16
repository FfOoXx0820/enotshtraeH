using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minion : MonoBehaviour
{
    public Player Player;
    public string name;
    public int type;
    public int attack;
    public int health;
    public int tier;
    public Dictionary<string, bool> traits = new Dictionary<string, bool>() { { "Reborn", false }, { "Taunt", false }, { "Battlecry", false }, { "Windfury", false }, { "Deathrattle", false }, { "Divine Shield", false } };

    public Minion(string name, int type, int attack, int health, int tier, bool[] traits)
    {
        this.name = name;
        this.type = type;
        this.attack = attack;
        this.health = health;
        this.tier = tier;
        this.traits["Reborn"] = traits[0];
        this.traits["Taunt"] = traits[1];
        this.traits["Battlecry"] = traits[2];
        this.traits["Windfury"] = traits[3];
        this.traits["Deathrattle"] = traits[4];
        this.traits["Divine Shield"] = traits[5];
    }
    public void Attack(Minion enemy)
    {
        Hit(enemy.attack);
        enemy.Hit(attack);
    }

    public void Hit(int damage)
    {
        if (traits["Divine Shield"])
        {
            traits["Divine Shield"] = false;
        }
        else
        {
            health -= damage;
        }
        if (health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        if (traits["Deathrattle"])
        {
            
        }
        if (traits["Reborn"])
        {
            health = 1;
            traits["Reborn"] = false;
        } else
        {
            Player.number_of_minions -= 1;
            Player.battleground.Remove(this);
            gameObject.SetActive(false);
        }
    }

    public void Played()
    {
        if (traits["Battlecry"])
        {

        }
    }
}
