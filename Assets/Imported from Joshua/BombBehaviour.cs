using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombBehaviour : MonoBehaviour {

	private float fallFactor = -9.81f / 10;
	private Vector3 movement;

	public void Initialise(Vector3 playerMovementDirection) {
		movement = playerMovementDirection;
	}

	void Update() {
		movement.y += fallFactor * Time.deltaTime;

		gameObject.transform.position += movement;

		if (gameObject.transform.position.y < -10) {
			Destroy(gameObject);
		}
	}

	void OnCollisionEnter(Collision collider) {
		Destroy(gameObject);
		print("Boom");
	}
}
