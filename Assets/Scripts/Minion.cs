using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minion : MonoBehaviour
{
    public int type;
    public int attack;
    public int health;
    public int tier;
    public Dictionary<string, bool> traits = new Dictionary<string, bool>() { { "Reborn", false }, { "Taunt", false }, { "Battlecry", false }, { "Windfury", false }, { "Deathrattle", false }, { "Divine Shield", false } };

    public void Attack(Minion enemy)
    {
        Hit(enemy.attack);
        enemy.Hit(attack);
        if (traits["Windfury"])
        {
            traits["Windfury"] = false;
            Attack(enemy);
        }
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
            Destroy(this.gameObject);
        }
    }

    public void Played()
    {
        if (traits["Battlecry"])
        {

        }
    }
}
