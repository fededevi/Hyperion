using UnityEngine;
using System.Collections;

public interface WeaponDamage
{
	float GetDamage();
}


public interface Weapon
{

	void  Fire(Transform fireTransform);
	float GetFireDelay();
	float GetEnergyUse();
	float GetProjectileSpeed();
	float GetAllowedAimingError();
	float GetRange();
	int GetProjectilesPerShot();
}
