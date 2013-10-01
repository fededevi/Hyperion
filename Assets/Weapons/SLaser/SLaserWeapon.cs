using UnityEngine;
using System.Collections;

public class SLaserWeapon : MonoBehaviour, Weapon, WeaponDamage
{
	public SpaceObject spaceObject;
	public Turret turret;

	private float fireDelay = 0.25f;
	private float energyUse = 0.5f;
	private float projectileSpeed = 800.0f;
	private float allowedAimingError = 0.2f;
	private int projectilesPerShot = 1;
	
	private float beamRange = 50f;

	public GameObject fxLine0;
	public GameObject fxLine1;
	public GameObject fxParticles;
	public GameObject fxSparkles;
	Material fxMaterial;
	Color fxMaterialColor;

	void Start ()
	{
		spaceObject = transform.root.GetComponent<SpaceObject> ();
		turret = gameObject.GetComponent<Turret> ();
		turret.weapon = this;
		
		fxMaterial = fxLine0.renderer.material;
		fxMaterialColor = fxMaterial.GetColor("_TintColor");
		fxLine0.renderer.material = fxMaterial;
		fxLine1.renderer.material = fxMaterial;
		fxParticles.renderer.material = fxMaterial;
		fxSparkles.renderer.material = fxMaterial;
	}

	public float GetDamage ()
	{
		return 200f;
	}

	public void FixedUpdate ()
	{
		if (turret.firing) 
		{
			RaycastHit hitPoint;
			float distance = beamRange;
			
			fxSparkles.SetActive(false);
			
			if (Physics.Raycast (turret.firePoints[0].transform.position, turret.firePoints[0].transform.up, out hitPoint, beamRange, spaceObject.weaponMask)) {
				distance = Vector3.Distance (hitPoint.point, fxLine0.transform.position);
				
				fxSparkles.SetActive(true);
				fxSparkles.transform.localPosition = new Vector3 (0f, distance, 0f);
			}
			
			float fractionOfDistance = distance / beamRange;
			fxLine0.SetActive(true);
			fxLine1.SetActive(true);
			fxLine0.GetComponent<LineRenderer> ().SetPosition (1, new Vector3 (0f, distance, 0f));
			fxLine1.GetComponent<LineRenderer> ().SetPosition (1, new Vector3 (0f, distance, 0f));
			fxParticles.SetActive(true);
			fxParticles.particleEmitter.localVelocity = new Vector3 (0f, 25f * fractionOfDistance, 0f);
			
			
			if (fxMaterialColor.a < 1f ) 
			{
				fxMaterialColor = new Color(fxMaterialColor.r, fxMaterialColor.g, fxMaterialColor.b, fxMaterialColor.a + Time.deltaTime);
				fxMaterial.SetColor("_TintColor", fxMaterialColor);
				return;
			}
		} else 
		{
			//Se il laser era acceso nascondilo lentamente
			if (fxMaterialColor.a > 0.01f ) 
			{
				fxMaterialColor = new Color(fxMaterialColor.r, fxMaterialColor.g, fxMaterialColor.b, fxMaterialColor.a - Time.deltaTime);
				fxMaterial.SetColor("_TintColor", fxMaterialColor);
				return;
			} else
			{
				//spegnamo tutto
				fxLine0.SetActive(false);
				fxLine1.SetActive(false);
				fxParticles.SetActive(false);
				fxSparkles.SetActive(false);
			}
		}
	}

		

	public void Fire (Transform fireTransform)
	{
		/*
		float rotation = firingError * (Random.value-0.5f);
		Vector3 direction = Mathf2.RotateVector2(fireTransform.up, rotation);
		GameObject projectile = (GameObject)Instantiate(vulcanCannonProj, fireTransform.position, fireTransform.rotation);
		spaceObject.ExcludeCollisionWith(projectile.collider);
		projectile.rigidbody.velocity = direction * GetProjectileSpeed();
		Destroy(projectile, projectileLifeTime);
		*/		
		//Debug.DrawLine(fireTransform.position, fireTransform.position+fireTransform.up*50f);
		RaycastHit hitPoint;
		if (Physics.Raycast (turret.firePoints[0].transform.position, turret.firePoints[0].transform.up, out hitPoint, beamRange, spaceObject.weaponMask)) {
			hitPoint.collider.SendMessage ("WeaponHit", this);
		}
	}

	public float GetFireDelay ()
	{
		return fireDelay;
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
	
	public int GetProjectilesPerShot()
	{
		return projectilesPerShot;
	}
	
	public float GetRange()
	{
		return beamRange;
	}
	
}

