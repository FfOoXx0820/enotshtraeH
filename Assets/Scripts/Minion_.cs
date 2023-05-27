using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class CardDisplay : MonoBehaviour {

	public Minion Minion;

	public TextMeshPro nameText;
	public TextMeshPro descriptionText;

	public Image artworkImage;

	public TextMeshPro manaText;
	public TextMeshPro attackText;
	public TextMeshPro healthText;

	// Use this for initialization
	void Start () {
		nameText.text = Minion.name;
		descriptionText.text = Minion.description;

		artworkImage.sprite = Minion.artwork;

		manaText.text = Minion.manaCost.ToString();
		attackText.text = Minion.attack.ToString();
		healthText.text = Minion.health.ToString();
	}
	
}
