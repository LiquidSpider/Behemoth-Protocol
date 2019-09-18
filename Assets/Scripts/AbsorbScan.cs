using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbsorbScan : MonoBehaviour {
	private float speed = 150f;

	private void Start() {
		gameObject.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material.color = Color.cyan;

		gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-speed, speed), Random.Range(-speed, speed), Random.Range(-speed, speed)));
	}

	void Update() {
		if (GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>().isScanning) {
			gameObject.transform.GetChild(0).gameObject.SetActive(true);
		} else {
			gameObject.transform.GetChild(0).gameObject.SetActive(false);
		}
	}
}
