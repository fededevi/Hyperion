using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour 
{

	public SpaceObject spaceObject;
	
	//IF THIS IS ENABLED THE SCRIPT TAKES CURRENT VELOCITY TO CORRECT OBJECT HEADING AND ACHIEVE BETTER INTERCEPT COURSE
	public bool headingCorrection = true; 
	public bool accellerationCorrection = true; 
	public float headingCorrectionValue = 0.45f; //0.45 seems to yeld better results
	
	
	//public GameObject se;
	public bool movingToTarget = true;
	public GameObject moveTargetObject;
	public GameObject headingTargetObject;
	public Vector2 moveTarget;
	
	public bool headingToTarget = true;
	public float distanceToHeadingTarget;
	public Vector2 headingTarget = new Vector2(0f,1f);
	
	public float distanceToMovingTarget;
		
	void Awake()
	{
		spaceObject = (SpaceObject) transform.root.GetComponent<SpaceObject>();
		spaceObject.movement = this;
	}
	
	void Start() 
	{
		
	}
	
	public void OnHierarchyDetached(Section destroyedSection)
	{
		enabled = false;
		Debug.Log("Movement Disabled");
	}
	
	void FixedUpdate()
	{
		if (spaceObject.engine == null) this.enabled = false;
		
		if (moveTargetObject != null)
		{
			moveTarget = moveTargetObject.transform.position;
		}
		if ( headingTargetObject != null )
		{
			Vector3 dir = ((Vector3)headingTargetObject.transform.position - gameObject.transform.position).normalized;
			headingTarget = dir;
		}
		
		//MOVING TARGET PID
		distanceToMovingTarget = Vector2.Distance (spaceObject.position, moveTarget); 
		
		if (movingToTarget)
		{
			Vector2 directionToTarget = (moveTarget - spaceObject.position).normalized;
			Vector2 normVel = spaceObject.velocity.normalized;
			
			if (headingCorrection && spaceObject.velocity.magnitude > 0.1f) 
			{
				float val = headingCorrectionValue;
				float nval = 1f - val;
				if (Vector2.Dot(directionToTarget, normVel) < 0.9999f)//IF WE ARE NOT MOVING IN THE RIGHT DIRECTION TOWARD OUR TARGET
				{
					//CORRECT HEADING
					directionToTarget = (directionToTarget * nval) - (normVel * val); 
				}
			}
			
			Vector2 unmodifiedDirectionToTarget = (moveTarget - spaceObject.position).normalized;
			if (accellerationCorrection)
			{
				if (Vector2.Dot(unmodifiedDirectionToTarget, normVel) > 0.80f) //IF WE ARE MOVING IN THE RIGHT DIRECTION TOWARD OUR TARGET
				{
					float deceleration = spaceObject.engine.getThrustInLocalDirection(-normVel) / rigidbody.mass;
					float velocityFactor = (0.5f * spaceObject.velocity.magnitude *spaceObject.velocity.magnitude);
					velocityFactor = velocityFactor / deceleration;
					 
					if (distanceToMovingTarget < velocityFactor) // IF WE ARE NEAR
					{
						directionToTarget =  -normVel; //Decelerate
					}
				}
			}
			
			directionToTarget.Normalize();
			float thrustAmount = 1f;
			spaceObject.engine.setThrustInWorldDirection( directionToTarget, thrustAmount );	
			
			if ( (distanceToMovingTarget < 0.20f) && (spaceObject.velocity.magnitude < 0.1f) && (Vector2.Dot(directionToTarget, normVel) > 0.80f))
			{
				if (spaceObject.velocity.magnitude < 0.2f) 
				{
					spaceObject.engine.StopMovementThrust();
					movingToTarget = false;
				} else
				{
					float amount = spaceObject.engine.getThrustInLocalDirection(-normVel);
					spaceObject.engine.setThrustInWorldDirection( -normVel, (Time.fixedDeltaTime*rigidbody.mass/amount) /10f );
				}
			}			
		}
		
		distanceToHeadingTarget = Mathf2.AngleBetweenVectors(headingTarget, spaceObject.heading);
		
		if (headingToTarget)
		{
			headingTarget.Normalize();
			spaceObject.engine.setRotationThrust( distanceToHeadingTarget + gameObject.rigidbody.angularVelocity.z/2f);
		}
		
	}
	
}
