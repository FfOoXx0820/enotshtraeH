using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Minion : MonoBehaviour
{
    public Player Player;
    public string _name;
    public int type;
    public int attack;
    public int health;
    public int tier;
    public bool alive;
    public TextMeshPro attack_text;
    public TextMeshPro health_text;
    //BattleCry Deathrattle DivineShield Reborn Taunt Windfury
    public bool[] traits = { false, false, false, false, false, false };
    //public Dictionary<string, bool> traits = new Dictionary<string, bool>() { { "Reborn", false }, { "Taunt", false }, { "Battlecry", false }, { "Windfury", false }, { "Deathrattle", false }, { "Divine Shield", false } };
    private void Start()
    {
        alive = true;
        Attack_Update(attack);
        Health_Update(health);
    }
    public void Attack(Minion enemy)
    {
        this.GetComponent<SpriteRenderer>().color = Color.red;
        enemy.GetComponent<SpriteRenderer>().color = Color.green;
        Hit(enemy.attack);
        enemy.Hit(attack);
    }

    public void Hit(int damage)
    {
        if (traits[2])
        {
            traits[2] = false;
        }
        else
        {
            Health_Update(health - damage);
        }
        if (health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        if (traits[1])
        {
            
        }
        if (traits[3])
        {
            Health_Update(1);
            traits[3] = false;
            //Player.i -= 1;
        } else
        {
            Player.number_of_minions -= 1;
            Player.battleground.Remove(this);
            alive = false;
            gameObject.SetActive(false);
        }
    }
    public void Attack_Update(int new_attack)
    {
        attack = new_attack;
        attack_text.text = attack.ToString();
    }
    public void Health_Update(int new_health)
    {
        health = new_health;
        health_text.text = health.ToString();
    }
}
