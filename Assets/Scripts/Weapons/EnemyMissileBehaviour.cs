using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyMissileBehaviour : MonoBehaviour {

	public GameObject explosion;

	private float flarDistractChance = 2.5f;

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

	public void Distraction(GameObject inputFlare) {
		//Debug.Log("Distraction");

		float r = Random.Range(0f, 10f);

		if (r < flarDistractChance) {
			//Debug.Log("Distracted");
			playerObj = inputFlare;
			targetPoint = inputFlare.transform.position;
		}
	}

	private void OnTriggerLeave(Collider collider) {
		if (collider.gameObject.tag == "Environment") Explode();
	}
}