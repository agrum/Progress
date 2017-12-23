using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : MonoBehaviour
{
	public SpellSchool school;
	public Spell[] nextSpells;

	public delegate void SpellCastDelegate();
	public delegate void SpellPossiblyNextCastDelegate();
	public delegate void SpellWontBeNextCastDelegate();
	public event SpellCastDelegate OnSpellCast;
	public event SpellPossiblyNextCastDelegate OnSpellPossiblyNextCast;
	public event SpellPossiblyNextCastDelegate OnSpellWontBeNextCast;

	public void Cast()
	{
		if (OnSpellCast != null)
			OnSpellCast();
		foreach (var nextSpell in nextSpells)
		{
			if (nextSpell.OnSpellCast != null)
			{
				nextSpell.OnSpellPossiblyNextCast();
				nextSpell.ResetNextCasts();
			}
		}
	}

	public void WontBeCastNext()
	{
		if (OnSpellWontBeNextCast != null)
			OnSpellWontBeNextCast();
	}

	private void ResetNextCasts()
	{
		/*foreach (var nextSpell in nextSpells)
		{
			if (nextSpell.OnSpellWontBeNextCast != null)
			{
				nextSpell.OnSpellWontBeNextCast();
				nextSpell.ResetNextCasts();
			}
		}*/
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
