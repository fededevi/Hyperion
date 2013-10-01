using UnityEngine;
using System.Collections;

public class MPlasmaProjectile : MonoBehaviour, WeaponDamage
{
	public GameObject explosion;
	
	private float minDamage = 200;
	private float maxDamage = 300;
	
	public float GetDamage()
	{
		float difference = maxDamage - minDamage;
		return Random.value * difference + minDamage;
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
		
		Destroy(gameObject);
	}
	
	
}

