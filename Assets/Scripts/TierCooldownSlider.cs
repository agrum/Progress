using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TierCooldownSlider : MonoBehaviour {
	public Cooldown cooldown;
	public Slider slider;
	public Composer composer;
	public Player player;
	public int tier;
	public RectTransform fill;
	public Material nonActiveTierMaterial;
	public Material activeTierHealthMaterial;
	public Material activeTierManaMaterial;

	private bool usesHealthMaterial = false;
	private int previousTierObserved = -1;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		slider.value = cooldown.CooledPercentage();
		int nextTier = composer.NextTier();
		if (nextTier != previousTierObserved || usesHealthMaterial)
		{
			if (nextTier == tier)
			{
				usesHealthMaterial = slider.value < player.resourceAffinity;
				fill.GetComponent<Image>().material = usesHealthMaterial ? activeTierHealthMaterial : activeTierManaMaterial;
			}
			else
			{
				fill.GetComponent<Image>().material = nonActiveTierMaterial;
			}
			previousTierObserved = nextTier;
		}
	}
}
