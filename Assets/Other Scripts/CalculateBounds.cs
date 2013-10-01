using UnityEngine;
using System.Collections;

public class CalculateBounds : MonoBehaviour 
{
	private float radius = 1f;
	private SpaceObject spaceObject;

	public void Start() 
	{
		spaceObject =  GetComponent<SpaceObject>();
		AddChildrenToBounds(  );
	}

	public void  AddChildrenToBounds(  ) 
	{
		foreach ( Section s in spaceObject.sections ) {
			float distance = Vector3.Distance(transform.position, s.transform.position) + s.gameObject.collider.bounds.size.magnitude;
			if (distance > radius) radius = distance;
		}
	}

	public void  OnDrawGizmosSelected() 
	{
		Gizmos.DrawSphere(transform.position, radius);
	}
}