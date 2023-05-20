using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Shop : MonoBehaviour
{
    public GameObject sample;
    public Player Player;
    public TextMeshProUGUI tavern_tier_text;
    public int[] tavern_tier_up_cost = { 6, 7, 8, 9, 10};
    public int[] max_minion_number = { 3, 4, 4, 5, 5, 6 };
    public List<GameObject> shop_minions;
    //public Minion[] available_minions;
    private void Start()
    {
        for (int i = 0; i < max_minion_number[Player.tier - 1]; i++)
        {
            GameObject minion = Instantiate(sample, new Vector3((3.0f - 3.0f * max_minion_number[Player.tier - 1]) / 4.0f + (1.5f * i), 3.0f, 0.0f), Quaternion.identity/*, gameObject.transform*/);
            shop_minions.Add(minion);
        }
    }
    public void Reroll()
    {
        Player.Gold_Update(-1);
        foreach (GameObject m in shop_minions)
        {
            Destroy(m);
        }
        for (int i = 0; i < max_minion_number[Player.tier - 1]; i++)
        {
            GameObject minion = Instantiate(sample, new Vector3((3.0f - 3.0f * max_minion_number[Player.tier - 1]) / 4.0f + (1.5f * i), 3.0f, 0.0f), Quaternion.identity);
            shop_minions.Add(minion);
        }
    }

    public bool Buy(GameObject minion)
    {
        if (Player.hand_number >= 10 || !shop_minions.Contains(minion))
        {
            return false;
        }
        Player.Gold_Update(-3);
        shop_minions.Remove(minion);
        Player.hand.Add(minion);
        Player.hand_number += 1;
        minion.transform.position = new Vector3(-7.5f + (1.5f * (Player.hand_number - 1)), -4.0f, 0.0f);
        return true;
    }

    public void Up()
    {
        Player.Gold_Update(-tavern_tier_up_cost[Player.tier-1]);
        Player.tier += 1;
        tavern_tier_text.text = Player.tier.ToString();
    }
}