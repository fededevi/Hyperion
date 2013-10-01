using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody))]

public class SpaceObject : MonoBehaviour
{
	public enum Standing
	{
		Enemy,
		Neutral,
		Friendly,
		Player
	}
	public Standing standing;
	
	public int weaponLayer;
	public int weaponMask;

	public Vector2 position;
	public Vector2 velocity;
	public Vector2 heading;

	public float cargoSpaceUsed = 1f;
	public float hangarSpaceUsed = 1f;

	public Ship ship = null;
	public Generator generator = null;
	public Engine engine = null;
	public Movement movement = null;

	public List<Hardpoint> hardpoints = new List<Hardpoint> ();
	public List<Section> sections = new List<Section> ();

	public bool debug = true;
	
	public ListsKeeper listsKeeper;
	
	void Awake ()
	{
		GameObject list = GameObject.Find("ListsKeeper");
		listsKeeper = list.GetComponent<ListsKeeper>();
		listsKeeper.AddSpaceObject(this);
	}
	
	void Start ()
	{
		rigidbody.centerOfMass = Vector3.zero;
		
	}

	void Update ()
	{
		velocity = gameObject.rigidbody.velocity;
		heading = gameObject.transform.up;
		position = gameObject.transform.position;
		
	}

	public static void SetLayerRecursively (GameObject obj, int layer)
	{
		obj.layer = layer;
		
		foreach (Transform child in obj.transform) {
			SetLayerRecursively (child.gameObject, layer);
		}
	}

	void FixedUpdate ()
	{
		if (standing == Standing.Enemy) 
		{
			gameObject.layer = 14;
			weaponLayer = 10;
			weaponMask = 1 << 15 | 1 << 16 | 1 << 17;
			//Neutral Ally Player
		} else
		if (standing == Standing.Neutral) 
		{
			gameObject.layer = 15;
			weaponLayer = 11;
			weaponMask = 1 << 14 | 1 << 15 | 1 << 16 | 1 << 17;
			//Neutral Ally Player
		} else
		if (standing == Standing.Friendly) 
		{
			gameObject.layer = 16;
			weaponLayer = 12;
			weaponMask = 1 << 14 | 1 << 15;
			//Neutral Ally Player
		} else
		if (standing == Standing.Player) 
		{
			gameObject.layer = 17;
			weaponLayer = 13;
			weaponMask = 1 << 14 | 1 << 15;
			//Neutral Ally Player
		}
	}
	
	public void LostSection()
	{
		if (sections.Count == 0) 
		{
			listsKeeper.spaceObjects.Remove(this);
			if (IsFriendly()) listsKeeper.friendlyCore.supplies += ship.supplyCost;
			if (!IsFriendly()) listsKeeper.enemyCore.supplies += ship.supplyCost;
			Destroy(gameObject);
		}
	}
	
	/*void OnCollisionEnter(Collision collision) 
	{
		//Debug.Log("Collision, impact Force:");
		//print(collision.impactForceSum);
		

		Collider thisCollider = null;	
		Collider otherCollider = null;	
		foreach (ContactPoint contact in collision.contacts) 
		{
			thisCollider = contact.thisCollider;
			otherCollider = contact.otherCollider;
		}
		float collisionDamage = Mathf.Pow(collision.impactForceSum.magnitude, 2f) * otherCollider.rigidbody.mass;
		//print(thisCollider.name +" damage: "+collisionDamage);
		thisCollider.GetComponent<Section>().Damage(collisionDamage);
		
	}*/

	public void ExcludeCollisionWith (Collider projectileCollider)
	{
		foreach (Section sec in sections) {
			Physics.IgnoreCollision (sec.collider, projectileCollider);
		}
	}
	
	public bool IsFriendly()
	{
		return standing == SpaceObject.Standing.Friendly;
	}
}
