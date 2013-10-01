using UnityEngine;
using System.Collections;

public class ControlPoint : MonoBehaviour
{
	public SpaceObject.Standing owner;
	public ListsKeeper listsKeeper;
	public ControlPoint[] connectedControlPoints;
	public CoreShip core;
	
	public bool canBeCapturedByFriendly = false;
	public bool canBeCapturedByEnemy = false;
	public float enemyShipInside = 0f;
	public float friendlyShipInside = 0f;
	public float radius = 40f;
	private float timer = 1f;
	public float status = 0f;
	
	GameObject contour;
	LineRenderer contuourRenderer;
	public Material controlPointContourMaterial;
	public Material controlPointContourMaterialFriendly;
	public Material controlPointContourMaterialEnemy;
	
	void Awake ()
	{
		GameObject list = GameObject.Find("ListsKeeper");
		listsKeeper = list.GetComponent<ListsKeeper>();
		
		foreach ( ControlPoint cp in connectedControlPoints)
		{
			GameObject go = new GameObject();
			go.transform.parent = transform;
			go.name = this.name + " Link to " + cp.name;
			LineRenderer lr = go.AddComponent<LineRenderer>();
			lr.SetPosition(0,transform.position);
			lr.SetPosition(1,cp.transform.position);
			lr.SetVertexCount(2);
			lr.SetWidth(0.4f,0.4f);
			lr.material = listsKeeper.controlPointLinksMaterial;
		}
		if (core != null)
		{
			GameObject go = new GameObject();
			
			go.transform.parent = transform;
			go.name = this.name + " Link to " + core.name;
			LineRenderer lr = go.AddComponent<LineRenderer>();
			lr.SetPosition(0,transform.position);
			lr.SetPosition(1,core.transform.position);
			lr.SetVertexCount(2);
			
			lr.SetWidth(0.4f,0.4f);
			lr.material = listsKeeper.controlPointLinksMaterial;
		}
	}
	
	void Start ()
	{
		listsKeeper.AddControlPoint(this);
		
		contour  = new GameObject();
		contour.transform.parent = transform;
		contuourRenderer = contour.AddComponent<LineRenderer>();
		contuourRenderer.SetVertexCount (70);
		contuourRenderer.useWorldSpace = false;
		contuourRenderer.material = controlPointContourMaterial;
		int index = 0;
		for (float i = 0f; i < Mathf.PI * 2f; i = i + (Mathf.PI * 2f) / 60f) 
		{
			contuourRenderer.SetPosition (index++, new Vector3 (transform.position.x + radius * Mathf.Sin (i),transform.position.y + radius * Mathf.Cos (i), 0f));
			
		}
		contuourRenderer.SetPosition (index++, new Vector3 (transform.position.x + radius * Mathf.Sin (0f),transform.position.y + radius * Mathf.Cos (0f), 0f));
		contuourRenderer.SetVertexCount (index);
		contuourRenderer.SetWidth(0.3f,0.3f);
	}

	void Update ()
	{
		
		timer -= Time.deltaTime;
		if (timer< 0f)
		{
			timer = 1f + Random.value*0.1f;
			CheckStatus();
		}
		
		if (canBeCapturedByEnemy)
		{
			if (enemyShipInside > 0f) status -= Time.deltaTime;
		}
		if (canBeCapturedByFriendly)
		{
			if (friendlyShipInside > 0f)status += Time.deltaTime;
		}
		if (status > 10f)
		{
			status = 10.0f;
			owner = SpaceObject.Standing.Friendly;
			contuourRenderer.material = controlPointContourMaterialFriendly;
		}
		else if (status < -10f)
		{
			status = -10.0f;
			owner = SpaceObject.Standing.Enemy;
			contuourRenderer.material = controlPointContourMaterialEnemy;
		} else 
		{
			contuourRenderer.material = controlPointContourMaterial;
			owner = SpaceObject.Standing.Neutral;
		}
	}
	
	private void CheckStatus()
	{
		canBeCapturedByFriendly = false;
		canBeCapturedByEnemy = false;
		enemyShipInside = 0f;
		friendlyShipInside = 0f;
		
		foreach ( ControlPoint cp in connectedControlPoints )
		{
			if (cp.owner == SpaceObject.Standing.Enemy) canBeCapturedByEnemy = true;
			if (cp.owner == SpaceObject.Standing.Friendly) canBeCapturedByFriendly = true;
		}
		foreach ( SpaceObject s in listsKeeper.spaceObjects)
		{
			float distance = Vector3.Distance(transform.position,s.transform.position);
			if (distance < radius)
			{
				if (s.standing == SpaceObject.Standing.Enemy)  enemyShipInside += 1f;
				if (s.standing == SpaceObject.Standing.Friendly) friendlyShipInside += 1f;
			}
		}
		if (core != null)
		{
			if (core.spaceObject.standing == SpaceObject.Standing.Enemy) 
			{
				canBeCapturedByEnemy = true;
				if (owner == SpaceObject.Standing.Friendly)  
				{
					core.spaceObject.ship.godMode = false;
				} else 
					core.spaceObject.ship.godMode = true;
			}
			if (core.spaceObject.standing == SpaceObject.Standing.Friendly) 
			{
				canBeCapturedByFriendly = true;
				if (owner == SpaceObject.Standing.Enemy)  
				{
					core.spaceObject.ship.godMode = false;
				} else core.spaceObject.ship.godMode = true;
			}
		}
	}
	
}

