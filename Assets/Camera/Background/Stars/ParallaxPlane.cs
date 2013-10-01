using UnityEngine;
using System.Collections;

public class ParallaxPlane : MonoBehaviour {

	public GameObject target;
	public float value = 2f;
	public float rotationSpeed = 0.01f;
	public float depth = 45f;
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 p = target.transform.position/value;
		transform.position = new Vector3 (p.x,p.y,depth);
		
		transform.Rotate(Vector3.forward*rotationSpeed);
	}
}
