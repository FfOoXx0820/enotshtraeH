using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Minion_ : MonoBehaviour {

    public Player Player;

    public Minion Minion;

	public TextMeshPro nameText;
	public TextMeshPro descriptionText;

	public Image artworkImage;

	public TextMeshPro manaText;
	public TextMeshPro attack_text;
	public TextMeshPro health_text;

    public bool alive;
    private void Start()
    {
        nameText.text = Minion.name;
        descriptionText.text = Minion.description;

        artworkImage.sprite = Minion.artwork;

        manaText.text = Minion.manaCost.ToString();
        attack_text.text = Minion.attack.ToString();
        health_text.text = Minion.health.ToString();
        alive = true;
        Attack_Update(Minion.attack);
        Health_Update(Minion.health);
    }
    public void Attack(Minion_ enemy)
    {
        this.GetComponent<SpriteRenderer>().color = Color.red;
        enemy.GetComponent<SpriteRenderer>().color = Color.green;
        Hit(enemy.Minion.attack);
        enemy.Hit(Minion.attack);
    }

    public void Hit(int damage)
    {
        if (Minion.traits[2])
        {
            Minion.traits[2] = false;
        }
        else
        {
            Health_Update(Minion.health - damage);
        }
        if (Minion.health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        if (Minion.traits[1])
        {

        }
        if (Minion.traits[3])
        {
            Health_Update(1);
            Minion.traits[3] = false;
            //Player.i -= 1;
        }
        else
        {
            Player.number_of_minions -= 1;
            Player.battleground.Remove(this);
            alive = false;
            gameObject.SetActive(false);
        }
    }
    public void Attack_Update(int new_attack)
    {
        Minion.attack = new_attack;
        attack_text.text = Minion.attack.ToString();
    }
    public void Health_Update(int new_health)
    {
        Minion.health = new_health;
        health_text.text = Minion.health.ToString();
    }
}
