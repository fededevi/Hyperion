using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyAI : MonoBehaviour
{
	
	public ListsKeeper listsKeeper;
	public CoreShip coreShip;
	
	//FUZZY LOGIC VARIABLES
	public float needForShips = 0f;
	
	public List<ControlPoint> controlsPointToDefend = new List<ControlPoint>();
	public List<ControlPoint> controlsPointToAttack = new List<ControlPoint>();
	
	private float timeCounter;
	
	GameObject target;
	
	void Awake ()
	{
		timeCounter = Random.value;
		target = coreShip.gameObject;
	}
	
	void Start ()
	{
		
	}

	void Update ()
	{
		
	}
	
	void LateUpdate ()
	{
		
	}
	
	void FixedUpdate ()
	{
		timeCounter += Time.fixedDeltaTime;	
		if (timeCounter > 1f) 
		{
			OnEverySecond();
			timeCounter = 0f;
		}
	}
	
	void OnEverySecond ()
	{
		PopulateControlPointLists();
		CallShips();
		CheckShips();
	}
	
	void PopulateControlPointLists()
	{
		controlsPointToDefend = new List<ControlPoint>();
		controlsPointToAttack = new List<ControlPoint>();
		foreach (ControlPoint p in listsKeeper.controlPoints)
		{
			if ( p.canBeCapturedByFriendly && p.owner == SpaceObject.Standing.Enemy ) controlsPointToDefend.Add (p);
			if ( p.canBeCapturedByEnemy && p.owner != SpaceObject.Standing.Enemy) controlsPointToAttack.Add (p);
			
			if ( p.canBeCapturedByEnemy && p.owner != SpaceObject.Standing.Enemy) target = p.gameObject;
		}
		foreach (ControlPoint p in listsKeeper.controlPoints)
		{

			if ( p.owner == SpaceObject.Standing.Enemy)
			{
				if (p.core !=null)
				{
					if (p.core.spaceObject.IsFriendly()) target = p.core.gameObject;
				}
			}
		}
		//controlsPointToAttack.so
	}
	
	void CallShips()
	{
		SpaceObject newShip;
		int i = Mathf.FloorToInt(Random.value*3f);
		newShip = coreShip.SpawnShip(i);
		
		if (newShip != null && newShip.movement != null)
		{
			Debug.Log("Setting target: " + target);
			newShip.movement.moveTarget = target.transform.position + (Vector3)Random.insideUnitCircle * 15f;
			newShip.movement.headingTarget = (Vector3)Random.insideUnitCircle;
			newShip.movement.movingToTarget = true;
			newShip.movement.headingToTarget = true;
			Debug.Log("Setting target finish");
		}
	}
	
	void CheckShips()
	{
		foreach (SpaceObject s in listsKeeper.enemySpaceObjects)
		{
			if (isInsideEnemyControlPoint(s))
			{
				s.movement.moveTarget = target.transform.position + (Vector3)Random.insideUnitCircle * 15f;
				s.movement.headingTarget = (Vector3)Random.insideUnitCircle;
			}
		}
	}
	
	bool isInsideEnemyControlPoint( SpaceObject s )
	{
		return true;
	}
	
}

