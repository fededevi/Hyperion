using UnityEngine;
using System.Collections.Generic;


public class RTSInterface : MonoBehaviour
{
	public Camera cam;
	GameObject target;
	GameObject targetLeft;
	GameObject targetRight;
	public GameObject baryCenter;

	public List<Ship> selection;
	public ListsKeeper listsKeeper;
	Vector2 lastDownSelect;
	Vector2 lastUpSelect;

	Vector3 lastDownMove;
	Vector3 lastUpMove;

	GameObject selectionBoxObject;
	GameObject selectionBoxInsideObject;
	LineRenderer selectionBox;
	LineRenderer selectionBoxInside;

	void Start ()
	{
		selectionBoxObject = new GameObject ();
		selectionBox = selectionBoxObject.AddComponent<LineRenderer> ();
		selectionBox.renderer.material = listsKeeper.selectionBoxLineMaterial;
		
		selectionBoxInsideObject = new GameObject ();
		selectionBox = selectionBoxInsideObject.AddComponent<LineRenderer> ();
		selectionBox.renderer.material = listsKeeper.selectionBoxInsideLineMaterial;
		
		selection = new List<Ship> ();
		target = new GameObject ();
		targetLeft = new GameObject ();
		targetRight = new GameObject ();
		
		//baryCenter = new GameObject ();
		//baryCenter.name = "Selection Barycenter";
	}

	void Update ()
	{
		foreach (Ship s in selection) {
			if (s == null)
				selection.Remove (s);
		}
		
		baryCenter.transform.position = Vector3.zero;
		Vector3 pos = Vector3.zero;
		if (selection.Count > 0) {
			foreach (Ship s in selection) {
				pos = pos + s.gameObject.transform.position;
			}
			baryCenter.transform.position = pos / selection.Count;
		}
		
		MoveCheck ();
		AttackCheck ();
		SelectCheck ();
	}

	void MoveCheck ()
	{
		Vector3 currentPos = camera.ScreenToWorldPoint (Input.mousePosition);
		currentPos.z = 0f;
		
		if (Input.GetMouseButtonDown (1)) {
			lastDownMove = camera.ScreenToWorldPoint (Input.mousePosition);
			lastDownMove.z = 0f;
			
		}
		if (Input.GetMouseButton (1)) {
			Debug2.DrawArrow (lastDownMove, currentPos, Color.green);
			Debug2.DrawCross (lastDownMove, Color.green);
		}
		if (Input.GetMouseButtonUp (1)) {
			lastUpMove = camera.ScreenToWorldPoint (Input.mousePosition);
			lastUpMove.z = 0f;
			
			if (Vector2.Distance (lastUpMove, lastDownMove) > 1f) {
				MoveAtFormation ();
			} else {
				
				foreach (Ship s in selection) {
					s.spaceObject.movement.movingToTarget = true;
					s.spaceObject.movement.headingToTarget = true;
					s.spaceObject.movement.moveTargetObject = null;
					s.spaceObject.movement.headingTargetObject = null;
					Vector3 pos = lastDownMove - baryCenter.transform.position;
					s.spaceObject.movement.moveTarget = s.transform.position + pos;
				}
			}
		}
	}

	void MoveAtFormation ()
	{
		float r = 0f;
		foreach (Ship s in selection) if (s.radius > r) r = s.radius;
		float numberOfShipsPerLine = 5f;
		float formationWidth = 15f;
		formationWidth = Mathf.Max (numberOfShipsPerLine * r*2f, formationWidth);
		
		Vector3 dir = (lastUpMove - lastDownMove).normalized;
		Vector3 dir2 = (baryCenter.transform.position - lastDownMove).normalized;
		target.transform.position = lastDownMove;
		float angle = Mathf2.AngleBetweenVectors (target.transform.up, dir2);
		target.transform.Rotate (new Vector3 (0f, 0f, angle * (180f / Mathf.PI)));
		//target.transform.LookAt(Vector3.forward ,Vector3.right);
		targetLeft.transform.position = target.transform.position;
		targetLeft.transform.rotation = target.transform.rotation;
		targetRight.transform.position = target.transform.position;
		targetRight.transform.rotation = target.transform.rotation;
		
		targetLeft.transform.position += targetLeft.transform.right * (formationWidth / 2f);
		targetRight.transform.position -= targetRight.transform.right * (formationWidth / 2f);
		Vector3 behind = targetRight.transform.up;
		
		
		
		float count = 0f;
		
		float verticalCount = 0f;
		
		float lerp;
		int sign = 1;
		int steps = 0;
		foreach (Ship s in selection) {
			lerp = 0.5f + (float)sign * (float)steps * 1f / numberOfShipsPerLine;
			s.spaceObject.movement.moveTarget = Vector3.Lerp (targetLeft.transform.position, targetRight.transform.position, lerp);
			
			s.spaceObject.movement.moveTarget += (verticalCount * (Vector2)behind * r*2f);
			
			count += 1f / numberOfShipsPerLine;
			if (count > 0.98) {
				verticalCount = verticalCount + 1f;
				count = 0f;
				sign = -1;
				steps = 0;
			}
			
			sign = -sign;
			if (sign < 0)
				steps++;
		}
		
		foreach (Ship s in selection) {
			
			s.spaceObject.movement.movingToTarget = true;
			s.spaceObject.movement.headingToTarget = true;
			
			s.spaceObject.movement.moveTargetObject = null;
			s.spaceObject.movement.headingTargetObject = null;
			
			s.spaceObject.movement.headingTarget = dir;
		}
	}

	void SelectCheck ()
	{
		if (Input.GetMouseButtonDown (0)) {
			lastDownSelect = Input.mousePosition;
			selectionBox.enabled = true;
		}
		
		if (Input.GetMouseButton (0)) {
			Vector2 a = camera.ScreenToWorldPoint (lastDownSelect);
			Vector2 b = camera.ScreenToWorldPoint (Input.mousePosition);
			Vector2 c = new Vector2 (Mathf.Min (a.x, b.x), Mathf.Min (a.y, b.y));
			Vector2 d = new Vector2 (Mathf.Max (a.x, b.x), Mathf.Max (a.y, b.y));
			DrawSelectionBox (c, d);
		}
		
		
		if (Input.GetMouseButtonUp (0)) {
			foreach (Ship s in selection) s.selected = false;
			selectionBox.enabled = false;
			lastUpSelect = Input.mousePosition;
			if (Vector2.Distance (lastDownSelect, lastUpSelect) < 1) {
				RaycastHit hitPoint;
				selection = new List<Ship> ();
				if (Physics.Raycast (camera.ScreenToWorldPoint (Input.mousePosition), transform.forward, out hitPoint, 1000f)) {
					
					selection.Add (hitPoint.collider.gameObject.transform.root.GetComponent<Ship> ());
				}
			} else {
				Select ();
			}
			foreach (Ship s in selection) s.selected = true;
		}
	}

	public void DrawSelectionBox (Vector2 a, Vector2 b)
	{
		selectionBox.SetVertexCount (100);
		int indexcount = 0;
		float radius = 0.2f;
		for (float i = 0f; i < Mathf.PI / 2f; i = i + (Mathf.PI / 2f) / 20f) {
			selectionBox.SetPosition (indexcount, new Vector3 (a.x - radius * Mathf.Cos (i), a.y - radius * Mathf.Sin (i), 0f));
			indexcount++;
		}
		for (float i = 0f; i < Mathf.PI / 2f; i = i + (Mathf.PI / 2f) / 20f) {
			selectionBox.SetPosition (indexcount, new Vector3 (b.x + radius * Mathf.Sin (i), a.y - radius * Mathf.Cos (i), 0f));
			indexcount++;
		}
		for (float i = 0f; i < Mathf.PI / 2f; i = i + (Mathf.PI / 2f) / 20f) {
			selectionBox.SetPosition (indexcount, new Vector3 (b.x + radius * Mathf.Cos (i), b.y + radius * Mathf.Sin (i), 0f));
			indexcount++;
		}
		for (float i = 0f; i < Mathf.PI / 2f; i = i + (Mathf.PI / 2f) / 20f) {
			selectionBox.SetPosition (indexcount, new Vector3 (a.x - radius * Mathf.Sin (i), b.y + radius * Mathf.Cos (i), 0f));
			indexcount++;
		}
		selectionBox.SetPosition (indexcount++, new Vector3 (a.x - radius * Mathf.Sin (Mathf.PI / 2f), b.y + radius * Mathf.Cos (Mathf.PI / 2f), 0f));
		selectionBox.SetPosition (indexcount++, new Vector3 (a.x - radius * Mathf.Cos (0f), a.y - radius * Mathf.Sin (0f), 0f));
		//electionBox.SetPosition(8,new Vector3(a.x+0.5f,a.y+0.5f,0f));
		selectionBox.SetWidth (0.005f * listsKeeper.mainCamera.orthographicSize, 0.005f * listsKeeper.mainCamera.orthographicSize);
		selectionBox.SetVertexCount (indexcount);
	}

	public void Select ()
	{
		selection = new List<Ship> ();
		//Debug.Log ("select");
		float minX = Mathf.Min (lastDownSelect.x, lastUpSelect.x);
		float maxX = Mathf.Max (lastDownSelect.x, lastUpSelect.x);
		float minY = Mathf.Min (lastDownSelect.y, lastUpSelect.y);
		float maxY = Mathf.Max (lastDownSelect.y, lastUpSelect.y);
		Vector4 rect = new Vector4 (minX, minY, maxX, maxY);
		foreach (SpaceObject s in listsKeeper.spaceObjects) {
			//Debug.Log ("checking Ship..");
			if (IsSpaceObjectFriendly (s)) {
				//Debug.Log ("Ship is friendly..");
				Vector2 spaceObjScreenPos = cam.WorldToScreenPoint (s.transform.position);
				
				if (IsInsideRect (spaceObjScreenPos, rect)) {
					//Debug.Log ("Ship is Inside..");
					if (s.ship != null) {
						selection.Add (s.ship);
					}
					
					//Debug.Log ("Ship added..");
				}
				
			}
		}
	}

	public void AttackCheck ()
	{
		if (Input.GetMouseButtonDown (2)) {
			RaycastHit hitPoint;
			GameObject target;
			if (Physics.Raycast (camera.ScreenToWorldPoint (Input.mousePosition), transform.forward, out hitPoint, 1000f)) {
				target = hitPoint.collider.gameObject;
				
				foreach (Ship s in selection) {
					foreach (Hardpoint hp in s.spaceObject.hardpoints) {
						hp.primaryTarget = target;
					}
				}
			}
		}
	}

	Ship GetShipAtCoordinate (Vector2 c)
	{
		return null;
	}

	bool IsSpaceObjectFriendly (SpaceObject s)
	{
		return (s.standing == SpaceObject.Standing.Friendly);
	}

	bool IsInsideRect (Vector2 p, Vector4 r)
	{
		return (p.x > r.x) && (p.x < r.z) && (p.y > r.y) && (p.y < r.w);
	}
	
}
