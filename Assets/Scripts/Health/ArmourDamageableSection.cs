﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmourDamageableSection : MonoBehaviour {

	private Material mainMaterial;
	public Material changedMaterial;

	private float timeOfScan = -1;
	private float scanDistance;

	private int materialMode = 0;

	private void Start() {
		mainMaterial = gameObject.GetComponent<MeshRenderer>().material;
		changedMaterial = new Material(changedMaterial);
	}

	private void Update() {
		if (GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().player.transform.root.gameObject.GetComponent<PlayerHealth>().isScanning) {
			if (materialMode == 0) {
				timeOfScan = Time.time;
				scanDistance = Vector3.Magnitude(transform.position - GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().player.transform.GetChild(1).position) / 450f;
				materialMode = 1;
			}
			if (materialMode == 1 && Time.time > timeOfScan + scanDistance) {
				changedMaterial.color = DetermineColour();
				gameObject.GetComponent<MeshRenderer>().material = changedMaterial;
				materialMode = 2;
			}
		} else {
			if (materialMode != 0) {
				gameObject.GetComponent<MeshRenderer>().material = mainMaterial;
				materialMode = 0;
			}
		}
	}

	private void OnCollisionEnter(Collision other) {
		if (other.gameObject.transform.tag == "Explosion - Player") {
			GetComponent<ArmourHealth>().TakeDamage(100, other.gameObject);
		}

		if (other.gameObject.transform.tag == "Bullet - Player") {
			GetComponent<ArmourHealth>().TakeDamage(10, other.gameObject);
		}

		changedMaterial.color = DetermineColour();
	}

	private Color DetermineColour() {
		float HP = GetComponent<ArmourHealth>().HP;
		float maxHP = GetComponent<ArmourHealth>().maxHP;

		float temp = HP / maxHP;
		Color tempColour = new Color();

		if (temp <= 0.05f) {
			tempColour = new Color(1, 0, 0);
		} else if (temp <= 0.48f) {
			float rFactor = (0.48f - temp) / 0.43f;
			float yFactor = (temp - 0.05f) / 0.43f;

			tempColour = new Color(rFactor + yFactor, yFactor, 0);
		} else if (temp <= 0.53f) {
			tempColour = new Color(1, 1, 0);
		} else if (temp <= 0.95f) {
			float yFactor = (0.95f - temp) / 0.42f;
			float gFactor = (temp - 0.53f) / 0.42f;

			tempColour = new Color(yFactor, yFactor + gFactor, 0);
		} else {
			tempColour = new Color(0, 1, 0);
		}

		return tempColour;
	}
}