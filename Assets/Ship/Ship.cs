using UnityEngine;
using System.Collections;

public class Ship : MonoBehaviour 
{
	public float resourcesCost = 200f;
	public float supplyCost = 2f;
	
	public float radius = 0f;
	public bool godMode = false;
	public bool selected = false;
	
	public SpaceObject spaceObject;
	public LineRenderer lr;
	
	public GameObject line;
	public LineRenderer lr2;
	
	public GameObject selectionBox;
	public LineRenderer selectionBoxRenderer;
		
	public Material m;
	
	void Start () 
	{
		spaceObject = (SpaceObject) transform.root.GetComponent<SpaceObject>();
		spaceObject.ship = this;
		
		
		lr = gameObject.AddComponent<LineRenderer>();
		lr.material = spaceObject.listsKeeper.movementLineMaterial;
		line = new GameObject();
		line.transform.parent = transform;
		lr2 = line.AddComponent<LineRenderer>();
		lr2.material = spaceObject.listsKeeper.headingLineMaterial;
		
		AddChildrenToBounds(  );
		
		selectionBox  = new GameObject();
		selectionBox.transform.parent = transform;
		

		selectionBoxRenderer = selectionBox.AddComponent<LineRenderer>();
		selectionBoxRenderer.SetVertexCount (70);
		selectionBoxRenderer.useWorldSpace = false;
		selectionBoxRenderer.material = spaceObject.listsKeeper.unitCircleUnselected;
		int index = 0;
		for (float i = 0f; i < Mathf.PI * 2f; i = i + (Mathf.PI * 2f) / 60f) 
		{
			selectionBoxRenderer.SetPosition (index++, new Vector3 (transform.position.x + radius * Mathf.Sin (i),transform.position.y + radius * Mathf.Cos (i), 0f));
		}
		selectionBoxRenderer.SetPosition (index++, new Vector3 (transform.position.x + radius * Mathf.Sin (0f),transform.position.y + radius * Mathf.Cos (0f), 0f));
		selectionBoxRenderer.SetVertexCount (index);
	}
	
	public void  AddChildrenToBounds(  ) 
	{
		foreach ( Section s in spaceObject.sections ) {
			float distance = Vector3.Distance(transform.position, s.transform.position) + s.gameObject.collider.bounds.size.magnitude/2f;
			if (distance > radius) radius = distance;
		}
	}

	public void  OnDrawGizmosSelected() 
	{
		Gizmos.DrawWireSphere(transform.position, radius);
	}
	
	void Update()
	{
		
	}
	
	void FixedUpdate()
	{
		if (spaceObject.standing == SpaceObject.Standing.Friendly) if (selected) selectionBoxRenderer.material = spaceObject.listsKeeper.unitCircleSelected;
		if (spaceObject.standing == SpaceObject.Standing.Friendly) if (!selected) selectionBoxRenderer.material = spaceObject.listsKeeper.unitCircleUnselected;
		if (spaceObject.standing == SpaceObject.Standing.Enemy) if (selected) selectionBoxRenderer.material = spaceObject.listsKeeper.enemyCircle;
		if (spaceObject.standing == SpaceObject.Standing.Enemy) if (!selected) selectionBoxRenderer.material = spaceObject.listsKeeper.enemyCircle;
		selectionBoxRenderer.SetWidth (0.2f, 0.2f);
		if (spaceObject.standing == SpaceObject.Standing.Friendly)
		{
			lr.SetPosition(0, transform.position);
			lr.SetPosition(1, spaceObject.movement.moveTarget);
			lr.SetWidth(0.2f,0.2f);
			lr2.SetPosition(0, spaceObject.movement.moveTarget);
			lr2.SetPosition(1, spaceObject.movement.moveTarget+spaceObject.movement.headingTarget*3f);
			lr2.SetWidth(0.3f,0.3f);
		}
	}
	
	public void OnHierarchyDetached(Section destroyedSection)
	{
		enabled = false;
	}
	
}
