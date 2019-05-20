using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDroneMovement : MonoBehaviour {

	private Vector3 originalPosition;

	private Vector3 directionOfMovement;

	private float flySpeed = 5;

	void Start() {
		originalPosition = transform.position;

		float nX = Random.Range(-15, 15);
		float nY = Random.Range(-15, 15);
		float nZ = Random.Range(-15, 15);
		directionOfMovement = new Vector3(nX, nY, nZ);
		directionOfMovement = directionOfMovement.normalized;
	}

	void Update() {
		if ((transform.position - originalPosition).magnitude > 30) {
			float nX = Random.Range(-15, 15);
			float nY = Random.Range(-15, 15);
			float nZ = Random.Range(-15, 15);

			directionOfMovement = new Vector3(nX, nY, nZ);
			directionOfMovement = directionOfMovement.normalized;
		}

		transform.position += directionOfMovement * Time.deltaTime * flySpeed;
	}

}
