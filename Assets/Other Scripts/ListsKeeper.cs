using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ListsKeeper : MonoBehaviour 
{
	public List<GameObject> friendlyShipsPrefabs;
	public List<GameObject> enemyShipsPrefabs;
	
	public CoreShip enemyCore;
	public CoreShip friendlyCore;
	
	public List<SpaceObject> spaceObjects = new List<SpaceObject>();
	
	public List<SpaceObject> friendlySpaceObjects = new List<SpaceObject>();
	public List<SpaceObject> enemySpaceObjects = new List<SpaceObject>();
	
	public List<ControlPoint> controlPoints = new List<ControlPoint>();
	public List<Ship> selection;
	
	public Camera mainCamera;
	public Material movementLineMaterial;
	public Material headingLineMaterial;
	public Material weaponLineMaterial;
	public Material selectionBoxLineMaterial;
	public Material selectionBoxInsideLineMaterial;
	public Material unitCircleSelected;
	public Material unitCircleUnselected;
	public Material enemyCircle;
	public Material controlPointLinksMaterial;
	
	//EXPLOSIONS
	public GameObject sectionExplosionSmall;
	public GameObject sectionExplosionMedium;
	public GameObject sectionExplosionBig;
	public GameObject sectionExplosionHuge;
	
	public bool inCombat = false;
	public float combatTime = 0f;
	public float missionTime = 0f;
	
	public void AddSpaceObject(SpaceObject spaceObject)
	{
		spaceObjects.Add(spaceObject);
	}
	
	public void AddControlPoint(ControlPoint controlPoint)
	{
		controlPoints.Add(controlPoint);
	}
	
	public void Update()
	{
		missionTime += Time.deltaTime;
		if ( inCombat ) combatTime += Time.deltaTime;
		if ( combatTime > 10f ) inCombat = false;
		
		
		friendlySpaceObjects = new List<SpaceObject>();
		enemySpaceObjects = new List<SpaceObject>();
		foreach (SpaceObject s in spaceObjects)
		{
			if (s.IsFriendly()) 
			{
				friendlySpaceObjects.Add(s);
			} else enemySpaceObjects.Add(s);
		}
	}
	
	public void SetInCombat()
	{
		inCombat = true;
		combatTime = 0f;
	}
	
}
