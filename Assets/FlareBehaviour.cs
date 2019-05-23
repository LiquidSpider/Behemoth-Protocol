using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlareBehaviour : MonoBehaviour {

	private Vector3 flareDropDirectoin;

	public void Initialise(Vector3 playerMovementDirection) {
		transform.root.parent = GameObject.FindGameObjectWithTag("FlareParent").transform;

		flareDropDirectoin = playerMovementDirection;
	}

	private void Update() {
		gameObject.transform.position += new Vector3(0, -9.81f * Time.deltaTime * 3, 0);
		gameObject.transform.position += flareDropDirectoin * Time.deltaTime * 3;
	}

	private void OnCollisionEnter(Collision other) {
		if (other.gameObject.tag != "Player" && other.gameObject.tag != "Flare") {
			Destroy(gameObject.transform.parent.gameObject);
		}
	}
}
