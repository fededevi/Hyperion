using UnityEngine;
using System.Collections;

public class Generator : MonoBehaviour
{
	public float maxEnergy = 100f;
	public float energy = 0f;
	
	public float regeneration = 10f;
		
	public SpaceObject spaceObject;
	
	void Start ()
	{
		spaceObject = (SpaceObject)transform.root.GetComponent("SpaceObject");
		spaceObject.generator = this;
	}
	
	public void OnHierarchyDetached(Section destroyedSection)
	{
		enabled = false;
	}
	
	void FixedUpdate ()
	{
		energy = energy + regeneration * Time.fixedDeltaTime;
		if (energy > maxEnergy) energy = maxEnergy; 
	}
	
	public bool UseEnergy(float amount)
	{
		if (energy > amount)
		{
			energy = energy - amount;
			return true;
		}
		return false;
	}
}

