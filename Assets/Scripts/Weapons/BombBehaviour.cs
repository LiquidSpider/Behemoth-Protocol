using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombBehaviour : MonoBehaviour {

	private float fallFactor = -9.81f;

	public GameObject explosion;

	public void Initialise(Vector3 playerMovementDirection) {
		transform.parent = GameObject.FindGameObjectWithTag("BombParent").transform;
	}

	void Update() {
		gameObject.transform.position += new Vector3(0, fallFactor * Time.deltaTime * 3, 0);

		if (gameObject.transform.position.y < -10) {
			Destroy(gameObject);
		}
	}

	private void OnCollisionEnter (Collision other) {
		explosion = Instantiate(explosion);
		explosion.transform.position = transform.position;
		explosion.tag = "Explosion - Player";
		Destroy(gameObject);
	}
}
