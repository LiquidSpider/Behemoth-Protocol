using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyMissileIndication : MonoBehaviour {

	public GameObject dot;

	private List<GameObject> hasADot = new List<GameObject>();

	void Start() {

	}
	
	void Update() {
		GameObject[] missiles = GameObject.FindGameObjectsWithTag("Missile");

		foreach (GameObject missile in missiles) {
			if (missile && missile.GetComponent<MissileBehaviour>().owner.tag == "Enemy" && !hasADot.Contains(missile)) {
				GameObject newDot = Instantiate(dot);
				newDot.transform.SetParent(gameObject.transform);
				newDot.GetComponent<RectTransform>().position = Camera.main.WorldToViewportPoint(missile.transform.position);

				missile.GetComponent<MissileBehaviour>().dot = newDot;

				hasADot.Add(missile);
			}
		}

		List<GameObject> tempHasADot = new List<GameObject>();
		foreach (GameObject missile in hasADot) {
			if (missile) {
				tempHasADot.Add(missile);
			}
		}
		hasADot = tempHasADot;
	}
}
