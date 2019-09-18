using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyMissileBehaviour : MonoBehaviour {

	public GameObject explosion;

	private float flarDistractChance = 2.5f;

	private float launchTime = 6f;
	private bool launched = false;
	private Quaternion targetRotation;
	private float rotationSpeed = 0.2f;
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

		//StartCoroutine(AutoDestruct());
	}

	private void Update() {

		if (!hasExploded) {

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
		}

		Debug.DrawLine(transform.position, targetPoint, Color.green);
	}

	private void OnCollisionEnter(Collision other) {
		if (other.gameObject.transform.root.tag != "Enemy") Explode();
	}

	//private void OnTriggerEnter(Collider other) {
	//	if (other.gameObject.transform.root.tag != "Enemy") Explode();
	//}

	private bool hasExploded = false;

	private void Explode() {
		if (!hasExploded) {
			hasExploded = true;
			Destroy(gameObject.transform.GetChild(6).gameObject);

			explosion = Instantiate(explosion);
			explosion.tag = "Explosion - Enemy";

			explosion.transform.position = transform.position;

			gameObject.GetComponent<CapsuleCollider>().enabled = false;
			gameObject.transform.GetChild(0).gameObject.SetActive(false);

			ParticleSystem fire = gameObject.transform.GetChild(7).GetComponent<ParticleSystem>();
			ParticleSystem glow = gameObject.transform.GetChild(7).GetChild(0).GetComponent<ParticleSystem>();

			fire.Stop();
			glow.Stop();

			Exploder.explode(transform);

			StartCoroutine(TimedDestroy());
		}
	}

	private IEnumerator TimedDestroy() {
		yield return new WaitForSeconds(3.0f);
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

	private IEnumerator AutoDestruct() {
		yield return new WaitForSeconds(12.0f);
		Explode();
	}
}