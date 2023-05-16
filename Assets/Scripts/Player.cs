using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int health;
    public int tier;
    public int gold;
    public int hand;
    public int number_of_minions;
    public List<Minion> battleground;

    public void Fight(Player opponent)
    {
        number_of_minions = battleground.Count;
        opponent.number_of_minions = opponent.battleground.Count;
        if (opponent.number_of_minions > number_of_minions)
        {
            opponent.battleground[0].Attack(battleground[Random.Range(0, number_of_minions)]);
        }
        else if (opponent.number_of_minions == number_of_minions)
        {
            if (Random.Range(0, 1) == 0)
            {
                opponent.battleground[0].Attack(battleground[Random.Range(0, number_of_minions)]);
            }
        }
        List<Minion> taunting_minions = new List<Minion>();
        List<Minion> opponent_taunting_minions = new List<Minion>();
        foreach (Minion minion in battleground)
        {
            if (minion.traits["Taunt"])
            {
                taunting_minions.Add(minion);
            }
        }
        foreach (Minion minion in opponent.battleground)
        {
            if (minion.traits["Taunt"])
            {
                opponent_taunting_minions.Add(minion);
            }
        }
        int i = 0;
        int j = 0;
        while (opponent.number_of_minions != 0 && number_of_minions != 0)
        {
            if (i >= number_of_minions - 1)
            {
                i = 0;
            }
            else
            {
                i += 1;
            }
            Debug.Log("i: " + i);
            Minion_Attack(this, opponent, opponent_taunting_minions, i);
            if (opponent.number_of_minions == 0 || number_of_minions == 0)
            {
                break;
            }
            if (j >= opponent.number_of_minions - 1)
            {
                j = 0;
            }
            else
            {
                j += 1;
            }
            Debug.Log("j: " + j);
            Minion_Attack(opponent, this, taunting_minions, j);
        }
        Debug.Log("Done");
        Debug.Log(number_of_minions);
        Debug.Log(opponent.number_of_minions);
    }

    public void Minion_Attack(Player attacker, Player opponent, List<Minion> taunting_minions, int i)
    {
        int target = Random.Range(0, opponent.number_of_minions);
        if (taunting_minions.Count != 0)
        {
            attacker.battleground[i].Attack(taunting_minions[Random.Range(0, taunting_minions.Count)]);
            if (battleground[i].traits["Windfury"])
            {
                Minion_Attack(attacker, opponent, taunting_minions, i);
            }
        }
        else
        {
            Debug.Log("Target:" + target);
            attacker.battleground[i].Attack(opponent.battleground[target]);
        }
    }

    public void Win(Player opponent)
    {
        int damage = tier;
        foreach (Minion minion in battleground)
        {
            damage += minion.tier;
        }
        opponent.health -= damage;
        if (opponent.health <= 0)
        {
            opponent.Die();
        }
    }

    public void Die()
    {

    }
}
