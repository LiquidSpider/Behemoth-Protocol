using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleEmitter : MonoBehaviour {

	public GameObject particle;
	public Vector3 spawn;


	// // Method 1
	//void Update() {
	//	spawn = transform.position;

	//	for (float i = 0; i < 10; i++) {
	//		float offsetX = Random.Range(-i/10, i/10);
	//		float offsetY = Random.Range(-i/10, i/10);

	//		Vector3 up = transform.up;
	//		Vector3 right = transform.right;

	//		Vector3 position = spawn + offsetX * up + offsetY * right;

	//		GameObject newParticle = Instantiate(particle);

	//		newParticle.transform.position = position;
	//		newParticle.transform.parent = GameObject.FindGameObjectWithTag("MissileParent").transform.GetChild(0);
	//	}
	//}

	// Method 2
	void Update() {
		spawn = transform.position;

		for (float i = 0; i < 15; i++) {
			float offsetX = Random.Range(-0.2f, 0.2f);
			float offsetY = Random.Range(-0.2f, 0.2f);
			float offsetZ = Random.Range(-0.5f, 0.1f);

			Vector3 up = transform.up;
			Vector3 right = transform.right;
			Vector3 forward = transform.forward;

			Vector3 position = spawn + offsetX * up + offsetY * right + offsetZ * forward;

			GameObject newParticle = Instantiate(particle);

			newParticle.GetComponent<ParticleBehaviour>().directionFromCentre = (offsetX * up + offsetY * right);
			newParticle.transform.position = position;
			newParticle.transform.parent = GameObject.FindGameObjectWithTag("MissileParent").transform.GetChild(0);
		}
	}
}
