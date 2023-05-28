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

	public TextMeshPro tierText;
	public TextMeshPro attack_text;
	public TextMeshPro health_text;

    public int attack;
    public int health;
    public bool[] traits;

    public bool alive;
    private void Start()
    {
        attack = Minion.default_attack;
        attack = Minion.default_health;
        traits = Minion.default_traits;
        nameText.text = Minion.name;
        descriptionText.text = Minion.description;

        artworkImage.sprite = Minion.artwork;

        tierText.text = Minion.tier.ToString();
        attack_text.text = Minion.default_attack.ToString();
        health_text.text = Minion.default_health.ToString();
        alive = true;
        Attack_Update(Minion.default_attack);
        Health_Update(Minion.default_health);
    }
    public void Attack(Minion_ enemy)
    {
        this.GetComponent<SpriteRenderer>().color = Color.red;
        enemy.GetComponent<SpriteRenderer>().color = Color.green;
        Hit(enemy.attack);
        enemy.Hit(attack);
    }

    public void Hit(int damage)
    {
        if (traits[2])
        {
            traits[2] = false;
        }
        else
        {
            Health_Update(health - damage);
        }
        if (health <= 0)
        {
            Die();
            Debug.Log("Die");
        }
    }

    public void Die()
    {
        if (traits[1])
        {

        }
        if (traits[3])
        {
            Health_Update(1);
            traits[3] = false;
            //Player.i -= 1;
        }
        else
        {
            Player.number_of_minions -= 1;
            alive = false;
            gameObject.SetActive(false);
        }
    }
    public void Attack_Update(int new_attack)
    {
        attack = new_attack;
        attack_text.text = attack.ToString();
    }
    public void Health_Update(int new_health)
    {
        health = new_health;
        health_text.text = health.ToString();
    }
}
