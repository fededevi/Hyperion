using UnityEngine;
using System.Collections;

public class Hardpoint : MonoBehaviour
{
	public bool automaticAiming = true;
	public SpaceObject spaceObject;
	public GameObject turret;
	public Turret turretScript;
	public GameObject primaryTarget;
	public GameObject currentTarget;
	public bool turretCanFire = false;

	public float arc = Mathf.PI;
	public bool aimOnlyAtPrimaryTarget = false;

	//STATS
	public int size = 10;
	private float speed = 0.5f;

	//
	float lookForTargetTimer = 0f;
	public LineRenderer lr;

	void Start ()
	{
		spaceObject = (SpaceObject)transform.root.GetComponent ("SpaceObject");
		foreach (Transform child in transform)
			turret = child.gameObject;
		turretScript = (Turret)turret.GetComponent ("Turret");
		RegisterWithShip ();
		
		if (arc > Mathf.PI * 2f)
			arc = Mathf.PI * 2f;
		
		
		lr = gameObject.AddComponent<LineRenderer> ();
		lr.material = spaceObject.listsKeeper.weaponLineMaterial;
	}

	public void OnHierarchyDetached (Section destroyedSection)
	{
		Destroy(lr);
		enabled = false;
		
	}

	void RegisterWithShip ()
	{
		spaceObject.hardpoints.Add (this);
	}

	bool isInsideArcPosition (Vector3 position)
	{
		Vector3 dir = (position - transform.position).normalized;
		float angle = Mathf2.AngleBetweenVectors (transform.up, dir);
		float distance = Vector3.Distance (position, transform.position);
		if (Mathf.Abs (angle) < arc / 2f && distance < turretScript.weapon.GetRange ())
			return true;
		return false;
	}

	void Update ()
	{
		//if (spaceObject == null) this.enabled = false;
		
		lookForTargetTimer += Time.fixedDeltaTime;
		if (automaticAiming && turretScript.weapon != null) {
			if (aimOnlyAtPrimaryTarget && primaryTarget == null)
				return;
			
			bool primaryTargetIsInsideArc = false;
			
			if (primaryTarget != null)
				primaryTargetIsInsideArc = isInsideArcPosition (primaryTarget.transform.position);
			
			if (aimOnlyAtPrimaryTarget && !primaryTargetIsInsideArc)
				return;
			
			if (primaryTargetIsInsideArc) {
				currentTarget = primaryTarget;
			} else {
				if (currentTarget == null) {
					FindTargetInsideArc (turretScript.weapon.GetRange ());
				} else {
					if (!isInsideArcPosition (currentTarget.transform.position)) {
						FindTargetInsideArc (turretScript.weapon.GetRange ());
					}
				}
			}
			
			if (currentTarget != null) {
				if (spaceObject.standing == SpaceObject.Standing.Friendly) {
					lr.enabled = true;
					lr.SetPosition(0, transform.position);
					lr.SetPosition(1, currentTarget.transform.position);
					lr.SetWidth(0.1f,0.1f);
				}
				Vector3 leadPosition;
				if (currentTarget.transform.root.rigidbody != null)
				{
				leadPosition = FirstOrderIntercept (transform.position, Vector3.zero, turretScript.weapon.GetProjectileSpeed(),
															currentTarget.transform.position, currentTarget.transform.root.rigidbody.velocity);
				} else leadPosition = currentTarget.transform.position;
				
				TurnTorwardsPosition (leadPosition);
				
				//IS THE TURRET INSIDE FIRING ARC
				turretCanFire = isInsideArcPosition (transform.position + turret.transform.up);
				
				float angle = Mathf.Abs (Mathf2.AngleBetweenVectors (turret.transform.up, (leadPosition - turret.transform.position).normalized));
				if (angle < turretScript.weapon.GetAllowedAimingError () && isInsideArcPosition (leadPosition)) {
					turretScript.firing = turretCanFire;
				} else
					turretScript.firing = false;
			} else {
				lr.enabled = false;
				turretScript.firing = false;
			}
		}
		
	}

	public void FindTargetInsideArc (float maxDistance)
	{
		if (lookForTargetTimer < 0.5f)
			return;
		lookForTargetTimer = Random.value * 0.1f;
		float lastDistance = maxDistance;
		foreach (SpaceObject target in spaceObject.listsKeeper.spaceObjects) {
			if (target != spaceObject) {
				float dis = Vector3.Distance (transform.position, target.transform.position);
				if (spaceObject.standing == SpaceObject.Standing.Enemy) {
					if (target.standing != SpaceObject.Standing.Enemy && target.standing != SpaceObject.Standing.Neutral) {
						if (isInsideArcPosition (target.transform.position) && dis < lastDistance) {
							lastDistance = dis;
							currentTarget = target.gameObject;
						}
					}
				} else if (spaceObject.standing == SpaceObject.Standing.Neutral) {
				} else if (spaceObject.standing == SpaceObject.Standing.Friendly) {
					if (target.standing == SpaceObject.Standing.Enemy) {
						if (isInsideArcPosition (target.transform.position) && dis < lastDistance) {
							lastDistance = dis;
							currentTarget = target.gameObject;
						}
					}
				} else if (spaceObject.standing == SpaceObject.Standing.Player) {
					if (target.standing == SpaceObject.Standing.Enemy) {
						
						if (isInsideArcPosition (target.transform.position) && dis < lastDistance) {
							lastDistance = dis;
							currentTarget = target.gameObject;
						}
					}
				}
			}
		}
	}

	void TurnToStandby ()
	{
		TurnTorwardsDirection (transform.up);
	}

	void TurnTorwardsPosition (Vector3 position)
	{
		TurnTorwardsDirection ((position - transform.position).normalized);
	}

	void TurnTorwardsDirection (Vector3 direction)
	{
		float angle = Mathf2.AngleBetweenVectors (turret.transform.up, direction);
		
		float sign = angle / Mathf.Abs (angle);
		float amount = Time.deltaTime * speed;
		float absoluteAngle = Mathf.Abs (angle);
		if (absoluteAngle < 0.001)
			return;
		if (amount > absoluteAngle)
			amount = absoluteAngle;
		turret.transform.Rotate (new Vector3 (0f, 0f, sign * amount * (180f / Mathf.PI)));
	}

	void DrawHardpointArc ()
	{
		Vector3 left = Mathf2.RotateVector2 (gameObject.transform.up, -arc / 2f);
		Vector3 right = Mathf2.RotateVector2 (gameObject.transform.up, arc / 2f);
		
		Vector3 slerpo;
		Vector3 srerpo;
		
		Debug.DrawLine (gameObject.transform.position, gameObject.transform.position + left * 1f);
		Debug.DrawLine (gameObject.transform.position, gameObject.transform.position + right * 1f);
		
		for (int i = 0; i < 35; i++) {
			slerpo = Mathf2.RotateVector2 (left, arc * (float)i / 35f);
			srerpo = Mathf2.RotateVector2 (left, arc * (float)(i + 1) / 35f);
			Debug.DrawLine (gameObject.transform.position + 1f * slerpo, gameObject.transform.position + 1f * srerpo);
		}
	}

	public static Vector3 FirstOrderIntercept (Vector3 shooterPosition, Vector3 shooterVelocity, float shotSpeed, Vector3 targetPosition, Vector3 targetVelocity)
	{
		Vector3 targetRelativeVelocity = targetVelocity - shooterVelocity;
		float t = FirstOrderInterceptTime (shotSpeed, targetPosition - shooterPosition, targetRelativeVelocity);
		return targetPosition + t * (targetRelativeVelocity);
	}
	//first-order intercept using relative target position
	public static float FirstOrderInterceptTime (float shotSpeed, Vector3 targetRelativePosition, Vector3 targetRelativeVelocity)
	{
		float velocitySquared = targetRelativeVelocity.sqrMagnitude;
		if (velocitySquared < 0.001f)
			return 0f;
		
		float a = velocitySquared - shotSpeed * shotSpeed;
		
		//handle similar velocities
		if (Mathf.Abs (a) < 0.001f) {
			float t = -targetRelativePosition.sqrMagnitude / (2f * Vector3.Dot (targetRelativeVelocity, targetRelativePosition));
			return Mathf.Max (t, 0f);
			//don't shoot back in time
		}
		
		float b = 2f * Vector3.Dot (targetRelativeVelocity, targetRelativePosition), c = targetRelativePosition.sqrMagnitude, determinant = b * b - 4f * a * c;
		
		if (determinant > 0f) {
			//determinant > 0; two intercept paths (most common)
			float t1 = (-b + Mathf.Sqrt (determinant)) / (2f * a), t2 = (-b - Mathf.Sqrt (determinant)) / (2f * a);
			if (t1 > 0f) {
				if (t2 > 0f)
					return Mathf.Min (t1, t2);
				else
					//both are positive
					return t1;
				//only t1 is positive
			} else
				return Mathf.Max (t2, 0f);
			//don't shoot back in time
		} else if (determinant < 0f)
			//determinant < 0; no intercept path
			return 0f;
		else
			//determinant = 0; one intercept path, pretty much never happens
			return Mathf.Max (-b / (2f * a), 0f);
		//don't shoot back in time
	}
	
}

