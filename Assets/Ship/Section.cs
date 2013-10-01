using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Section : MonoBehaviour
{
	public enum Size
	{
		Small,
		Medium,
		Big,
		Huge
	}
	public Size size;
	public GameObject[] graphicsObjects;
	public SpaceObject spaceObject;
	public Section parentSection;
	public List<Section> subSections;
	public float hp = 500f;
	public float maxHp = 500f;
	public float armor = 30;
	public float mass = 10f;
	public bool dying = false;
	public bool detached = false;
	public float baseExplosionSpeed = 1f;
	private float explosionSpeed = 1f;
	private GameObject explosion;
	
	Color c;
	
	void Awake ()
	{
		if (renderer != null) c = renderer.material.color;
		spaceObject = (SpaceObject)transform.root.GetComponent<SpaceObject>();
		spaceObject.sections.Add (this);
		
		if ( transform.root != transform.parent) //If parent is not root
		{
			parentSection = (Section) transform.parent.gameObject.GetComponent<Section>();
			if (parentSection.subSections == null)  parentSection.subSections = new List<Section>();
			parentSection.subSections.Add(this);
		}
	}
	
	void Start()
	{
		if ( size == Size.Small )  explosion = spaceObject.listsKeeper.sectionExplosionSmall;
		if ( size == Size.Medium )  explosion = spaceObject.listsKeeper.sectionExplosionMedium;
		if ( size == Size.Big )  explosion = spaceObject.listsKeeper.sectionExplosionBig;
		if ( size == Size.Huge )  explosion = spaceObject.listsKeeper.sectionExplosionHuge;	
	}

	void FixedUpdate ()
	{
		if ( detached && hp > 0f ) 
		{
			hp = hp - 10f * Time.fixedDeltaTime;
			if (hp < 0f) explosion = null;
		}
		
		
		if (transform.parent == null && rigidbody == null) //IF THIS IS A ROOT ADD A RIGIDBODY
		{
			gameObject.AddComponent<Rigidbody>();
			gameObject.rigidbody.mass = mass;//GetHierarchyMass(this);
			gameObject.rigidbody.useGravity = false;
			gameObject.rigidbody.angularDrag = 0.5f;
			gameObject.transform.position = new Vector3(gameObject.transform.position.x,gameObject.transform.position.y,0f);
			gameObject.rigidbody.drag = 1f;
			ConfigurableJoint joint = gameObject.AddComponent<ConfigurableJoint>();
			joint.configuredInWorldSpace = true;
			joint.zMotion = ConfigurableJointMotion.Locked;
			joint.angularXMotion = ConfigurableJointMotion.Locked;
			joint.angularYMotion = ConfigurableJointMotion.Locked;
			joint.anchor = Vector3.zero;
			
		}

		if (hp <= 0 && !dying) 
		{
			dying = true;
			StartCoroutine (OnDestroyed());
		}
		
		if (hp < 0)
		{
			float a = 1f + (-hp / maxHp);
			explosionSpeed = baseExplosionSpeed / a;
		}
		
		if (renderer != null)
		{
			renderer.material.color = new Color( c.r * (hp / maxHp),c.g * (hp / maxHp),c.b * (hp / maxHp),c.a * (hp / maxHp));
		}
	}
	
	public float GetHierarchyMass(Section sec)
	{
		float subSectionsMass = 0;
		if (subSections	!= null && subSections.Count > 0)
			foreach (Section subSec in sec.subSections)
				subSectionsMass += GetHierarchyMass(subSec);
		return sec.mass + subSectionsMass;
	}

	private void Explosion (Vector3 position)
	{
		if (explosion != null)
		{
		GameObject exp = (GameObject)Instantiate (explosion, position, transform.rotation);
		exp.layer = 10;
		}
	}

	private IEnumerator OnDestroyed ()
	{
		//DO SOME FANCY EXPLOSIONS
		int i = (int)(Random.value * 2f);//4 Explosions Max
		while (i > 0) {
			i--;
			yield return new WaitForSeconds (Random.value * explosionSpeed * 0.5f);
			Explosion (transform.position + (Vector3)Random.insideUnitCircle);
		}
		
		yield return new WaitForSeconds (Random.value * explosionSpeed * 0.5f);
		if (transform.parent != null) Explosion( (transform.position + transform.parent.position) / 2f );
		if (parentSection != null) parentSection.subSections.Remove(this);
		
		if (spaceObject != null) spaceObject.LostSection();
		transform.parent = null;
		this.BroadcastMessage("OnHierarchyDetached", this);
		foreach (Section sec in subSections) 
		{
			sec.transform.parent=null;
		}
		yield return new WaitForSeconds (Random.value * explosionSpeed);
		yield return new WaitForSeconds (Random.value * explosionSpeed);
		yield return new WaitForSeconds (Random.value * explosionSpeed);
		i = 1 + (int)(Random.value * 3f);//3 Explosions Max
		while (i > 0) {
			i--;
			yield return new WaitForSeconds (Random.value * explosionSpeed * 0.5f);
			Explosion (transform.position + (Vector3)Random.insideUnitCircle);
		}
		Destroy (gameObject);
	}
	
	public void OnHierarchyDetached(Section destroyedSection)
	{
		if (spaceObject != null)
		{
			spaceObject.sections.Remove(this);
			spaceObject.LostSection();
		}
		detached = true;
	}


	public void Damage (float amount)
	{		
		float blockedAmount = armor;//Random.value * armor;
		float finalDamage = amount - blockedAmount;
		if (finalDamage < 0f)
			finalDamage = 0f;
		if ( spaceObject != null ) if ( spaceObject.ship.godMode ) finalDamage = 0;
		hp = hp - finalDamage;
	}
	
	void OnCollisionEnter(Collision collision) 
	{
		//Debug.Log("Collision, impact Force:");
		//Debug.Log(collision.impactForceSum);
	}

	public void WeaponHit(WeaponDamage weaponDamage)
	{
		spaceObject.listsKeeper.SetInCombat();
		Damage(weaponDamage.GetDamage());
	}
		
	void Update ()
	{
		
	}
}

