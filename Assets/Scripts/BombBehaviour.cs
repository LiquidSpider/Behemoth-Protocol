using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombBehaviour : MonoBehaviour {

	private float fallFactor = -9.81f / 10;
	private Vector3 movement;

	public GameObject explosion;

	public void Initialise(Vector3 playerMovementDirection) {
		movement = playerMovementDirection;

		transform.parent = GameObject.FindGameObjectWithTag("BombParent").transform;
	}

	void Update() {
		movement.y += fallFactor * Time.deltaTime;

		gameObject.transform.position += movement;

		if (gameObject.transform.position.y < -10) {
			Destroy(gameObject);
		}
	}

	private void OnCollisionEnter (Collision other) {
		explosion = Instantiate(explosion);
		explosion.transform.position = transform.position;
		Destroy(gameObject);
	}
}
