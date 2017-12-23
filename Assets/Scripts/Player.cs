using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	public Cooldown[] tierCooldowns;

	public Composer composer;
	public Keybindings keybindings;
	public float resourceAffinity = 0.5f;

	public List<Resource> healthResourceList;

	private Spell lastCastSpell;

	public void Cast(Spell spell, int tier)
	{
		if(lastCastSpell != null)
		{
			foreach(var nextSpell in lastCastSpell.nextSpells)
			{
				if (nextSpell != spell)
					nextSpell.WontBeCastNext();
			}
			lastCastSpell.WontBeCastNext();
		}

		tierCooldowns[tier].Consume();
		spell.Cast();
		lastCastSpell = spell;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKeyDown(keybindings.yellow.value))
			composer.KeyPressed(keybindings.yellow, Cast);
		else if (Input.GetKeyDown(keybindings.red.value))
			composer.KeyPressed(keybindings.red, Cast);
		else if (Input.GetKeyDown(keybindings.green.value))
			composer.KeyPressed(keybindings.green, Cast);
		else if (Input.GetKeyDown(keybindings.blue.value))
			composer.KeyPressed(keybindings.blue, Cast);
	}
}
