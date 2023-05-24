using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public Shop Shop;
    public Player Player1;
    public Player Player2;
    private float timeValue;
    private float seconds;
    public TextMeshProUGUI timer_text;
    private System.Random rand = new System.Random();
    private void Start()
    {
        timeValue = 0;
    }
    private void Update()
    {
        if (timeValue > 0)
        {
            timeValue -= Time.deltaTime;
            seconds = MathF.Floor(timeValue);
            timer_text.text = seconds.ToString();
        }
        else
        {
            timeValue = 30;
            seconds = MathF.Floor(timeValue);
            timer_text.text = seconds.ToString();
            TurnEnd();
        }
    }
    public void TurnEnd()
    {
        Shop.TurnStart();
    }
    public void Fight(Player Player1, Player Player2)
    {
        Player1.number_of_minions = Player1.battleground.Count;
        Player2.number_of_minions = Player2.battleground.Count;
        if (Player1.number_of_minions > Player2.number_of_minions)
        {
            Minion_Attack(Player1, Player2, false);
        }
        else if (Player1.number_of_minions < Player2.number_of_minions)
        {
            Minion_Attack(Player2, Player1, false);
        }
        else
        {
            if (rand.Next(0, 2) == 0)
            {
                Minion_Attack(Player1, Player2, false);
            }
            else
            {
                Minion_Attack(Player2, Player1, false);
            }
        }
    }

    public void Minion_Attack(Player attacker, Player opponent, bool Windfury)
    {
        if (!Windfury)
        {
            if (attacker.i >= attacker.number_of_minions - 1)
            {
                attacker.i = 0;
            }
            else
            {
                attacker.i += 1;
            }
        }
        Debug.Log("A:" + attacker.number_of_minions + "index: " + attacker.i);
        Debug.Log("O:" + opponent.number_of_minions);
        if (Check_Win(attacker, opponent))
        {
            return;
        }
        Minion attacking_minion = attacker.battleground[attacker.i];
        List<Minion> taunting_minions = new List<Minion>();
        foreach (Minion minion in opponent.battleground)
        {
            minion.GetComponent<SpriteRenderer>().color = Color.white;
            if (minion.traits[4])
            {
                taunting_minions.Add(minion);
            }
        }
        foreach (Minion minion in attacker.battleground)
        {
            minion.GetComponent<SpriteRenderer>().color = Color.white;
        }
        if (taunting_minions.Count != 0)
        {
            attacking_minion.Attack(taunting_minions[rand.Next(0, taunting_minions.Count)]);
        }
        else
        {
            attacking_minion.Attack(opponent.battleground[rand.Next(0, opponent.number_of_minions)]);
        }
        if (attacking_minion.traits[5] && attacking_minion.alive && !Windfury)
        {
            if (Check_Win(attacker, opponent))
            {
                return;
            }
            this.Invoke(() => Minion_Attack(attacker, opponent, true), 1f);
        } else
        {
            this.Invoke(() => Minion_Attack(opponent, attacker, false), 1f);
        }
    }

    public bool Check_Win(Player attacker, Player opponent)
    {
        if (attacker.number_of_minions == 0)
        {
            if (opponent.number_of_minions == 0)
            {
                return true;
            }
            else
            {
                opponent.Win(attacker);
                return true;
            }
        }
        else if (opponent.number_of_minions == 0)
        {
            attacker.Win(opponent);
            return true;
        }
        return false;
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
