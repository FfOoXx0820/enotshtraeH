using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Shop : MonoBehaviour
{
    public GameObject sample;
    public Player Player;
    public TextMeshProUGUI tavern_tier_text;
    public TextMeshProUGUI tavern_tier_cost_text;
    public TextMeshProUGUI timer_text;
    public TextMeshProUGUI freezeState_text;
    public int[] max_minion_number = { 3, 4, 4, 5, 5, 6 };
    public int[] number_of_available_minions;
    public Minion[] tier_1_minions;
    public Minion[] tier_2_minions;
    public Minion[] tier_3_minions;
    public Minion[] tier_4_minions;
    public Minion[] tier_5_minions;
    public Minion[] tier_6_minions;
    public List<Minion[]> minion_pool;
    public GameManager GameManager;
    private float timeValue;
    private float seconds;

    private System.Random rand = new System.Random();
    private void Start()
    {
        number_of_available_minions = new int[6];
        minion_pool = new List<Minion[]>() { tier_1_minions, tier_2_minions, tier_3_minions, tier_4_minions, tier_5_minions, tier_6_minions};
        for (int i = 0; i < 6; i++)
        {
            number_of_available_minions[i] = minion_pool[i].Length;
        }
        timeValue = 200;
    }
    private void Update()
    {
        if (GameManager.shopping_phase)
        {
            if (timeValue > 0)
            {
                timeValue -= Time.deltaTime;
                seconds = MathF.Floor(timeValue);
                timer_text.text = seconds.ToString();
            }
            else
            {
                NextTurn();
            }
        }
    }
    public void NextTurn()
    {
        GameManager.turn_count += 1;
        Debug.Log(GameManager.turn_count);
        if (GameManager.turn_count < GameManager.number_of_players)
        {
            timeValue = 200;
            seconds = MathF.Floor(timeValue);
            timer_text.text = seconds.ToString();
            if (!(Player == null)){
                Player.gameObject.SetActive(false);
                if (!Player.Freezed)
                {
                    foreach (GameObject m in Player.shop_minions)
                    {
                        Destroy(m);
                    }
                }
            }
            Player = GameManager.Players[GameManager.turn_count].GetComponent<Player>();
            GameManager.shopping_phase = true;
            gameObject.SetActive(true);
            Player.gameObject.SetActive(true);
            UpdateFreezeState(Player.Freezed);
            if (!Player.Freezed)
            {
                int n = 0;
                for (int i = 0; i < Player.tier; i++)
                {
                    n += number_of_available_minions[i];
                }
                for (int i = 0; i < max_minion_number[Player.tier - 1]; i++)
                {
                    int r = rand.Next(0, n);
                    int j = 0;
                    while (r >= number_of_available_minions[j])
                    {
                        r -= number_of_available_minions[j];
                        j += 1;
                    }
                    sample.GetComponent<Minion_>().Minion = minion_pool[j][r];
                    GameObject minion = Instantiate(sample, new Vector3(-max_minion_number[Player.tier - 1] + 1 + (2.0f * i), 3.0f, 0.0f), Quaternion.identity, gameObject.transform);
                    Player.shop_minions.Add(minion);
                }
            }
            Player.turn += 1;
            tavern_tier_text.text = Player.tier.ToString();
            Player.Gold_Update(-Player.gold);
            Player.Gold_Update(Mathf.Min(Player.turn + 2, 10));
            Player.tavern_tier_up_cost[Player.tier - 1] -= 1;
            tavern_tier_cost_text.text = "Up(" + Player.tavern_tier_up_cost[Player.tier - 1].ToString() + ")";
        }
        else if (GameManager.turn_count == GameManager.number_of_players)
        {
            if (!Player.Freezed)
            {
                foreach (GameObject m in Player.shop_minions)
                {
                    Destroy(m);
                }
            }
            Player.gameObject.SetActive(false);
            gameObject.SetActive(false);
            GameManager.turn_count = -1;
            GameManager.StartCombat();
        }
    }

    public void Freeze()
    {
        UpdateFreezeState(!Player.Freezed);
    }

    public void Reroll()
    {
        UpdateFreezeState(false);
        if (!Player.Gold_Update(-1))
        {
            Debug.Log("Nomoney");
            return;
        }
        foreach (GameObject m in Player.shop_minions)
        {
            Destroy(m);
        }
        int n = 0;
        for (int i = 0; i < Player.tier; i++)
        {
            n += number_of_available_minions[i];
        }
        for (int i = 0; i < max_minion_number[Player.tier - 1]; i++)
        {
            int r = rand.Next(0, n);
            int j = 0;
            while (r >= number_of_available_minions[j])
            {
                r -= number_of_available_minions[j];
                j += 1;
            }
            sample.GetComponent<Minion_>().Minion = minion_pool[j][r];
            GameObject minion = Instantiate(sample, new Vector3(-max_minion_number[Player.tier - 1] + 1 + (2.0f * i), 3.0f, 0.0f), Quaternion.identity, gameObject.transform);
            Player.shop_minions.Add(minion);
        }
    }

    public bool Buy(GameObject minion)
    {
        if (Player.hand_number >= 10 || !Player.shop_minions.Contains(minion))
        {
            return false;
        }
        if (!Player.Gold_Update(-3))
        {
            return false;
        }
        minion.GetComponent<Minion_>().Player = Player;
        Player.shop_minions.Remove(minion);
        Player.hand.Add(minion);
        Player.hand_number += 1;
        minion.transform.position = new Vector3(-6.75f + (1.5f * (Player.hand_number - 1)), -3.5f, 0.0f);
        minion.transform.parent = Player._hand.transform;
        return true;
    }

    public void Up()
    {
        if (!Player.Gold_Update(-Player.tavern_tier_up_cost[Player.tier - 1]))
        {
            return;
        }
        Player.tier += 1;
        tavern_tier_text.text = Player.tier.ToString();
        tavern_tier_cost_text.text = "Up(" + Player.tavern_tier_up_cost[Player.tier - 1].ToString() + ")";
    }

    public void UpdateFreezeState(bool state)
    {
        Player.Freezed = state;
        freezeState_text.text = state.ToString();
    }
}