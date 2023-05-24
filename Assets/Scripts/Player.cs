using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    public int health;
    public int tier;
    public int gold;
    public int hand_number;
    public int number_of_minions;
    public int i = -1;
    public TextMeshProUGUI gold_text;
    public List<Minion> battleground;
    public List<GameObject> hand;

    private void Start()
    {
        health = 30;
        tier = 1;
        hand_number = 0;
        number_of_minions = 0;
        i = -1;
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
    public bool Gold_Update(int change)
    {
        if (gold + change >= 0)
        {
            gold += change;
            gold_text.text = gold.ToString();
            return true;
        }
        return false;
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
        if (minion.GetComponent<Minion>().traits[0])
        {

        }
        Remove_minion_from_hand(minion);
        battleground.Add(minion.GetComponent<Minion>());
        hand_number -= 1;
        minion.transform.position = new Vector3(-7.5f + 1.5f * (battleground.Count - 1), -2.0f, 0.0f);
        return true;
    }
    public bool Sell(GameObject minion)
    {
        if (!battleground.Contains(minion.GetComponent<Minion>())){
            return false;
        }
        Remove_minion_from_battleground(minion.GetComponent<Minion>());
        Gold_Update(1);
        Destroy(minion);
        return true;
    }

    public void Remove_minion_from_hand(GameObject minion)
    {
        int index = hand.IndexOf(minion);
        for (int i = index; i < hand.Count; i++)
        {
            hand[i].transform.position = new Vector3(-7.5f + 1.5f * (i - 1), -4.0f, 0.0f);
        }
        hand.Remove(minion);
    }
    public void Remove_minion_from_battleground(Minion minion)
    {
        int index = battleground.IndexOf(minion);
        for (int i = index; i < battleground.Count; i++)
        {
            battleground[i].transform.position = new Vector3(-7.5f + 1.5f * (i - 1), -2.0f, 0.0f);
        }
        battleground.Remove(minion);
    }
}
