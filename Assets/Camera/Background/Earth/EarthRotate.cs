using UnityEngine;
using System.Collections;

public class EarthRotate : MonoBehaviour 
{

	public float amount = 1f;

	
	void Update () 
	{
		transform.Rotate(transform.forward, amount * Time.deltaTime);
	}
	
}
