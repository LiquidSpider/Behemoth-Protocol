using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlareBehaviour : MonoBehaviour {

	private Vector3 flareDropDirection;

	private float dropStartTime;

	private Light myLight;
	private float changeIntensityTimer = 0.0f;

	public GameObject flareModel;

	public void Initialise(Vector3 playerMovementDirection) {
		myLight = gameObject.GetComponent<Light>();

		transform.root.parent = GameObject.FindGameObjectWithTag("FlareParent").transform;

		flareDropDirection = playerMovementDirection;

		dropStartTime = Time.time;



		GameObject[] currentEnemyMissiles = GameObject.FindGameObjectsWithTag("Missile");

		foreach (GameObject missile in currentEnemyMissiles) {
			if (missile.name == "ENEMY Missile") {
				if (Vector3.Distance(missile.transform.position, gameObject.transform.position) < 150f) {
					missile.GetComponent<EnemyMissileBehaviour>().Distraction(gameObject);
				}
			}
		}
	}

	private void Update() {
		gameObject.transform.position += new Vector3(0, -9.81f * Time.deltaTime * 3, 0);
		gameObject.transform.position += flareDropDirection * Time.deltaTime * 5 * (Mathf.Exp(dropStartTime - Time.time) * 10 + 1);

		changeIntensityTimer += Time.deltaTime;
		if(changeIntensityTimer > 0.3) {
			myLight.range = Random.Range(55.0f, 65.0f);
			myLight.intensity = Random.Range(2.7f, 3.3f);
		}

		flareModel.transform.Rotate(0.0f, 0.0f, 4.5f);
	}

	private void OnCollisionEnter(Collision other) {
		if (other.collider.tag == "Environment") Destroy(transform.parent.gameObject);

		//if (other.gameObject.tag != "Player" && other.gameObject.tag != "Flare") {
		//	Destroy(transform.parent.gameObject);
		//}
	}
}
