using UnityEngine;
using System.Collections;

public class CoreShip : MonoBehaviour
{
	public GUIText one;
	public GUIText two;
	
	public GameObject friendlyCore;
	public GameObject enemyCore;

	public float resources = 1000f;
	public float maxResources = 10000f;
	public float resourceRecover = 200f;
	
	public float supplies = 150;
	public float maxSupplies = 150;

	public ListsKeeper listsKeeper;

	public Transform spawnPosition;

	public SpaceObject spaceObject;

	void Start ()
	{
		spaceObject = transform.root.GetComponent<SpaceObject> ();
	}


	void Update ()
	{
		resources += resourceRecover * Time.deltaTime;
		if (resources > maxResources)
			resources = maxResources;
		
		if (Input.GetKeyDown ("1")) SpawnShip (0);
		if (Input.GetKeyDown ("2")) SpawnShip (1);
		if (Input.GetKeyDown ("3")) SpawnShip (2);
		if (Input.GetKeyDown ("4")) SpawnShip (3);
		
		if (one != null)
		{
			one.text = "Resources: " + resources + "/1000"; 
		}
		if (two != null)
		{
			two.text = "Supplies: " + supplies + "/15"; 
		}
	}
	
	public bool CheckResources(Ship s)
	{
		if (resources > s.resourcesCost && supplies > s.supplyCost ) 
		{
			resources = resources - s.resourcesCost;
			supplies = supplies - s.supplyCost;
			return true;
		} else return false;	
		
	}

	public SpaceObject SpawnShip (int type)
	{
		
		int i = type;
		
		if (spaceObject.standing == SpaceObject.Standing.Enemy) 
		{
			if (!CheckResources(listsKeeper.enemyShipsPrefabs[i].GetComponent<Ship>())) 
			{
				Debug.Log("Not enough resources or supplies to warp ship in: " + listsKeeper.enemyShipsPrefabs[i]);
				return null;
			}
			GameObject newShip = (GameObject)Instantiate (listsKeeper.enemyShipsPrefabs[i], spawnPosition.position, spawnPosition.rotation);
			SpaceObject newSpaceObject = newShip.GetComponent<SpaceObject> ();
			newSpaceObject.listsKeeper = spaceObject.listsKeeper;
			newSpaceObject.standing = spaceObject.standing;
			SpaceObject.SetLayerRecursively (newShip, gameObject.layer);
			Movement mov = newSpaceObject.GetComponent<Movement> ();
			mov.headingToTarget = true;
			mov.movingToTarget = true;
			mov.moveTarget = friendlyCore.transform.position + (Vector3)Random.insideUnitCircle * 25f;
			return newSpaceObject;
		} else 
		{
			if (!CheckResources(listsKeeper.friendlyShipsPrefabs[i].GetComponent<Ship>())) 
			{
				Debug.Log("Not enough resources to warp ship in: " + listsKeeper.friendlyShipsPrefabs[i]);
				return null;
			}
			GameObject newShip = (GameObject)Instantiate (listsKeeper.friendlyShipsPrefabs[i], spawnPosition.position, spawnPosition.rotation);
			SpaceObject newSpaceObject = newShip.GetComponent<SpaceObject> ();
			SpaceObject.SetLayerRecursively (newShip, gameObject.layer);
			newSpaceObject.standing = spaceObject.standing;
			newSpaceObject.listsKeeper = spaceObject.listsKeeper;
			Movement mov = newSpaceObject.GetComponent<Movement> ();
			mov.headingToTarget = true;
			mov.movingToTarget = true;
			mov.headingTarget = new Vector2(1f,0f);
			Vector3 a = (Vector3)Random.insideUnitCircle * 25f;
			a.x +=30f;
			mov.moveTarget = friendlyCore.transform.position + a;
			return newSpaceObject;
		}
	}
}
