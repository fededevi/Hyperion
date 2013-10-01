using UnityEngine;
using System.Collections;

public class MSwarmMissileProjectile : MonoBehaviour, WeaponDamage
{
	public GameObject explosion;
	public GameObject trail;
	//MissileTarget
	public Transform target;
	
	private float minDamage = 100;
	private float maxDamage = 200;
	public float age = 0f;
	private float rotationSpeed = 1.2f;
	
	public float GetDamage()
	{
		float difference = maxDamage - minDamage;
		return Random.value * difference + minDamage;
	}
	
	public void FixedUpdate ()
	{ 
		if ( target != null)
		{
		age = age + Time.fixedDeltaTime;
		if (age > 1.5f)
		{
			Vector3 targetDirection = (target.position - transform.position).normalized;
			float angle = Mathf2.AngleBetweenVectors(transform.up, targetDirection);
			float sign = angle / Mathf.Abs(angle);
			float amount = Time.fixedDeltaTime*rotationSpeed;
			angle = Mathf.Abs(angle);
			if ( amount > angle ) amount = angle;
			if (Mathf.Abs(angle) > 0.01f) transform.Rotate(new Vector3(0f,0f,sign*amount*(180f/Mathf.PI)));
			rigidbody.AddRelativeForce(Vector3.up*3.30f);
		}
		} else
		{
			rigidbody.AddRelativeForce(Vector3.up*3.30f);
		}
	}
	
	void OnCollisionEnter(Collision collision) 
	{		
		Collider rightCollider = null;
		ContactPoint lastContactPoint;
		
		foreach (ContactPoint contact in collision.contacts)
		{
			rightCollider = contact.otherCollider;
			lastContactPoint = contact;
			Instantiate(explosion, transform.position, Quaternion.FromToRotation(Vector3.up, lastContactPoint.normal));
		}
		
		rightCollider.gameObject.SendMessage("WeaponHit",this);
		trail.transform.parent = null;
		Destroy(gameObject);
	}

}

