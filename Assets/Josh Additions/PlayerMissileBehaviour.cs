using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMissileBehaviour : MonoBehaviour {

	public GameObject explosion;

	private float launchTime = 2f;
	private bool launched = false;
	private Quaternion targetRotation;
	private float rotationSpeed = 1.0f;
	private float projectileSpeed = 10.0f;

	private bool parentAssigned = false;

	public Vector3 playerSpeed;
	public Vector3 movement;
	private Vector3 targetPoint;
	private GameObject targetObj;

	//private bool movementDirection = false;

	//public GameObject dot;

	//[SerializeField] private GameObject playerLockTarget;
	//private bool playerMissileLock;

	public void Initialise(RaycastHit inputHit) {
		// Set time of launch
		launchTime = 0.5f;
		launchTime = Time.time + launchTime;

		// Set direction for missile to initially face
		GameObject head = GameObject.FindGameObjectWithTag("CurrentWeapon").transform.GetChild(0).transform.GetChild(0).gameObject;
		GameObject tail = GameObject.FindGameObjectWithTag("CurrentWeapon").transform.GetChild(0).transform.GetChild(1).gameObject;
		Vector3 positionToLookAt = head.transform.position - tail.transform.position;
		transform.rotation = Quaternion.LookRotation(positionToLookAt);

		// Find the current velocity of the missile (based on relative velocity of player)
		playerSpeed = GameObject.FindGameObjectWithTag("Player").transform.GetChild(0).gameObject.GetComponent<Rigidbody>().velocity * Time.deltaTime;

		if (inputHit.transform.root.tag == "DragonFly") {
			targetObj = inputHit.transform.gameObject;
		} else {
			targetPoint = inputHit.point;
		}
	}

	private void Update() {
		if (targetObj) targetPoint = targetObj.transform.position;

		projectileSpeed += 0.75f;

		if (!parentAssigned) {
			gameObject.transform.parent = GameObject.FindGameObjectWithTag("MissileParent").transform;
			parentAssigned = true;
		}

		if (!launched && Time.time > launchTime) {
			launched = true;
		} else if (launched) {
			targetRotation = Quaternion.LookRotation(targetPoint - transform.position);
			transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

			rotationSpeed += Time.deltaTime * 10;
		}

		playerSpeed *= 0.99f;
		transform.position += playerSpeed;

		transform.position += Time.deltaTime * projectileSpeed * transform.forward;

		Debug.DrawLine(transform.position, targetPoint, Color.green);
	}

	private void Explode() {
		explosion = Instantiate(explosion);
		explosion.tag = "Explosion - Player";

		explosion.transform.position = transform.position;
		Destroy(gameObject);
	}

	private void OnCollisionEnter(Collision other) {
		if (other.transform.root.tag != "Player") Explode();

		//if (other.gameObject.tag == "Environment") {
		//	Explode();
		//} else {
		//	//// Get the root parent
		//	//Transform parent = other.gameObject.transform.root;
		//	//// Get all the colliders in the object
		//	//Collider[] colliders = parent.GetComponentsInChildren<Collider>();

		//	//// Get the index of the collider to see if it exists in the array
		//	//int DoesContain = System.Array.IndexOf(colliders, other.collider);

		//	//if (DoesContain >= 0 || other.gameObject.layer == 9 || other.gameObject.name == this.gameObject.name) {

		//	//} else {
		//	//	Explode();
		//	//}

		//	//if (other.gameObject.tag == "Bullet - Player") {
		//	//	Explode();
		//	//}
		//}
	}
}