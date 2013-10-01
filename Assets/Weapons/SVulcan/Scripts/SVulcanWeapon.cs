using UnityEngine;
using System.Collections;

public class SVulcanWeapon : MonoBehaviour, Weapon
{
	public GameObject vulcanCannonProj;
	
	public SpaceObject spaceObject;
	public Turret turret;
	
	private float fireDelay = 1f;
	private float energyUse = 0.5f;
	private float projectileSpeed = 15.0f;
	private float allowedAimingError = 0.2f;
	private int projectilesPerShot = 10;
	private float firingError = 0.1f;
	private float projectileLifeTime = 3.0f;
	
	void Start ()
	{
		spaceObject = transform.root.GetComponent<SpaceObject>();
		turret = gameObject.GetComponent<Turret>();
		turret.weapon = this;
	}
	
	public void Fire(Transform fireTransform)
	{
		float rotation = firingError * (Random.value-0.5f);
		Vector3 direction = Mathf2.RotateVector2(fireTransform.up, rotation);

		GameObject projectile = (GameObject)Instantiate(vulcanCannonProj, fireTransform.position, fireTransform.rotation);
		projectile.layer = spaceObject.weaponLayer;
		spaceObject.ExcludeCollisionWith(projectile.collider);
		projectile.rigidbody.velocity = direction * GetProjectileSpeed();
		Destroy(projectile, projectileLifeTime);
	}
	
	public float GetFireDelay()
	{
		return fireDelay;
	}
	
	public float GetEnergyUse()
	{
		return energyUse;
	}
	
	public float GetProjectileSpeed()
	{
		return projectileSpeed;
	}
	
	public float GetAllowedAimingError()
	{
		return allowedAimingError;
	}	
	
	public int GetProjectilesPerShot()
	{
		return projectilesPerShot;
	}

	public float GetRange()
	{
		return projectileSpeed*projectileLifeTime;
	}

}

