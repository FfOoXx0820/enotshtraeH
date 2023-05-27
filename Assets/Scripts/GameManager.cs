using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public Shop Shop1;
    public Shop Shop2;
    public Player Player1;
    public Player Player2;
    public int turn_count;
    private float timeValue;
    private float seconds;
    public bool combatting;
    public TextMeshProUGUI timer_text;
    public TextMeshProUGUI VS_text;
    private List<Minion> temp1;
    private List<Minion> temp2;
    private System.Random rand = new System.Random();
    private void Start()
    {
        turn_count = 0;
        VS_text.gameObject.SetActive(false);
        Shop1.gameObject.SetActive(false);
        Shop2.gameObject.SetActive(false);
        timeValue = 30;
        Shop1.TurnStart();
    }
    private void Update() 
    { 
        if (!combatting)
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
    }
    public void TurnEnd()
    {
        Player1.BattleGroundSlot.SetActive(false);
        Player2.BattleGroundSlot.SetActive(false);
        turn_count += 1;
        if (turn_count == 1)
        {
            foreach (GameObject m in Shop1.shop_minions)
            {
                Destroy(m);
            }
            Shop1.gameObject.SetActive(false);
            Shop2.gameObject.SetActive(true);
            Shop2.TurnStart();
        } else if (turn_count == 2)
        {
            foreach (GameObject m in Shop2.shop_minions)
            {
                Destroy(m);
            }
            VS_text.gameObject.SetActive(true);
            Shop2.gameObject.SetActive(false);
            Fight(Player1, Player2);
            turn_count = 0;
        }
    }
    public void Fight(Player Player1, Player Player2)
    {
        Player1.BattleGroundSlot.SetActive(true);
        Player2.BattleGroundSlot.SetActive(true);
        combatting = true;
        temp1 = Player1.battleground;
        temp2 = Player2.battleground;
        Player1.number_of_minions = Player1.battleground.Count;
        Player2.number_of_minions = Player2.battleground.Count;
        Player1.BattleGroundSlot.transform.position = new Vector3(0.0f, -2.0f, 0.0f);
        Player2.BattleGroundSlot.transform.position = new Vector3(0.0f, 2.0f, 0.0f);
        int j = 0;
        foreach (Minion m in Player1.battleground)
        {
            m.gameObject.transform.position = new Vector3(m.gameObject.transform.position.x, -2.0f, 0.0f);
            j += 1;
        }
        j = 0;
        foreach (Minion m in Player2.battleground)
        {
            m.gameObject.transform.position = new Vector3(m.gameObject.transform.position.x, 2.0f, 0.0f);
            j += 1;
        }
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

    private void CombatEnd()
    {
        Player1.battleground = temp1;
        Player2.battleground = temp2;
        int j = 0;
        foreach (Minion m in Player1.battleground)
        {
            m.gameObject.transform.position = new Vector3(m.gameObject.transform.position.x, -4.0f, 0.0f);
            j += 1;
        }
        foreach (Minion m in Player2.battleground)
        {
            m.gameObject.transform.position = new Vector3(-11.0f, -2.0f, 0.0f);
        }
        combatting = false;
        Shop1.TurnStart();
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
        if (Check_Win(attacker, opponent))
        {
            CombatEnd();
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
                CombatEnd();
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
