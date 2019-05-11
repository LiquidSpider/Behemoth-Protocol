using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionBehaviour : MonoBehaviour {

	private float timeOfExplosion;
	private float explosionTime = 0.8f;

	private float fadeStartTime = -1;
	private float fadeTime = 2f;

	void Start() {
		transform.parent = GameObject.FindGameObjectWithTag("ExplosionParent").transform;
		timeOfExplosion = Time.time;
	}
	
	void Update() {
		if (Time.time > fadeStartTime + fadeTime) {
			if (Time.time > timeOfExplosion + explosionTime) {
				if (fadeStartTime == -1) {
					fadeStartTime = Time.time;
				}

				Color old = GetComponent<MeshRenderer>().material.color;
				old.a = old.a - ( Time.deltaTime / fadeTime );

				GetComponent<MeshRenderer>().material.color = old;
			} else {
				float scaleFactor = Time.time - timeOfExplosion;
				transform.localScale += new Vector3(scaleFactor, scaleFactor, scaleFactor);
			}
		} else {
			Destroy(gameObject);
		}
	}
}
