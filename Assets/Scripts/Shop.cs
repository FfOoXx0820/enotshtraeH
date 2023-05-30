using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Shop : MonoBehaviour
{
    public GameObject sample;
    public Player Player;
    public TextMeshProUGUI tavern_tier_text;
    public TextMeshProUGUI tavern_tier_cost_text;
    public int[] max_minion_number = { 3, 4, 4, 5, 5, 6 };
    public int[] number_of_available_minions;
    public Minion[] tier_1_minions;
    public Minion[] tier_2_minions;
    public Minion[] tier_3_minions;
    public Minion[] tier_4_minions;
    public Minion[] tier_5_minions;
    public Minion[] tier_6_minions;
    public List<Minion[]> minion_pool;

    private System.Random rand = new System.Random();
    private void Start()
    {
        number_of_available_minions = new int[6];
        minion_pool = new List<Minion[]>() { tier_1_minions, tier_2_minions, tier_3_minions, tier_4_minions, tier_5_minions, tier_6_minions};
        for (int i = 0; i < 6; i++)
        {
            number_of_available_minions[i] = minion_pool[i].Length;
        }
    }
    public void TurnStart()
    {
        gameObject.SetActive(true);
        Player._hand.SetActive(true);
        Player._battleground.SetActive(true);
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
    
    public void Freeze()
    {
        Debug.Log(Player.Freezed == false);
        Player.Freezed = Player.Freezed == false;
    }

    public void Reroll()
    {
        Player.Freezed = false;
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
}