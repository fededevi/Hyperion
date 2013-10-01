using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class DrawObjectAxes : MonoBehaviour 
{
	public float scale = 1.0f;
	public bool drawX = true;
	public bool drawY = true;
	public bool drawZ = true;
	
	
	void Update () 
	{
		if (Application.isEditor)
		{
			if (drawX) Debug.DrawLine( transform.position - 0.5f * transform.right*scale, transform.position + transform.right*scale, Color.red );
			if (drawY) Debug.DrawLine( transform.position - 0.5f * transform.up*scale, transform.position + transform.up*scale, Color.green );
			if (drawZ) Debug.DrawLine( transform.position - 0.5f * transform.forward*scale, transform.position + transform.forward*scale, Color.blue );	
		}
	}
	
}
