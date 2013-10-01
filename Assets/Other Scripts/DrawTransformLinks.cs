using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class DrawTransformLinks : MonoBehaviour
{
	void Update () 
	{
		DrawLinks(transform);
	}
	
	public void DrawLinks(Transform t)
	{
		if (t.childCount > 0)
		{
			foreach (Transform c in t)
			{
				Debug.DrawLine(t.position, c.position, Color.red);
				DrawLinks(c);
			}
		}
	}
}
