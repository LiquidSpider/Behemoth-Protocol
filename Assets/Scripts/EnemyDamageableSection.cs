using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageableSection : MonoBehaviour {

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
		if (Input.GetMouseButtonDown(1) && materialMode == 0) {
			timeOfScan = Time.time;
			scanDistance = Vector3.Magnitude(transform.position - GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().player.transform.position) / 500f;
			materialMode = 1;
		}

		if (materialMode == 1 && Time.time > timeOfScan + scanDistance) {
			changedMaterial.color = DetermineColour();
			gameObject.GetComponent<MeshRenderer>().material = changedMaterial;
			materialMode = 2;
		} else if (materialMode == 2) {
			if (!Input.GetMouseButton(1)) {
				gameObject.GetComponent<MeshRenderer>().material = mainMaterial;
				materialMode = 0;
			}
		}
	}

	private void OnCollisionEnter(Collision other) {
		if (other.gameObject.transform.tag == "Explosion - Player") {
			Transform parent = transform.root;

			parent.GetComponent<EnemyHealth>().TakeDamage(200, other.gameObject);
		}

		if (other.gameObject.transform.tag == "Bullet - Player") {
			Transform parent = transform.root;

			parent.GetComponent<EnemyHealth>().TakeDamage(10);
		}

		changedMaterial.color = DetermineColour();
	}

	private Color DetermineColour() {
		float HP = transform.root.GetComponent<EnemyHealth>().HP;
		float maxHP = transform.root.GetComponent<EnemyHealth>().maxHP;

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
