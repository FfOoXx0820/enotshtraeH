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
        gold = 10;
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
    public void Gold_Update(int change)
    {
        gold += change;
        gold_text.text = gold.ToString();
    }

    public void Die()
    {

    }

    public void Play(GameObject minion)
    {
        if (minion.GetComponent<Minion>().traits[0])
        {

        }
        hand.Remove(minion);
        battleground.Add(minion.GetComponent<Minion>());
        minion.transform.position = new Vector3(-4.5f + 1.5f * (battleground.Count - 1), -2.0f, 0.0f);
    }
}
