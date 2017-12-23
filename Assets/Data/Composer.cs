using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Composer : MonoBehaviour
{
	public Spell startingSpell;
	public float resetPeriod;

	private Spell currentSpell;
	private float lastComposeTime;
	private int currentTier;

	public delegate void CastDelegate(Spell spell, int tier);

	public void KeyPressed(Key key, CastDelegate castDelegate)
	{
		if(currentSpell.school && currentSpell.school.key == key)
		{
			castDelegate(currentSpell, ++currentTier); //amplified
			Reset();
			castDelegate(currentSpell, currentTier);
			return;
		}
		else
		{
			foreach(var nextSpell in currentSpell.nextSpells)
			{
				if (nextSpell.school.key == key)
				{
					castDelegate(nextSpell, ++currentTier);
					if (nextSpell.nextSpells.Length == 0)
					{
						Reset();
						castDelegate(currentSpell, currentTier);
					}
					else
					{
						currentSpell = nextSpell;
						lastComposeTime = Time.time;
					}
					return;
				}
			}
		}
		Reset();
		castDelegate(currentSpell, currentTier);
	}

	public int NextTier()
	{
		return currentTier + 1;
	}

	void Reset ()
	{
		currentSpell = startingSpell;
		currentTier = 0;
		lastComposeTime = Time.time;
	}

	// Use this for initialization
	void Start ()
	{
		Reset();
	}
	
	// Update is called once per frame
	void Update ()
	{
		//return to square one
		if(currentSpell != startingSpell && Time.time - lastComposeTime > resetPeriod)
			Reset();
	}
}
