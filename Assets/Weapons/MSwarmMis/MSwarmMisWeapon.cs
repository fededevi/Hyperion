using UnityEngine;
using System.Collections;

public class MSwarmMisWeapon : MonoBehaviour, Weapon
{
	public GameObject mSwarmMisProj;
	public SpaceObject spaceObject;
	public Turret turret;
	private float fireDelay = 24.0f;
	private float energyUse = 0.5f;
	private float projectileSpeed = 5.0f;
	private float allowedAimingError = 7.0f;
	private int projectilesPerShot = 12;
	private float projectileLifeTime = 12.0f;
	private float range = 60f;

	void Start ()
	{
		spaceObject = transform.root.GetComponent<SpaceObject> ();
		turret = gameObject.GetComponent<Turret> ();
		turret.weapon = this;
	}

	public void  Fire (Transform fireTransform)
	{		
		    StartCoroutine( FireDelayed(fireTransform));

	}
	
	public IEnumerator FireDelayed (Transform fireTransform)
	{
		for (int i = 0; i < projectilesPerShot; i++) {
			
			float rotation = (Mathf.PI * 2f) * ((float)i / (float)projectilesPerShot);
			Vector3 direction = Mathf2.RotateVector2 (fireTransform.up, rotation);
			GameObject projectile = (GameObject)Instantiate (mSwarmMisProj, fireTransform.position, fireTransform.rotation);
			projectile.transform.Rotate (new Vector3 (0f, 0f, rotation * (180f / Mathf.PI)));
			projectile.layer = spaceObject.weaponLayer;
			spaceObject.ExcludeCollisionWith (projectile.collider);
			projectile.rigidbody.velocity = direction * GetProjectileSpeed ();
			
			//SettingUp Missile Target
			MSwarmMissileProjectile missile = projectile.GetComponent<MSwarmMissileProjectile> ();
			missile.target = turret.hardpoint.currentTarget.transform;
			Destroy (projectile, projectileLifeTime);
			yield return new WaitForSeconds(0.1f);
		}
		
	}

	public float GetFireDelay ()
	{
		return fireDelay;
	}

	public int GetProjectilesPerShot ()
	{
		return projectilesPerShot;
	}

	public float GetEnergyUse ()
	{
		return energyUse;
	}

	public float GetProjectileSpeed ()
	{
		return projectileSpeed;
	}

	public float GetAllowedAimingError ()
	{
		return allowedAimingError;
	}
	
	public float GetRange ()
	{
		return range;
	}
	
}

