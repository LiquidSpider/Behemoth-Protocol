﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBehaviour : MonoBehaviour {

	public float resourceAvailable;
	private float maxResourceAvailable;
	private float waterLevel;

	private float yStartLocation;

	private Vector3 position;

	void Start() {
		resourceAvailable = 2500;
		maxResourceAvailable = resourceAvailable;

		waterLevel = resourceAvailable / maxResourceAvailable;

		position = gameObject.transform.position;
		yStartLocation = gameObject.transform.position.y;
	}

	void Update() {
		if (resourceAvailable < maxResourceAvailable) {
			resourceAvailable += 1 * Time.deltaTime;
		}

		position.y = yStartLocation - (( 1 - (resourceAvailable / maxResourceAvailable) ) * 10);
		transform.position = position;
	}

	public bool TakeWater(float waterUsed) {
		if (resourceAvailable > waterUsed) {
			resourceAvailable -= waterUsed;
			return true;
		} else {
			return false;
		}
	}
}