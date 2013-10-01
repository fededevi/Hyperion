using UnityEngine;
using System.Collections;

public class DebugSpawner : MonoBehaviour {

	public int inputMode = 1;
	public SpaceObject spaceObject;
	
	public GameObject[] toSpawn;

	void Start ()
	{
		spaceObject = (SpaceObject)transform.root.GetComponent ("SpaceObject");
	}

	void Update ()
	{

		if (Input.GetKeyDown ("g")) 
		{
			spaceObject.ship.godMode = !spaceObject.ship.godMode;
		}
		if (Input.GetKeyDown ("s")) 
		{
			foreach (GameObject g in toSpawn)
			{
				Vector3 r = Random.insideUnitSphere*25f;
				Instantiate(g,r, Quaternion.identity);
			}
		}
	}

	

}
