using UnityEngine;
using System.Collections;

public class SubCameraScript : MonoBehaviour 
{
	
	public Camera target;
	public Camera cam;
	

	public void OnPreCull () 
	{
		
		cam.orthographicSize = target.orthographicSize;
	}
	
}
