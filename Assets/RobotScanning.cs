using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotScanning : MonoBehaviour {
	public LayerMask healthLayer;
	private GameObject player;

	private float timeOfScan = -1;
	private float scanDistance;

	private int materialMode = 0;

	void Start() {
		player = GameObject.FindGameObjectWithTag("Player");

		gameObject.GetComponent<MeshRenderer>().enabled = false;
		gameObject.GetComponent<MeshRenderer>().material = new Material(gameObject.GetComponent<MeshRenderer>().material);

		Material tempMat = gameObject.GetComponent<MeshRenderer>().material;
		tempMat.SetFloat("Far fade", 1000);
		gameObject.GetComponent<MeshRenderer>().material = tempMat;

		gameObject.GetComponent<MeshRenderer>().material.color = DetermineColour();
	}

	private void Update() {
		if (player.GetComponent<PlayerHealth>().isScanning && materialMode == 0) {
			timeOfScan = Time.time;
			scanDistance = Vector3.Magnitude(transform.position - player.transform.position) / 2000f;
			materialMode = 1;
		}

		if (materialMode == 1 && Time.time > timeOfScan + scanDistance) {
			gameObject.GetComponent<MeshRenderer>().enabled = true;
			materialMode = 2;
		} else if (materialMode == 2) {
			gameObject.GetComponent<MeshRenderer>().material.color = DetermineColour();

			if (!player.GetComponent<PlayerHealth>().isScanning) {
				gameObject.GetComponent<MeshRenderer>().enabled = false;
				materialMode = 0;
			}
		}
	}

	private Color DetermineColour() {
		float HP = 1;
		float maxHP = 1;

		foreach (BaseHealth tempBaseHealth in transform.root.GetComponents<BaseHealth>()) {
			if (tempBaseHealth.healthLayer == healthLayer) {
				HP = tempBaseHealth.health;
				maxHP = tempBaseHealth.startingHealth;
			}

		}

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

		tempColour.a = 0.01f;

		return tempColour;
	}
}
