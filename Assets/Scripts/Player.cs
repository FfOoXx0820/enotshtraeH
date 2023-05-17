using System;
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
    public int i = 0;
    private System.Random rand = new System.Random();
    public List<Minion> battleground;

    public void Fight(Player opponent)
    {
        number_of_minions = battleground.Count;
        opponent.number_of_minions = opponent.battleground.Count;
        Minion_Attack(this, opponent);
    }

    public void Minion_Attack(Player attacker, Player opponent)
    {
        if (attacker.number_of_minions == 0 || opponent.number_of_minions == 0)
        {
            return;
        }
        Minion attacking_minion = attacker.battleground[attacker.i];
        List<Minion> taunting_minions = new List<Minion>();
        foreach (Minion minion in opponent.battleground)
        {
            if (minion.traits[4])
            {
                taunting_minions.Add(minion);
            }
        }
        if (taunting_minions.Count != 0)
        {
            attacking_minion.Attack(taunting_minions[rand.Next(0, taunting_minions.Count)]);
            if (attacking_minion.traits[5] && attacking_minion.alive)
            {
                if (taunting_minions.Count != 0)
                {
                    if (attacker.number_of_minions == 0 || opponent.number_of_minions == 0)
                    {
                        return;
                    }
                    attacking_minion.Attack(taunting_minions[rand.Next(0, taunting_minions.Count)]);
                } else
                {
                    if (attacker.number_of_minions == 0 || opponent.number_of_minions == 0)
                    {
                        return;
                    }
                    attacking_minion.Attack(opponent.battleground[rand.Next(0, opponent.number_of_minions)]);
                }
            }
        }
        else
        {
            attacking_minion.Attack(opponent.battleground[rand.Next(0, opponent.number_of_minions)]);
            if (attacking_minion.traits[5] && attacking_minion.alive)
            {
                if (attacker.number_of_minions == 0 || opponent.number_of_minions == 0)
                {
                    return;
                }
                attacking_minion.Attack(opponent.battleground[rand.Next(0, opponent.number_of_minions)]);
            }
        }
        if (attacker.i >= attacker.number_of_minions - 1)
        {
            attacker.i = 0;
        }
        else
        {
            attacker.i += 1;
        }
        this.Invoke(() => Minion_Attack(opponent, attacker), 1f);
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

public static class Utility
{
    public static void Invoke(this MonoBehaviour mb, Action f, float delay)
    {
        mb.StartCoroutine(InvokeRoutine(f, delay));
    }

    private static IEnumerator InvokeRoutine(System.Action f, float delay)
    {
        yield return new WaitForSeconds(delay);
        f();
    }
}
