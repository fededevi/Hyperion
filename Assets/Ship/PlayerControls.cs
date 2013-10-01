using UnityEngine;
using System.Collections;

public class PlayerControls : MonoBehaviour
{
	public int inputMode = 1;

	public SpaceObject spaceObject;
	public Camera camera;

	public Vector3 startPoint;
	public Vector3 endPoint;

	public Vector3 lastCameraPoint;
	public Vector3 worldMousePosition;


	void Start ()
	{
		spaceObject = (SpaceObject)transform.root.GetComponent ("SpaceObject");
	}

	void Update ()
	{
		if (inputMode == 0) {
			if (Input.GetKeyDown ("w")) {
				spaceObject.movement.movingToTarget = false;
				spaceObject.engine.thrustForwardAmount = 1.0f;
			}
			if (Input.GetKeyDown ("s")) {
				spaceObject.movement.movingToTarget = false;
				spaceObject.engine.thrustForwardAmount = -1.0f;
			}
			if (Input.GetKeyDown ("q")) {
				spaceObject.movement.movingToTarget = false;
				spaceObject.engine.thrustRightAmount = -1.0f;
			}
			if (Input.GetKeyDown ("e")) {
				spaceObject.movement.movingToTarget = false;
				spaceObject.engine.thrustRightAmount = 1.0f;
			}
			
			if (Input.GetKeyUp ("w")) {
				spaceObject.movement.movingToTarget = false;
				spaceObject.engine.thrustForwardAmount = 0.0f;
			}
			if (Input.GetKeyUp ("s")) {
				spaceObject.movement.movingToTarget = false;
				spaceObject.engine.thrustForwardAmount = -0.0f;
			}
			if (Input.GetKeyUp ("q")) {
				spaceObject.movement.movingToTarget = false;
				spaceObject.engine.thrustRightAmount = -0.0f;
			}
			if (Input.GetKeyUp ("e")) {
				spaceObject.movement.movingToTarget = false;
				spaceObject.engine.thrustRightAmount = 0.0f;
			}
			if (Input.GetKeyDown ("a")) {
				spaceObject.movement.headingToTarget = false;
				spaceObject.engine.thrustRotationAmount = -1.0f;
			}
			if (Input.GetKeyDown ("d")) {
				spaceObject.movement.headingToTarget = false;
				spaceObject.engine.thrustRotationAmount = 1.0f;
			}
			
			if (Input.GetKeyUp ("a")) {
				spaceObject.movement.headingToTarget = false;
				spaceObject.engine.thrustRotationAmount = 0.0f;
			}
			if (Input.GetKeyUp ("d")) {
				spaceObject.movement.headingToTarget = false;
				spaceObject.engine.thrustRotationAmount = 0.0f;
			}
			
			//INPUT MOVEMENT SHIP
			if (Input.GetMouseButtonDown (1)) {
				//Debug.Log(camera.ScreenToWorldPoint(Input.mousePosition));
				startPoint = camera.ScreenToWorldPoint (Input.mousePosition);
				startPoint.z = 0f;
				
			}
			if (Input.GetMouseButton (1)) {
				//Debug.Log(camera.ScreenToWorldPoint(Input.mousePosition));
				endPoint = camera.ScreenToWorldPoint (Input.mousePosition);
				endPoint.z = 0f;
				Debug2.DrawArrow (startPoint, endPoint, Color.green);
				Debug2.DrawCross (startPoint, Color.green);
			}
			if (Input.GetMouseButtonUp (1)) {
				//Debug.Log(camera.ScreenToWorldPoint(Input.mousePosition));
				spaceObject.movement.moveTarget = startPoint;
				
				spaceObject.movement.movingToTarget = true;
				spaceObject.movement.headingToTarget = true;
				
				Vector3 dir = (endPoint - startPoint).normalized;
				if (endPoint == startPoint)
					dir = (endPoint - gameObject.transform.position).normalized;
				spaceObject.movement.headingTarget = dir;
			}
		}
		
		if (inputMode == 1) {
			worldMousePosition = camera.ScreenToWorldPoint (Input.mousePosition);
			spaceObject.movement.headingToTarget = true;
			Vector3 dir = (-spaceObject.transform.position + worldMousePosition).normalized;
			spaceObject.movement.headingTarget = dir;
			Vector3 movementDirection = Vector3.zero;
			if (Input.GetKey ("up")) {
				spaceObject.movement.movingToTarget = false;
				movementDirection += Vector3.up;
			}
			if (Input.GetKey ("down")) {
				spaceObject.movement.movingToTarget = false;
				movementDirection -= Vector3.up;
			}
			if (Input.GetKey ("right")) {
				spaceObject.movement.movingToTarget = false;
				movementDirection += Vector3.right;
			}
			if (Input.GetKey ("left")) {
				spaceObject.movement.movingToTarget = false;
				movementDirection -= Vector3.right;
			}
			
			if (movementDirection.magnitude > 0.5f)
			{
				spaceObject.engine.setThrustInWorldDirection(movementDirection, 1f);
			} else
			{
				spaceObject.engine.setThrustInWorldDirection(movementDirection, 0f);
			}
			
		}
		
		
		if (Input.GetKeyUp ("g")) 
		{
			spaceObject.ship.godMode = !spaceObject.ship.godMode;
		}
		
		
		if (Input.GetMouseButtonDown (2)) {
			RaycastHit hitPoint;
			GameObject target;
			if (Physics.Raycast (camera.ScreenToWorldPoint (Input.mousePosition), transform.forward, out hitPoint, 1000f)) 
			{
				target = hitPoint.collider.gameObject;
			
				foreach (Hardpoint hp in spaceObject.hardpoints) 
				{
					hp.primaryTarget = target;
				}
			}
		}
		
		if (Input.GetMouseButtonDown (0)) {
			
			foreach (Hardpoint hp in spaceObject.hardpoints) {
				if (!hp.automaticAiming) {
					hp.turretScript.firing = true;
				}
			}
		}
		
		if (Input.GetMouseButtonUp (0)) {
			
			foreach (Hardpoint hp in spaceObject.hardpoints) {
				if (!hp.automaticAiming) {
					hp.turretScript.firing = false;
				}
			}
		}
		
		
	}
}

