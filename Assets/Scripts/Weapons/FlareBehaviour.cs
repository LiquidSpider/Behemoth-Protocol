using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlareBehaviour : MonoBehaviour {

	private Vector3 flareDropDirection;

	private float dropStartTime;

	public void Initialise(Vector3 playerMovementDirection) {
		transform.root.parent = GameObject.FindGameObjectWithTag("FlareParent").transform;

		flareDropDirection = playerMovementDirection;

		dropStartTime = Time.time;
	}

	private void Update() {
		gameObject.transform.position += new Vector3(0, -9.81f * Time.deltaTime * 3, 0);
		gameObject.transform.position += flareDropDirection * Time.deltaTime * 5 * (Mathf.Exp(dropStartTime - Time.time) * 10 + 1);
	}

	private void OnCollisionEnter(Collision other) {
		if (other.collider.tag == "Environment") Destroy(transform.parent.gameObject);

		//if (other.gameObject.tag != "Player" && other.gameObject.tag != "Flare") {
		//	Destroy(transform.parent.gameObject);
		//}
	}
}
