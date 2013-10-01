using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShipCommands : MonoBehaviour
{
	public SpaceObject spaceObject;
	
	public enum lastCommand
	{
		Attack,
		Move,
		Conquer,
	}
	
	
	
	void Awake ()
	{
		spaceObject = transform.root.gameObject.GetComponent<SpaceObject>();	
	}
	
	void Start ()
	{
		
	}

	// Update is called once per frame
	void Update ()
	{
		
	}
}

public class Formation 
{
	//public Lists<Ship> shipsInFormation;
	public Transform baryCenter;
	
	public GameObject attackTarget;
	public GameObject moveTarget;
}

