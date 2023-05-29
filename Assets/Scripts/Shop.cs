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
    public int[] tavern_tier_up_cost = { 6, 7, 8, 9, 10};
    public int[] max_minion_number = { 3, 4, 4, 5, 5, 6 };
    public List<GameObject> shop_minions;
    public int turn;
    //public Minion[] available_minions;
    private void Start()
    {
        turn = 0;
    }
    public void TurnStart()
    {
        gameObject.SetActive(true);
        for (int i = 0; i < max_minion_number[Player.tier - 1]; i++)
        {
            GameObject minion = Instantiate(sample, new Vector3(-max_minion_number[Player.tier - 1] + 1 + (2.0f * i), 3.0f, 0.0f), Quaternion.identity, gameObject.transform);
            shop_minions.Add(minion);
        }
        turn += 1;
        tavern_tier_text.text = Player.tier.ToString();
        Player.Gold_Update(-Player.gold);
        Player.Gold_Update(Mathf.Min(turn + 2, 10));
        tavern_tier_up_cost[Player.tier - 1] -= 1;
        tavern_tier_cost_text.text = "Up(" + tavern_tier_up_cost[Player.tier - 1].ToString() + ")";
    }

    public void Reroll()
    {
        if (!Player.Gold_Update(-1))
        {
            Debug.Log("Nomoney");
            return;
        }
        foreach (GameObject m in shop_minions)
        {
            Destroy(m.gameObject);
        }
        for (int i = 0; i < max_minion_number[Player.tier - 1]; i++)
        {
            GameObject minion = Instantiate(sample, new Vector3(-max_minion_number[Player.tier - 1] + 1 + (2.0f * i), 3.0f, 0.0f), Quaternion.identity, gameObject.transform);
            shop_minions.Add(minion);
        }
    }

    public bool Buy(GameObject minion)
    {
        if (Player.hand_number >= 10 || !shop_minions.Contains(minion))
        {
            return false;
        }
        if (!Player.Gold_Update(-3))
        {
            return false;
        }
        minion.GetComponent<Minion_>().Player = Player;
        shop_minions.Remove(minion);
        Player.hand.Add(minion);
        Player.hand_number += 1;
        minion.transform.position = new Vector3(-6.75f + (1.5f * (Player.hand_number - 1)), -3.5f, 0.0f);
        minion.transform.parent = gameObject.transform;
        return true;
    }

    public void Up()
    {
        if (!Player.Gold_Update(-tavern_tier_up_cost[Player.tier - 1]))
        {
            return;
        }
        Player.tier += 1;
        tavern_tier_text.text = Player.tier.ToString();
        tavern_tier_cost_text.text = "Up(" + tavern_tier_up_cost[Player.tier - 1].ToString() + ")";
    }
}