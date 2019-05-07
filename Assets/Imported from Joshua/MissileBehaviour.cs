using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileBehaviour : MonoBehaviour {

	[SerializeField] private float variationFactor = 5;
	[SerializeField] private Vector3 direction;

	private GameObject owner;
	private GameObject player;
	private Vector3 target;

	private List<Vector3> previousLocations = new List<Vector3>();

	private float launchTime = 0.1f;
	private bool launched = false;
	private Quaternion targetRotation;
	private float rotationSpeed = 1.0f;
	private float projectileSpeed = 10.0f;

	private bool first = true;

	public Vector3 playerSpeed;

	public void Initialise(GameObject inputOwner, Vector3 inputTarget) {
		launchTime = Time.time + launchTime;

		owner = inputOwner;

		GameObject head = GameObject.FindGameObjectWithTag("CurrentWeapon").transform.GetChild(0).transform.GetChild(0).gameObject;
		GameObject tail = GameObject.FindGameObjectWithTag("CurrentWeapon").transform.GetChild(0).transform.GetChild(1).gameObject;

		Vector3 positionToLookAt = head.transform.position - tail.transform.position;

		transform.rotation = Quaternion.LookRotation(positionToLookAt);

		//float scaleFactor = ScaleFactor(positionToLookAt.normalized, playerSpeed.normalized);

		//projectileSpeed += Vector3.Magnitude(playerSpeed); // * scaleFactor;
	}

	public void Initialise(GameObject inputOwner) {
		launchTime = Time.time + launchTime;

		owner = inputOwner;
	}

	private void Update() {
		projectileSpeed += 0.5f;

		if (!launched && Time.time > launchTime) {
			launched = true;

			RaycastHit hit;

			if (owner.tag == "Player") {
				if (Physics.Raycast(transform.position, transform.forward, out hit)) {
					target = hit.point;
				}
			} else if (owner.tag == "Enemy") {
				target = player.transform.position;
			} else {
				target = transform.forward * 2000000;
			}
		} else if ( launched && target != null ) {
			if (first) {
				gameObject.transform.parent = GameObject.FindGameObjectWithTag("MissileParent").transform;
				//projectileSpeed += 20f;
				first = false;
			}

			targetRotation = Quaternion.LookRotation(target - transform.position);

			transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

			rotationSpeed += Time.deltaTime * 10;
		}

		transform.position += Time.deltaTime * projectileSpeed * transform.forward;
	}

	private void OnCollisionEnter(Collision other) {
		Destroy(gameObject);
	}

	private Quaternion PointForward(Quaternion inputDirection) {
		Vector3 tempRotation = inputDirection.eulerAngles;

		float tempZ = tempRotation.z;
		tempRotation.z = tempRotation.y;
		tempRotation.y = -tempZ;


		return Quaternion.Euler(tempRotation);
	}

	private float ScaleFactor(Vector3 inputOne, Vector3 inputTwo) {

		float numerator = Vector3.Dot(inputOne, inputTwo);

		if (numerator >= 0) {
			return -1;
		} else {
			return 1;
		}
	}
}