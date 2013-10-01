using UnityEngine;
using System.Collections;

public static class Debug2 
{


	public static void DrawLineCross3(Vector3 a, Vector3 b, Color col)
	{
		Debug.DrawLine( new Vector3( a.x,    a.y, a.z ),    new Vector3( b.x,    b.y, b.z ), col);
		Debug2.DrawCross(b, col);
	}
	
	public static void DrawCross(Vector3 b,  Color col)
	{

		Debug.DrawLine( new Vector3( b.x-1f, b.y, b.z ),    new Vector3( b.x+1f, b.y, b.z ), col);
		Debug.DrawLine( new Vector3( b.x,    b.y-1f, b.z ), new Vector3( b.x,    b.y+1f, b.z ), col);
	}
	
	public static void DrawArrow(Vector3 a, Vector3 b, Color col)
	{
		Debug.DrawLine( new Vector3( a.x,    a.y, a.z ),    new Vector3( b.x,    b.y, b.z ), col);
		Vector3 dir = (b - a).normalized;
		
		float distance = Vector3.Distance(a,b);
		Vector3 left = Mathf2.RotateVector2(dir, -0.5f/distance)*(distance-1f);
		Vector3 right = Mathf2.RotateVector2(dir, 0.5f/distance)*(distance-1f);
		
		Debug.DrawLine(a+left,b,col);
		Debug.DrawLine(a+right,b,col);
	}

}

