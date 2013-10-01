using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class DrawArcOfHardpoint : MonoBehaviour {
	
	Hardpoint hardpoint;
	static public bool activated = true;

	void Start () 
	{
		hardpoint = (Hardpoint)gameObject.GetComponent("Hardpoint");
	}
	
	
	void Update () 
	{
		if (activated)
		{
		Vector3 left = Mathf2.RotateVector2(gameObject.transform.up, -hardpoint.arc/2f);
		Vector3 right = Mathf2.RotateVector2(gameObject.transform.up, hardpoint.arc/2f);
		
		Vector3 slerpo;
		Vector3 srerpo;
		
		
		Debug.DrawLine(gameObject.transform.position, gameObject.transform.position+left*1f);
		Debug.DrawLine(gameObject.transform.position, gameObject.transform.position+right*1f);
		
		for (int i = 0; i < 35; i++)
		{
			slerpo = Mathf2.RotateVector2(left , hardpoint.arc * (float)i / 35f);
			srerpo = Mathf2.RotateVector2(left , hardpoint.arc * (float)(i+1) / 35f);
			Debug.DrawLine(gameObject.transform.position+1f*slerpo,gameObject.transform.position+1f*srerpo);
		}		
		}
	}
}
