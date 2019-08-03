using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowHealth : MonoBehaviour {

	private float timeOfScan = -1;
	private float scanDistance;

	private int materialMode = 0;

	void Start() {
		gameObject.GetComponent<MeshRenderer>().enabled = false;
		gameObject.GetComponent<MeshRenderer>().material = new Material(gameObject.GetComponent<MeshRenderer>().material);
		gameObject.GetComponent<MeshRenderer>().material.color = DetermineColour();
	}

	private void Update() {
		if (Input.GetMouseButtonDown(1) && materialMode == 0) {
			timeOfScan = Time.time;
			scanDistance = Vector3.Magnitude(transform.position - GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().player.transform.GetChild(0).position) / 1000f;
			materialMode = 1;
		}

		if (materialMode == 1 && Time.time > timeOfScan + scanDistance) {
			gameObject.GetComponent<MeshRenderer>().enabled = true;
			materialMode = 2;
		} else if (materialMode == 2) {
			if (!Input.GetMouseButton(1)) {
				gameObject.GetComponent<MeshRenderer>().enabled = false;
				materialMode = 0;
			}
		}
	}

	private Color DetermineColour() {
		float HP = transform.parent.GetComponent<EnemyHealth>().HP;
		float maxHP = transform.parent.GetComponent<EnemyHealth>().maxHP;

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

	public void DamageTaken() {
		gameObject.GetComponent<MeshRenderer>().material.color = DetermineColour();
	}

}
