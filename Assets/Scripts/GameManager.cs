using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public Shop Shop;
    public GameObject PlayerSample;
    public Canvas Canvas;
    public GameObject StartMenu_panel;
    public GameObject[] Players;
    public int turn_count;
    public int number_of_players;
    public bool shopping_phase;
    public TextMeshProUGUI VS_text;
    public TMP_InputField PlayerName_Inputfield;
    public TextMeshProUGUI[] Player_Slot;

    private System.Random rand = new System.Random();
    private void Start()
    {
        number_of_players = 0;
        Players = new GameObject[8];
        Shop.gameObject.SetActive(false);
        PlayerSample.gameObject.SetActive(true);
        VS_text.gameObject.SetActive(false);
        shopping_phase = false;
    }
    public void AddPlayer()
    {
        if (number_of_players == 8)
        {
            Debug.Log("Full");
            return;
        }
        if (!(PlayerName_Inputfield.text == ""))
        {
            Player_Slot[number_of_players].text = PlayerName_Inputfield.text;
            GameObject player = Instantiate(PlayerSample, new Vector3(0.0f, -100.0f, 0.0f), Quaternion.identity, Canvas.transform);
            Players[number_of_players] = player;
            player.GetComponent<Player>().name = PlayerName_Inputfield.text;
            player.GetComponent<Player>().name_text.text = player.GetComponent<Player>().name;
            number_of_players += 1;
        }
    }
    public void GameStart()
    {
        StartMenu_panel.SetActive(false);
        PlayerSample.SetActive(false);
        foreach (GameObject p in Players)
        {
            if (p == null)
            {
                break;
            }
            Debug.Log("not null");
            p.transform.position = Vector3.zero;
            p.GetComponent<Player>().health_text.gameObject.transform.position = new Vector3(-7.5f, -1.5f, 0.0f);
            p.GetComponent<Player>().name_text.gameObject.transform.position = new Vector3(-7.5f, -0.5f, 0.0f);
            p.gameObject.SetActive(false);
        }
        turn_count = -1;
        Shop.NextTurn();
    }

    public void StartCombat()
    {
        shopping_phase = false;
        Fight(Players[0].GetComponent<Player>(), Players[1].GetComponent<Player>());
    }
    public void Fight(Player Player1, Player Player2)
    {
        Player1.gameObject.SetActive(true);
        Player2.gameObject.SetActive(true);
        Player1._Minions.SetActive(false);
        Player2._Minions.SetActive(false);
        Player1.health_text.gameObject.transform.position = new Vector3(0.0f, -4.5f, 0.0f);
        Player1.name_text.gameObject.transform.position = new Vector3(0.0f, -3.5f, 0.0f);
        Player2.health_text.gameObject.transform.position = new Vector3(0.0f, 4.5f, 0.0f);
        Player2.name_text.gameObject.transform.position = new Vector3(0.0f, 3.5f, 0.0f);
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
        VS_text.gameObject.SetActive(false);
        foreach (GameObject player in Players)
        {
            if (player == null)
            {
                break;
            }
            Player p = player.GetComponent<Player>();
            foreach (Minion_ m in p.temp)
            {
                Destroy(m.gameObject);
            }
            p.temp = new List<Minion_>();
            p.health_text.gameObject.transform.position = new Vector3(-7.5f, -1.5f, 0.0f);
            p.name_text.gameObject.transform.position = new Vector3(-7.5f, -0.5f, 0.0f);
            p._Minions.SetActive(true);
            player.SetActive(false);
        }
        Shop.NextTurn();
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
        List<Minion_> taunting_minions = new();
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
