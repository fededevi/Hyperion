using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour
{

	private GameObject target;
	public Camera[] cameras;
	public Camera thisCamera;
	public float size;

	public float minZoom = 10f;
	public float maxZoom = 150f;
	public float maxDistance = 200f;

	Vector3 lastCameraPoint;


	void Start ()
	{
		thisCamera = GetComponent<Camera> ();
		size = thisCamera.orthographicSize;
		target = new GameObject ();
		target.name = "CameraTarget";
		target.transform.position = transform.position;
	}

	void Update ()
	{
		if (Input.GetKey ("up"))
			target.transform.Translate (Vector3.up * 100f * Time.deltaTime);
		if (Input.GetKey ("down"))
			target.transform.Translate (-Vector3.up * 100f * Time.deltaTime);
		if (Input.GetKey ("right"))
			target.transform.Translate (Vector3.right * 100f * Time.deltaTime);
		if (Input.GetKey ("left"))
			target.transform.Translate (-Vector3.right * 100f * Time.deltaTime);
		
		//CAMERA MOVEMENT AND ZOOM
		if (Input.GetMouseButtonDown (2)) {
			lastCameraPoint = Input.mousePosition;
			lastCameraPoint.z = 0f;
		}
		if (Input.GetMouseButton (2)) {
			Vector3 newPoint = Input.mousePosition;
			newPoint.z = 0f;
			Vector3 againPoint = camera.ScreenToWorldPoint (newPoint) - camera.ScreenToWorldPoint (lastCameraPoint);
			againPoint.z = 0f;
			lastCameraPoint = newPoint;
			target.transform.Translate (-againPoint);
			
		}
		if (Input.GetMouseButtonUp (2)) {
			
		}
		Vector2 pos2D = target.transform.position;
		if (Vector2.Distance (pos2D, Vector2.zero) > maxDistance) {
			target.transform.position = pos2D.normalized * maxDistance;
		}
		
	}

	void LateUpdate()
	{
		
		
		
		size -= (Input.GetAxis ("Mouse ScrollWheel") * 1f) * size;
		
		size = Mathf.Max (size, minZoom);
		size = Mathf.Min (size, maxZoom);
		float a = thisCamera.orthographicSize;
		thisCamera.orthographicSize = a * 0.95f + size * 0.05f;
		
		
		if (target != null) {
			gameObject.transform.position = gameObject.transform.position * 0.90f + target.transform.position * 0.1f;
			gameObject.transform.position = new Vector3 (gameObject.transform.position.x, gameObject.transform.position.y, -500f);
		}
		foreach (Camera cam in cameras) {
			cam.orthographicSize = thisCamera.orthographicSize;
			cam.orthographicSize = thisCamera.orthographicSize;
		}
	}
	
}








