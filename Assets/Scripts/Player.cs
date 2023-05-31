using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    public new string name;
    public int health;
    public int tier;
    public int gold;
    public int hand_number;
    public int number_of_minions;
    public int i = -1;
    public GameObject _hand;
    public GameObject _battleground;
    public TextMeshProUGUI gold_text;
    public TextMeshProUGUI name_text;
    public TextMeshProUGUI health_text;
    public List<Minion_> battleground;
    public List<Minion_> temp;
    public List<GameObject> hand;
    public List<GameObject> shop_minions;
    public int[] tavern_tier_up_cost = { 6, 7, 8, 9, 10 };
    public bool Freezed;
    public int turn;
    private void Start()
    {
        health = 30;
        tier = 1;
        hand_number = 0;
        number_of_minions = 0;
        i = -1;
        Freezed = false;
        turn = 0;
    }
    public void Win(Player opponent)
    {
        int damage = tier;
        foreach (Minion_ minion in battleground)
        {
            damage += minion.Minion.tier;
        }
        opponent.Health_Update(damage);
    }
    public bool Gold_Update(int change)
    {
        if (gold + change >= 0)
        {
            gold += change;
            gold_text.text = gold.ToString();
            return true;
        }
        Debug.Log("Nomoney");
        return false;
    }
    public void Health_Update(int damage)
    {
        health -= damage;
        health_text.text = health.ToString();
        if (health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {

    }

    public bool Play(GameObject minion)
    {
        if (battleground.Count >= 7 || !hand.Contains(minion))
        {
            return false;
        }
        if (minion.GetComponent<Minion_>().traits[0])
        {

        }
        Remove_minion_from_hand(minion);
        battleground.Add(minion.GetComponent<Minion_>());
        hand_number -= 1;
        minion.transform.position = new Vector3(-4.75f + 1.5f * (battleground.Count - 1), -1.0f, 0.0f);
        minion.transform.parent = _battleground.transform;
        minion.GetComponent<Minion_>().Player = this;
        return true;
    }
    public bool Sell(GameObject minion)
    {
        if (!battleground.Contains(minion.GetComponent<Minion_>())){
            return false;
        }
        Remove_minion_from_battleground(minion.GetComponent<Minion_>());
        Gold_Update(1);
        Destroy(minion);
        return true;
    }

    public void Remove_minion_from_hand(GameObject minion)
    {
        int index = hand.IndexOf(minion);
        for (int i = index; i < hand.Count; i++)
        {
            hand[i].transform.position = new Vector3(-6.75f + 1.5f * (i - 1), -3.5f, 0.0f);
        }
        hand.Remove(minion);
    }
    public void Remove_minion_from_battleground(Minion_ minion)
    {
        int index = battleground.IndexOf(minion);
        for (int i = index; i < battleground.Count; i++)
        {
            battleground[i].transform.position = new Vector3(-4.75f + 1.5f * (i - 1), -1.0f, 0.0f);
        }
        battleground.Remove(minion);
    }
}
