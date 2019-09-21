using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleBehaviour : MonoBehaviour {

	private GameObject player;

	private void Start() {
		gameObject.transform.parent = GameObject.FindGameObjectWithTag("MissileParent").transform.GetChild(0);
		player = GameObject.FindGameObjectWithTag("Player");
	}

	private void Update() {
		gameObject.transform.LookAt(player.transform.position);
		transform.position += transform.forward * 15;

		if (Vector3.Distance(player.transform.position, gameObject.transform.position) < 10f) {
			Destroy(gameObject);
		}
	}


	//private float deathTime;
	//private float lifetime = 1.5f;

	//private float trailMovementTime;
	//private float trailMovementDelay = 0.01f;

	//private Color oiginalColour;

	//public Vector3 directionFromCentre;

	//// // Method 1
	////private float startScale = 0.25f;
	////private float scaleChange = 0.075f;

	////// Start is called before the first frame update
	////void Start() {
	////	deathTime = Time.time + lifetime;
	////	transform.localScale = new Vector3(startScale, startScale, startScale);
	////}

	////// Update is called once per frame
	////void Update() {
	////	if (Time.time > deathTime) Destroy(gameObject);

	////	transform.position += new Vector3(0, 0.05f, 0);

	////	Color old = GetComponent<MeshRenderer>().material.color;
	////	old.a = old.a - (Time.deltaTime / lifetime);

	////	GetComponent<MeshRenderer>().material.color = old;

	////	transform.localScale += new Vector3(scaleChange, scaleChange, scaleChange);
	////}

	//private float particleSize = 0.2f;

	//// Method 2
	//void Start() {
	//	deathTime = Time.time + lifetime;
	//	transform.localScale = new Vector3(particleSize, particleSize, particleSize);
	//	oiginalColour = GetComponent<MeshRenderer>().material.color;

	//	trailMovementTime = Time.time + trailMovementDelay;
	//}

	//void Update() {
	//	if (Time.time > deathTime) Destroy(gameObject);

	//	Color old = GetComponent<MeshRenderer>().material.color;
	//	//old.r = old.r - ( ( 1 - oiginalColour.r ) * ( Time.deltaTime / lifetime ) );
	//	//old.g = old.g - ( ( 1 - oiginalColour.g ) * ( Time.deltaTime / lifetime ) );
	//	//old.b = old.b - ( ( 1 - oiginalColour.b ) * ( Time.deltaTime / lifetime ) );
	//	old.a = old.a - ( Time.deltaTime / lifetime );

	//	GetComponent<MeshRenderer>().material.color = old;

	//	if (Time.time > trailMovementTime) {
	//		float factor = Time.time - trailMovementTime;
	//		transform.position += new Vector3(0, Mathf.Pow(2.61628f, factor) / 10, 0);
	//		transform.position += directionFromCentre * Time.deltaTime * 3;
	//	}
	//}
}
