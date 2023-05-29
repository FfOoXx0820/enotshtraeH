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
    public int player_turn_count;
    private float timeValue;
    private float seconds;
    public bool combatting;
    public TextMeshProUGUI timer_text;
    public TextMeshProUGUI VS_text;
    public GameObject TurnEndButton;

    private System.Random rand = new System.Random();
    private void Start()
    {
        Player1.health_text.gameObject.transform.position = new Vector3(-7.5f, -1.1f, 0.0f);
        Player2.health_text.gameObject.transform.position = new Vector3(-7.5f, -1.1f, 0.0f);
        player_turn_count = 0;
        VS_text.gameObject.SetActive(false);
        Shop1.gameObject.SetActive(false);
        Shop2.gameObject.SetActive(false);
        Player1.gameObject.SetActive(true);
        Player2.gameObject.SetActive(false);
        timeValue = 200;
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
                TurnEnd();
            }
        }
    }
    public void TurnEnd()
    {
        player_turn_count += 1;
        if (player_turn_count == 1)
        {
            Player1.gameObject.SetActive(false);
            Player2.gameObject.SetActive(true);
            timeValue = 200;
            seconds = MathF.Floor(timeValue);
            timer_text.text = seconds.ToString();
            foreach (GameObject m in Shop1.shop_minions)
            {
                Destroy(m.gameObject);
            }
            Shop1.gameObject.SetActive(false);
            Shop2.gameObject.SetActive(true);
            Shop2.TurnStart();
        } else if (player_turn_count == 2)
        {
            timer_text.gameObject.SetActive(false);
            foreach (GameObject m in Shop2.shop_minions)
            {
                Destroy(m.gameObject);
            }
            Shop2.gameObject.SetActive(false);
            player_turn_count = 0;
            TurnEndButton.SetActive(false);
            Fight(Player1, Player2);
        }
    }
    public void Fight(Player Player1, Player Player2)
    {
        Player1.gameObject.SetActive(true);
        Player2.gameObject.SetActive(true);
        Player1.health_text.gameObject.transform.position = new Vector3(0.0f, -4.5f, 0.0f);
        Player2.health_text.gameObject.transform.position = new Vector3(0.0f, 4.5f, 0.0f);
        combatting = true;
        VS_text.gameObject.SetActive(true);
        Player1.number_of_minions = Player1.battleground.Count;
        Player2.number_of_minions = Player2.battleground.Count;
        foreach (Minion_ m in Player1.battleground)
        {
            Minion_ clone = Instantiate(m, new Vector3(m.gameObject.transform.position.x, -2.0f, 0.0f), Quaternion.identity, Player1.transform);
            Player1.temp.Add(clone);
        }
        foreach (Minion_ m in Player2.battleground)
        {
            Minion_ clone = Instantiate(m, new Vector3(m.gameObject.transform.position.x, 2.0f, 0.0f), Quaternion.identity, Player2.transform);
            Player2.temp.Add(clone);
        }
        if (Player1.number_of_minions > Player2.number_of_minions)
        {
            this.Invoke(() => Minion_Attack(Player1, Player2, false), 1f);
        }
        else if (Player1.number_of_minions < Player2.number_of_minions)
        {
            this.Invoke(() => Minion_Attack(Player2, Player1, false), 1f);
        }
        else
        {
            if (rand.Next(0, 2) == 0)
            {
                this.Invoke(() => Minion_Attack(Player1, Player2, false), 1f);
            }
            else
            {
                this.Invoke(() => Minion_Attack(Player2, Player1, false), 1f);
            }
        }
    }

    private void CombatEnd()
    {
        Player1.health_text.gameObject.transform.position = new Vector3(-7.5f, -1.1f, 0.0f);
        Player2.health_text.gameObject.transform.position = new Vector3(-7.5f, -1.1f, 0.0f);
        timer_text.gameObject.SetActive(true);
        timeValue = 200;
        seconds = MathF.Floor(timeValue);
        timer_text.text = seconds.ToString();
        foreach (Minion_ m in Player1.temp)
        {
            Destroy(m.gameObject);
        }
        Player1.temp = new List<Minion_>();
        foreach (Minion_ m in Player2.temp)
        {
            Destroy(m.gameObject);
        }
        Player2.temp = new List<Minion_>();
        combatting = false;
        VS_text.gameObject.SetActive(false);
        Player1.gameObject.SetActive(true);
        Player2.gameObject.SetActive(false);
        Shop1.TurnStart();
        TurnEndButton.SetActive(true);
    }

    public void Minion_Attack(Player attacker, Player opponent, bool Windfuried)
    {
        if (!Windfuried)
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
            Invoke("CombatEnd", 1f);
            return;
        }
        Minion_ attacking_minion = attacker.temp[attacker.i];
        List<Minion_> taunting_minions = new List<Minion_>();
        foreach (Minion_ minion in opponent.temp)
        {
            minion.GetComponent<SpriteRenderer>().color = Color.white;
            if (minion.traits[4])
            {
                taunting_minions.Add(minion);
            }
        }
        foreach (Minion_ minion in attacker.temp)
        {
            minion.GetComponent<SpriteRenderer>().color = Color.white;
        }
        if (taunting_minions.Count != 0)
        {
            attacking_minion.Attack(taunting_minions[rand.Next(0, taunting_minions.Count)]);
        }
        else
        {
            attacking_minion.Attack(opponent.temp[rand.Next(0, opponent.number_of_minions)]);
        }
        if (attacking_minion.traits[5] && attacking_minion.alive && !Windfuried)
        {
            this.Invoke(() => Minion_Attack(attacker, opponent, true), 1f);
            Debug.Log("Invoked");
        } else
        {
            this.Invoke(() => Minion_Attack(opponent, attacker, false), 1f);
            Debug.Log("Invoked");
        }
    }

    public bool Check_Win(Player attacker, Player opponent)
    {
        if (attacker.number_of_minions == 0)
        {
            if (opponent.number_of_minions == 0)
            {
                Debug.Log("Win");
                return true;
            }
            else
            {
                opponent.Win(attacker);
                Debug.Log("Win");
                return true;
            }
        }
        else if (opponent.number_of_minions == 0)
        {
            attacker.Win(opponent);
            Debug.Log("Win");
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
