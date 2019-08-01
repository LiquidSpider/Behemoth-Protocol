﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyMissileBehaviour : MonoBehaviour {

	public GameObject explosion;

	private float launchTime = 1f;
	private bool launched = false;
	private Quaternion targetRotation;
	private float rotationSpeed = 1.0f;
	private float projectileSpeed = 10.0f;

	private bool parentAssigned = false;
	
	private Vector3 targetPoint;
	private GameObject playerObj;

	private RaycastHit hit;
	private bool playerReached = false;
	private bool noTargetFound = false;

	private float timer = 1f;


	public void Instantiate() {
		gameObject.transform.localScale = new Vector3(10, 10, 10);

		launchTime = Time.time + launchTime;

		playerObj = GameObject.FindGameObjectWithTag("Player").transform.GetChild(0).gameObject;
	}

	private void Update() {

		if (!playerReached && Vector3.Distance(targetPoint, transform.position) < 10) {
			playerReached = true;

			if (Physics.Raycast(transform.position, transform.forward, out hit)) {
				targetPoint = hit.point;
			} else {
				noTargetFound = true;
				timer = Time.time + timer;
			}
		} else if (!playerReached) {
			targetPoint = playerObj.transform.position;
		}


		if (noTargetFound && timer < Time.time) {
			Explode();
		}


		projectileSpeed += 0.25f;

		if (!parentAssigned) {
			gameObject.transform.parent = GameObject.FindGameObjectWithTag("MissileParent").transform;
			parentAssigned = true;
		}

		if (!launched && Time.time > launchTime) {
			launched = true;
		} else if (launched && !playerReached) {
			targetRotation = Quaternion.LookRotation(targetPoint - transform.position);
			transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

			rotationSpeed += Time.deltaTime * 10;
		}

		transform.position += Time.deltaTime * projectileSpeed * transform.forward;

		Debug.DrawLine(transform.position, targetPoint, Color.green);
	}

	private void OnCollisionEnter(Collision other) {
		if (other.gameObject.transform.root.tag != "Enemy") Explode();
	}

	private void Explode() {
		explosion = Instantiate(explosion);
		explosion.tag = "Explosion - Enemy";

		explosion.transform.position = transform.position;
		Destroy(gameObject);
	}

	//[SerializeField] private Vector3 direction;

	//public GameObject owner;
	//public GameObject player;
	//private Vector3 target;

	//private List<Vector3> previousLocations = new List<Vector3>();

	//private float launchTime = 2f;
	//private bool launched = false;
	//private Quaternion targetRotation;
	//private float rotationSpeed = 1.0f;
	//private float projectileSpeed = 10.0f;

	//private bool first = true;

	//public Vector3 playerSpeed;
	//public Vector3 movement;

	//private bool movementDirection = false;

	//public GameObject dot;

	//[SerializeField] private GameObject playerLockTarget;
	//private bool playerMissileLock;

	//public void Initialise(GameObject inputOwner, Vector3 inputTarget) {
	//	launchTime = Time.time + launchTime;

	//	owner = inputOwner;

	//	GameObject head = GameObject.FindGameObjectWithTag("CurrentWeapon").transform.GetChild(0).transform.GetChild(0).gameObject;
	//	GameObject tail = GameObject.FindGameObjectWithTag("CurrentWeapon").transform.GetChild(0).transform.GetChild(1).gameObject;

	//	Vector3 positionToLookAt = head.transform.position - tail.transform.position;

	//	transform.rotation = Quaternion.LookRotation(positionToLookAt);

	//	playerSpeed = owner.GetComponent<WeaponController>().playerSpeed;

	//	launchTime = 0.1f;
	//	//playerSpeed = GameObject.FindGameObjectWithTag("Player").transform.GetChild(1).gameObject.GetComponent<Rigidbody>().velocity;
	//}

	//public void Initialise(GameObject inputOwner, GameObject inputTarget, bool missileLock) {
	//	launchTime = Time.time + launchTime;

	//	owner = inputOwner;

	//	GameObject head = GameObject.FindGameObjectWithTag("CurrentWeapon").transform.GetChild(0).transform.GetChild(0).gameObject;
	//	GameObject tail = GameObject.FindGameObjectWithTag("CurrentWeapon").transform.GetChild(0).transform.GetChild(1).gameObject;

	//	Vector3 positionToLookAt = head.transform.position - tail.transform.position;

	//	transform.rotation = Quaternion.LookRotation(positionToLookAt);

	//	playerSpeed = owner.GetComponent<WeaponController>().playerSpeed;

	//	launchTime = 0.1f;

	//	playerLockTarget = inputTarget;
	//	playerMissileLock = missileLock;
	//}


	//public void Initialise(GameObject inputOwner, GameObject spawner) {
	//	launchTime = Time.time + launchTime;

	//	owner = inputOwner;

	//	// make sure spawner exists
	//	if (spawner) {
	//		GameObject head = spawner.transform.GetChild(0).transform.GetChild(0).gameObject;
	//		GameObject tail = spawner.transform.GetChild(0).transform.GetChild(1).gameObject;

	//		Vector3 positionToLookAt = head.transform.position - tail.transform.position;

	//		transform.rotation = Quaternion.LookRotation(positionToLookAt);
	//	}
	//}

	//private void Update() {
	//	RaycastHit ray;

	//	if (Physics.Raycast(transform.position, transform.forward, out ray, 2f)) {
	//		if (Time.time > launchTime) {
	//			Explode();
	//		}
	//	} else {
	//		if (owner == null) {
	//			explosion = Instantiate(explosion);
	//			explosion.tag = "Explosion - Enemy";
	//			explosion.transform.position = transform.position;
	//			Destroy(gameObject);
	//		} else {
	//			projectileSpeed += 0.75f;

	//			if (!launched && Time.time > launchTime) {
	//				launched = true;

	//				if (owner.tag == "Player") {
	//					if (!playerMissileLock) {
	//						RaycastHit hit;

	//						direction = owner.transform.GetChild(1).transform.forward;
	//						movement = transform.position;

	//						if (Physics.Raycast(transform.position, owner.transform.GetChild(1).transform.forward, out hit)) {
	//							target = hit.point;
	//						}
	//					} else {
	//						target = playerLockTarget.transform.position;
	//					}
	//				} else if (owner.tag == "Enemy") {
	//					target = player.transform.position;
	//				} else {
	//					target = Vector3.zero;
	//				}
	//			} else if (launched && target != null) {
	//				if (first) {
	//					gameObject.transform.parent = GameObject.FindGameObjectWithTag("MissileParent").transform;
	//					first = false;
	//				}

	//				if (owner.tag == "Player" || movementDirection == true) {
	//					if (!playerMissileLock) {
	//						RaycastHit hit;

	//						if (Physics.Raycast(movement, direction, out hit)) {
	//							target = hit.point;
	//						}
	//					} else {
	//						if (Vector3.Magnitude(transform.position - playerLockTarget.transform.position) < 15.0f) {
	//							movementDirection = true;

	//							direction = transform.forward;
	//							movement = transform.position;
	//						}

	//						target = playerLockTarget.transform.position;
	//					}
	//				} else if (owner.tag == "Enemy") {
	//					if (Vector3.Magnitude(transform.position - player.transform.position) < 15.0f) {
	//						movementDirection = true;

	//						direction = transform.forward;
	//						movement = transform.position;
	//					}

	//					target = player.transform.position;
	//				} else {
	//					target = Vector3.zero;
	//				}

	//				targetRotation = Quaternion.LookRotation(target - transform.position);

	//				transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

	//				rotationSpeed += Time.deltaTime * 10;
	//			}

	//			if (owner.tag == "Player") {
	//				playerSpeed *= 0.99f;
	//				transform.position += playerSpeed;
	//			}

	//			transform.position += Time.deltaTime * projectileSpeed * transform.forward;

	//			Debug.DrawLine(transform.position, target, Color.green);

	//			if (owner.tag == "Enemy" && dot) {
	//				if (Vector3.Dot(transform.position - Camera.main.transform.position, Camera.main.transform.forward) > 0) {
	//					dot.SetActive(true);
	//					Vector3 position = Camera.main.WorldToScreenPoint(transform.position) - new Vector3(Screen.width / 2f, Screen.height / 2f, 0);

	//					if (position.magnitude > 500) {
	//						dot.GetComponent<RectTransform>().localPosition = ( Camera.main.WorldToScreenPoint(transform.position) - new Vector3(Screen.width / 2f, Screen.height / 2f, 0) ).normalized * 500;
	//					} else {
	//						dot.GetComponent<RectTransform>().localPosition = Camera.main.WorldToScreenPoint(transform.position) - new Vector3(Screen.width / 2f, Screen.height / 2f, 0);
	//					}
	//				} else {
	//					dot.SetActive(false);
	//					//dot.GetComponent<RectTransform>().localPosition = ( Camera.main.WorldToScreenPoint(transform.position) - new Vector3(Screen.width / 2f, Screen.height / 2f, 0) ).normalized * -500;
	//				}
	//			}
	//		}
	//	}
	//}

	//private void OnCollisionEnter(Collision other) {
	//	if (other.gameObject.tag == "Environment") {
	//		Explode();
	//	} else {
	//		// Get the root parent
	//		Transform parent = owner.gameObject.transform.root;
	//		// Get all the colliders in the object
	//		Collider[] colliders = parent.GetComponentsInChildren<Collider>();

	//		// Get the index of the collider to see if it exists in the array
	//		int DoesContain = System.Array.IndexOf(colliders, other.collider);

	//		if (DoesContain >= 0 || other.gameObject.layer == 9 || other.gameObject.name == this.gameObject.name) {

	//		} else {
	//			Explode();
	//		}

	//		if (other.gameObject.tag == "Bullet - Player") {
	//			Explode();
	//		}
	//	}
	//}
}