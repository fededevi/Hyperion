using UnityEngine;
using System.Collections;


	
public class Engine : MonoBehaviour
{
	
	public SpaceObject spaceObject;
	public Rigidbody rigidbody;
	
	public float thrustForward = 10f;
	public float thrustBackward = 1f;
	public float thrustRight = 4f;
	public float thrustLeft = 2f;
		
	public float thrustForwardAmount = 0f;
	public float thrustRightAmount = 0f;
	
	public float thrustCW = 1f;
	public float thrustCCW = 1f;
	public float thrustRotationAmount = 0.0f;	

	void Start ()
	{
		spaceObject = (SpaceObject)transform.root.GetComponent("SpaceObject");
		spaceObject.engine = this;
		rigidbody = (Rigidbody)spaceObject.GetComponent("Rigidbody");
	}

	public void OnHierarchyDetached(Section destroyedSection)
	{
		enabled = false;
	}
	
	void FixedUpdate ()
	{
		float forceForward = thrustForward * thrustForwardAmount;
		float forceBackward = thrustBackward * -thrustForwardAmount;
		float forceRight = thrustRight * thrustRightAmount;
		float forceLeft = thrustLeft * -thrustRightAmount;
		
		Vector3 accVector = Vector3.zero;
		if (forceForward > 0) accVector += gameObject.transform.up*forceForward;
		if (forceBackward > 0) accVector -= gameObject.transform.up*forceBackward;
		if (forceRight > 0) accVector += gameObject.transform.right*forceRight;
		if (forceLeft > 0) accVector -= gameObject.transform.right*forceLeft;
		
		rigidbody.AddForce(accVector);
		
		/*if ( Application.isEditor && spaceObject.debug )
		{
			Debug2.DrawArrow(gameObject.transform.position, gameObject.transform.position + (accVector/rigidbody.mass)*3f,Color.white);
			if (forceForward > 0) Debug2.DrawArrow(gameObject.transform.position, gameObject.transform.position + gameObject.transform.up*(forceForward/rigidbody.mass)*5f, Color.yellow);
			if (forceBackward > 0) Debug2.DrawArrow(gameObject.transform.position, gameObject.transform.position - gameObject.transform.up*(forceBackward/rigidbody.mass)*5f, Color.yellow);
			if (forceRight > 0) Debug2.DrawArrow(gameObject.transform.position, gameObject.transform.position + gameObject.transform.right*(forceRight/rigidbody.mass)*5f, Color.yellow);
			if (forceLeft > 0) Debug2.DrawArrow(gameObject.transform.position, gameObject.transform.position - gameObject.transform.right*(forceLeft/rigidbody.mass)*5f, Color.yellow);
		}*/
		
		if (thrustRotationAmount > 1f) thrustRotationAmount = 1f;
		if (thrustRotationAmount < -1f) thrustRotationAmount = -1f;
		if ( thrustRotationAmount > 0f ) rigidbody.AddTorque( -Vector3.forward * thrustCW * thrustRotationAmount/rigidbody.mass, ForceMode.Acceleration);
		if ( thrustRotationAmount < 0f ) rigidbody.AddTorque( -Vector3.forward * thrustCCW * thrustRotationAmount/rigidbody.mass, ForceMode.Acceleration);
	}
	
	void setThrustersAmount(float forwardAmount, float rightAmount)
	{
		thrustForwardAmount = forwardAmount;
		thrustRightAmount = rightAmount;
	}
	
	public void setThrustInLocalDirection(Vector2 direction, float amount)
	{
		direction.Normalize();
		
		if (amount > 1f) amount = 1f;
		if (amount <= 0f) amount = 0f;
	    if (direction.x >= 0)
		{
			if (direction.y >=0)
			{
				//CALCOLO IL RAPPORTO PER CAPIRE QUALE THRUSTER VA AL 100%
				float forwardNeed = direction.y / thrustForward;
				float rightNeed = direction.x / thrustRight;
				float max = Mathf.Max(forwardNeed,rightNeed);
				//SETTO I THRUSTER 1 A 100% l'ALTRO AL VALORE NECESSARIO PER OTTERENRE L'HEADING DESIDERATO
				setThrustersAmount((amount*forwardNeed)/max, (amount*rightNeed)/max);
				//BY DIVITDING BY MAX WE SET THE THRUSTERS TO NO MORE THAN 1f
				//Debug.Log("++");
			} else
			{
				float forwardNeed = -direction.y / thrustBackward; //THIS NEEDS TO BE POSOTIVE TO CALCULATE MAX VALUE BETWEEN THE 2
				float rightNeed = direction.x / thrustRight;
				float max = Mathf.Max(forwardNeed,rightNeed);
				setThrustersAmount((-amount*forwardNeed)/max, (amount*rightNeed)/max);
				//Debug.Log("+-");
			}
		} else
		{
			if (direction.y >=0)
			{
				float forwardNeed = direction.y / thrustForward;
				float rightNeed = -direction.x / thrustLeft;
				float max = Mathf.Max(forwardNeed,rightNeed);
				setThrustersAmount((amount*forwardNeed)/max, (-amount*rightNeed)/max);
				//Debug.Log("-+");
			} else
			{
				float forwardNeed = -direction.y / thrustBackward;
				float rightNeed = -direction.x / thrustLeft;
				float max = Mathf.Max(forwardNeed,rightNeed);
				setThrustersAmount((-amount*forwardNeed)/max, (-amount*rightNeed)/max);
				//Debug.Log("--");
			}
		}
	}
	
	public float getThrustInLocalDirection(Vector2 directions)
	{
		Vector2 direction = Quaternion.Inverse(gameObject.transform.rotation) * directions;
		
	    if (direction.x >= 0)
		{
			if (direction.y >=0)
			{
				//CALCOLO IL RAPPORTO PER CAPIRE QUALE THRUSTER VA AL 100%
				float forwardNeed = direction.y / thrustForward;
				float rightNeed = direction.x / thrustRight;
				float max = Mathf.Max(forwardNeed,rightNeed);
				//SETTO I THRUSTER 1 A 100% l'ALTRO AL VALORE NECESSARIO PER OTTERENRE L'HEADING DESIDERATO
				return new Vector2(thrustForward*(forwardNeed)/max, thrustRight*(rightNeed)/max).magnitude;
				//BY DIVITDING BY MAX WE SET THE THRUSTERS TO NO MORE THAN 1f
			} else
			{
				float forwardNeed = -direction.y / thrustBackward; //THIS NEEDS TO BE POSOTIVE TO CALCULATE MAX VALUE BETWEEN THE 2
				float rightNeed = direction.x / thrustRight;
				float max = Mathf.Max(forwardNeed,rightNeed);
				return new Vector2(thrustBackward*(-forwardNeed)/max, thrustRight*(rightNeed)/max).magnitude;
			}
		} else
		{
			if (direction.y >=0)
			{
				float forwardNeed = direction.y / thrustForward;
				float rightNeed = -direction.x / thrustLeft;
				float max = Mathf.Max(forwardNeed,rightNeed);
				return new Vector2(thrustForward*(forwardNeed)/max, thrustLeft*(-rightNeed)/max).magnitude;
			} else
			{
				float forwardNeed = -direction.y / thrustBackward;
				float rightNeed = -direction.x / thrustLeft;
				float max = Mathf.Max(forwardNeed,rightNeed);
				return new Vector2(thrustBackward*(-forwardNeed)/max, thrustLeft*(-rightNeed)/max).magnitude;
			}
		}
	}
	
	public void setThrustInWorldDirection(Vector2 direction, float amount)
	{
		direction.Normalize();

		Vector2 direct = Quaternion.Inverse(gameObject.transform.rotation) * direction;
		
		setThrustInLocalDirection(direct, amount);
	}
	
	public void setRotationThrust(float amount)
	{
		if (amount > 1f) amount = 1f;
		if (amount < -1f) amount = -1f;
		thrustRotationAmount = amount;
	}
	
	public void StopMovementThrust()
	{
		thrustForwardAmount = 0f;
		thrustRightAmount = 0f;
		if (spaceObject.velocity.magnitude < 0.01f) spaceObject.rigidbody.velocity = Vector3.zero;
	}
	public void StopRotationThrust()
	{
		thrustRotationAmount = 0f;
	}
	
}

