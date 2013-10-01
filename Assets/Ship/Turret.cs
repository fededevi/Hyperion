using UnityEngine;
using System.Collections;

public class Turret : MonoBehaviour
{
	/*
	 * Questa classe rappresenta non l'arma ma il tipo di supporto all'arma, se si mette un arma piccola su un hardpoint di livello alto la torretta
	 * verrà istanziata con più bocche ed un maggior rateo di fuoco. Sullo stesso gameobject verrà creato il componente Weapon<TYPE> e Ammo<TYPE> 
	 * contenenti i dati del tipo di arma (e funzioni per l'istanziamento del proiettile)
	 * */
	public SpaceObject spaceObject;
	public Hardpoint hardpoint;
	public Weapon weapon;

	public bool firing = false;
	
	public float elapsedTime = 0;
	
	public int indexOfFirePoints = 0;
	public Transform[] firePoints;
	
	void Start ()
	{
		hardpoint = transform.parent.gameObject.GetComponent<Hardpoint>();	
		hardpoint.turret = gameObject;
		spaceObject =  transform.root.GetComponent<SpaceObject>();
		//elapsedTime = weapon.GetFireDelay();
	}
	
	public void OnHierarchyDetached(Section destroyedSection)
	{
		enabled = false;
	}
	
	void FixedUpdate ()
	{
		//IF ENERGY IS ENOUGHT
		if ( weapon	!= null)
		{
			elapsedTime = elapsedTime + Time.fixedDeltaTime;
			if (firing && elapsedTime > weapon.GetFireDelay())
			{
				elapsedTime = 0f;
				if (indexOfFirePoints >= firePoints.Length) indexOfFirePoints = 0;
				weapon.Fire( (Transform)firePoints[indexOfFirePoints] );
				indexOfFirePoints++;
				
			}	
		}
	}
}

