using UnityEngine;
using System.Collections;

public static class Mathf2
{

	public static float AngleBetweenVectors(Vector2 a, Vector2 b)
	{
		float cross = a.x *  b.y - a.y *  b.x;
		float dot = a.x *  b.x + a.y *  b.y;
		return Mathf.Atan2(cross, dot); 
		//ANGOLO DA -PI a  + PI TRA 2 VETTORI
	}
	
	public static Vector2 RotateVector2(Vector2 vector, float angle)
	{
		return new Vector2(Mathf.Cos(angle) * vector.x - Mathf.Sin(angle) * vector.y, Mathf.Sin(angle) * vector.x + Mathf.Cos(angle) * vector.y);
	}
}

