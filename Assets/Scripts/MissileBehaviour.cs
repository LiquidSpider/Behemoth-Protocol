using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileBehaviour : MonoBehaviour {

	public GameObject explosion;
	
	[SerializeField] private Vector3 direction;

	public GameObject owner;
	public GameObject player;
	private Vector3 target;

	private List<Vector3> previousLocations = new List<Vector3>();

	private float launchTime = 0.1f;
	private bool launched = false;
	private Quaternion targetRotation;
	private float rotationSpeed = 1.0f;
	private float projectileSpeed = 10.0f;

	private bool first = true;

	public Vector3 playerSpeed;
	public Vector3 movement;

	private bool movementDirection = false;

	public void Initialise(GameObject inputOwner, Vector3 inputTarget) {
		launchTime = Time.time + launchTime;

		owner = inputOwner;

		GameObject head = GameObject.FindGameObjectWithTag("CurrentWeapon").transform.GetChild(0).transform.GetChild(0).gameObject;
		GameObject tail = GameObject.FindGameObjectWithTag("CurrentWeapon").transform.GetChild(0).transform.GetChild(1).gameObject;

		Vector3 positionToLookAt = head.transform.position - tail.transform.position;

		transform.rotation = Quaternion.LookRotation(positionToLookAt);

		playerSpeed = owner.GetComponent<WeaponController>().playerSpeed;
		//playerSpeed = GameObject.FindGameObjectWithTag("Player").transform.GetChild(1).gameObject.GetComponent<Rigidbody>().velocity;
	}

	public void Initialise(GameObject inputOwner) {
		launchTime = Time.time + launchTime;

		owner = inputOwner;
	}

	private void Update() {
		if (owner == null) {
			explosion = Instantiate(explosion);
			explosion.tag = "Explosion - Enemy";
			explosion.transform.position = transform.position;
			Destroy(gameObject);
		} else {

			projectileSpeed += 0.75f;

			if (!launched && Time.time > launchTime) {
				launched = true;

				if (owner.tag == "Player") {
					RaycastHit hit;

					direction = owner.transform.GetChild(1).transform.forward;
					movement = transform.position;

					if (Physics.Raycast(transform.position, owner.transform.GetChild(1).transform.forward, out hit)) {
						target = hit.point;
					}
				} else if (owner.tag == "Enemy") {
					target = player.transform.position;
				} else {
					target = Vector3.zero;
				}
			} else if (launched && target != null) {
				if (first) {
					gameObject.transform.parent = GameObject.FindGameObjectWithTag("MissileParent").transform;
					first = false;
				}

				if (owner.tag == "Player" || movementDirection == true) {
					RaycastHit hit;

					if (Physics.Raycast(movement, direction, out hit)) {
						target = hit.point;
					}
				} else if (owner.tag == "Enemy") {
					if (Vector3.Magnitude(transform.position - player.transform.position) < 15.0f) {
						movementDirection = true;

						direction = transform.forward;
						movement = transform.position;
					}

					target = player.transform.position;
				} else {
					target = Vector3.zero;
				}

				targetRotation = Quaternion.LookRotation(target - transform.position);

				transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

				rotationSpeed += Time.deltaTime * 10;
			}

			if (owner.tag == "Player") {
				playerSpeed *= 0.99f;
				transform.position += playerSpeed;
			}

			transform.position += Time.deltaTime * projectileSpeed * transform.forward;

			Debug.DrawLine(transform.position, target, Color.green);
		}
	}

	private void OnCollisionEnter(Collision other) {
		// Get the root parent
		Transform parent = owner.gameObject.transform.root;
		// Get all the colliders in the object
		Collider[] colliders = parent.GetComponentsInChildren<Collider>();

		// Get the index of the collider to see if it exists in the array
		int DoesContain = System.Array.IndexOf(colliders, other.collider);

		if (DoesContain >= 0 || other.gameObject.layer == 9 || other.gameObject.name == this.gameObject.name) {

		} else {
			explosion = Instantiate(explosion);

			if (owner.tag == "Player") {
				explosion.tag = "Explosion - Player";
			} else {
				explosion.tag = "Explosion - Enemy";
			}
            explosion.transform.position = transform.position;
			Destroy(gameObject);
		}


		if (other.gameObject.tag == "Bullet - Player") {
			explosion = Instantiate(explosion);

			if (owner.tag == "Player") {
				explosion.tag = "Explosion - Player";
			} else {
				explosion.tag = "Explosion - Enemy";
			}

			explosion.transform.position = transform.position;
			Destroy(gameObject);
		}
	}
    
}