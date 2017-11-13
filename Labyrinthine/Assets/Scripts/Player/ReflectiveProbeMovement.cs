﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectiveProbeMovement : MonoBehaviour 
{

	public float ForwardMovementSpeed = 0.2f;
	// Use this for initialization
	void Start () 
	{


	}
	
	// Update is called once per frame
	void Update () 
	{
		transform.Translate (Vector3.forward * ForwardMovementSpeed * Time.fixedDeltaTime);
	}
}