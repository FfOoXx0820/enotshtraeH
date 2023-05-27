using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class CardDisplay : MonoBehaviour {

	public Card card;

	public TextMeshPro nameText;
	public TextMeshPro descriptionText;

	public Image artworkImage;

	public TextMeshPro manaText;
	public TextMeshPro attackText;
	public TextMeshPro healthText;

	// Use this for initialization
	void Start () {
		nameText.text = card.name;
		descriptionText.text = card.description;

		artworkImage.sprite = card.artwork;

		manaText.text = card.manaCost.ToString();
		attackText.text = card.attack.ToString();
		healthText.text = card.health.ToString();
	}
	
}
