using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileBehaviour : MonoBehaviour {

	public GameObject explosion;

	[SerializeField] private float variationFactor = 5;
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

	public void Initialise(GameObject inputOwner, Vector3 inputTarget) {
		launchTime = Time.time + launchTime;

		owner = inputOwner;

		GameObject head = GameObject.FindGameObjectWithTag("CurrentWeapon").transform.GetChild(0).transform.GetChild(0).gameObject;
		GameObject tail = GameObject.FindGameObjectWithTag("CurrentWeapon").transform.GetChild(0).transform.GetChild(1).gameObject;

		Vector3 positionToLookAt = head.transform.position - tail.transform.position;

		transform.rotation = Quaternion.LookRotation(positionToLookAt);

		playerSpeed = owner.GetComponent<WeaponController>().playerSpeed;
	}

	public void Initialise(GameObject inputOwner) {
		launchTime = Time.time + launchTime;

		owner = inputOwner;
	}

	private void Update() {
		projectileSpeed += 0.25f;

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

			if (owner.tag == "Player") {
				RaycastHit hit;

				if (Physics.Raycast(movement, direction, out hit)) {
					target = hit.point;
				}
			} else if (owner.tag == "Enemy") {
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
			explosion.tag = "Explosion";
			explosion.transform.position = transform.position;
			Destroy(gameObject);
		}

	}

	private Quaternion PointForward(Quaternion inputDirection) {
		Vector3 tempRotation = inputDirection.eulerAngles;

		float tempZ = tempRotation.z;
		tempRotation.z = tempRotation.y;
		tempRotation.y = -tempZ;


		return Quaternion.Euler(tempRotation);
	}
}

//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class MissileBehaviour : MonoBehaviour {

//	public GameObject explosion;

//	[SerializeField] private float variationFactor = 5;
//	[SerializeField] private Vector3 direction;

//	private GameObject owner;
//	private GameObject player;
//	private Vector3 target;

//	private List<Vector3> previousLocations = new List<Vector3>();

//	private float launchTime = 0.1f;
//	private bool launched = false;
//	private Quaternion targetRotation;
//	private float rotationSpeed = 1.0f;
//	private float projectileSpeed = 10.0f;

//	private bool first = true;

//	public Vector3 playerSpeed;
//	public Vector3 movement;

//	public void Initialise(GameObject inputOwner, Vector3 inputTarget) {
//		launchTime = Time.time + launchTime;

//		owner = inputOwner;

//		GameObject head = GameObject.FindGameObjectWithTag("CurrentWeapon").transform.GetChild(0).transform.GetChild(0).gameObject;
//		GameObject tail = GameObject.FindGameObjectWithTag("CurrentWeapon").transform.GetChild(0).transform.GetChild(1).gameObject;

//		Vector3 positionToLookAt = head.transform.position - tail.transform.position;

//		transform.rotation = Quaternion.LookRotation(positionToLookAt);

//		playerSpeed = GameObject.FindGameObjectWithTag("Player").transform.GetChild(1).gameObject.GetComponent<Rigidbody>().velocity;
//	}

//	public void Initialise(GameObject inputOwner) {
//		launchTime = Time.time + launchTime;

//		owner = inputOwner;
//	}

//	private void Update() {
//		projectileSpeed += 0.25f;

//		if (!launched && Time.time > launchTime) {
//			launched = true;

//			// Change the direction to where the crosshair is aiming at
//			//direction = Camera.main.transform.forward;
//			direction = owner.transform.GetChild(1).forward;

//		} else if (launched && target != null) {
//			if (first) {
//				gameObject.transform.parent = GameObject.FindGameObjectWithTag("MissileParent").transform;
//				first = false;
//			}

//			RaycastHit hit;
//			if (owner.tag == "Player") {
//				if (Physics.Raycast(owner.transform.position, direction, out hit)) {
//					target = hit.point;
//				}
//			} else if (owner.tag == "Enemy") {
//				target = player.transform.position;
//			} else {
//				target = Vector3.zero;
//			}

//			targetRotation = Quaternion.LookRotation(target - transform.position);

//			transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

//			rotationSpeed += Time.deltaTime * 10;
//		}

//		//transform.position += (target - transform.position) * Time.deltaTime + (playerSpeed / projectileSpeed) * Time.deltaTime;
//		Debug.DrawLine(transform.position, target, Color.blue);

//		playerSpeed *= 0.95f;
//		transform.position += playerSpeed * Time.deltaTime;
//		transform.position += Time.deltaTime * projectileSpeed * transform.forward;
//	}

//	private void OnCollisionEnter(Collision other) {
//		if (other.gameObject.tag == "Damageable") {
//			other.gameObject.GetComponent<Health>().TakeDamage(15);
//		}

//		explosion = Instantiate(explosion);
//		explosion.transform.position = transform.position;
//		Destroy(gameObject);
//	}

//	private Quaternion PointForward(Quaternion inputDirection) {
//		Vector3 tempRotation = inputDirection.eulerAngles;

//		float tempZ = tempRotation.z;
//		tempRotation.z = tempRotation.y;
//		tempRotation.y = -tempZ;


//		return Quaternion.Euler(tempRotation);
//	}
//}