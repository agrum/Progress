using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellPiePiece : MonoBehaviour
{
	public Material passiveMaterial;
	public Sprite passiveImage;
	public Sprite activeImage;
	public Spell spell;
	public Image image;

	private void SpellCast()
	{
		image.material = spell.school.material;
		image.sprite = activeImage;
	}

	private void SpellPossiblyNextCast()
	{
		image.material = spell.school.material;
		image.sprite = passiveImage;
	}

	private void SpellWontBeNextCast()
	{
		image.material = passiveMaterial;
		image.sprite = passiveImage;
	}

	// Use this for initialization
	void Start ()
	{
		spell.OnSpellCast += SpellCast;
		spell.OnSpellPossiblyNextCast += SpellPossiblyNextCast;
		spell.OnSpellWontBeNextCast += SpellWontBeNextCast;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
