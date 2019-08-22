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
	private Vector3 targetPoint;
	private GameObject targetObj;

	private bool targetReached = false;
	private bool noTargetFound = false;

	private float timer = 1f;
	private RaycastHit hit;
	private RaycastHit hitTarget;

	public GameObject targetObject;

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
		playerSpeed = GameObject.FindGameObjectWithTag("Player").transform.gameObject.GetComponent<Rigidbody>().velocity * Time.deltaTime;

		hitTarget = inputHit;
		RecalculateTrajectory();

		//if (inputHit.transform.root.tag == "DragonFly") {
		//	targetObj = inputHit.transform.gameObject;
		//} else {
		//	targetPoint = inputHit.point;
		//}
	}

	private void Update() {
		if (!hasExploded && targetObject) {
			if (!targetReached && Vector3.Distance(targetPoint, transform.position) < 3f) {
				targetReached = true;

				if (Physics.Raycast(transform.position, transform.forward, out hitTarget)) {
					RecalculateTrajectory();
				} else {
					noTargetFound = true;
					timer = Time.time + timer;
				}
			} else if (!targetReached) {
				targetPoint = targetObject.transform.position;
			}

			if (noTargetFound && Time.time > timer) {
				Explode();
			}


			projectileSpeed += 0.75f;

			if (!parentAssigned) {
				gameObject.transform.parent = GameObject.FindGameObjectWithTag("MissileParent").transform;
				parentAssigned = true;
			}

			if (!launched && Time.time > launchTime) {
				launched = true;

				gameObject.GetComponent<CapsuleCollider>().enabled = true;
			} else if (launched && !targetReached) {
				targetRotation = Quaternion.LookRotation(targetPoint - transform.position);
				transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

				rotationSpeed += Time.deltaTime * 10;
			}

			playerSpeed *= 0.99f;
			transform.position += playerSpeed;

			transform.position += Time.deltaTime * projectileSpeed * transform.forward;

			Debug.DrawLine(transform.position, targetPoint, Color.green);
		} else if (!targetObject){
			RecalculateTrajectory();
		}

		//if (hasExploded && !explosion) Destroy(gameObject);
	}

	public bool hasExploded = false;

	private void Explode() {
		if (!hasExploded) {
			hasExploded = true;

			Destroy(targetObject);

			GameObject temp = Instantiate(explosion);

			explosion = temp;
			explosion.tag = "Explosion - Player";
			explosion.transform.position = transform.position;

			ParticleSystem fire = gameObject.transform.GetChild(6).GetComponent<ParticleSystem>();
			ParticleSystem glow = gameObject.transform.GetChild(6).GetChild(0).GetComponent<ParticleSystem>();

			fire.Stop();
			glow.Stop();

			gameObject.GetComponent<CapsuleCollider>().enabled = false;

			Exploder.explode(transform);

			StartCoroutine(TimedDestroy());
		}
	}

	private IEnumerator TimedDestroy() {
		yield return new WaitForSeconds(3.0f);
		Destroy(gameObject);
	}

	private void OnTriggerLeave(Collider collider) {
		if (collider.gameObject.tag == "Environment") Explode();
	}

	private void OnCollisionEnter(Collision other) {
        if (other.transform.root.tag != "Player")
        {
            // If the object has base health
            if (other.transform.root.GetComponent<BaseHealth>())
            {
                BaseHealth[] healths = other.transform.root.GetComponents<BaseHealth>();
                if (healths.Length > 1)
                {
                    foreach (BaseHealth health in healths)
                    {
                        // If the health that has the same layer as the collided object
                        if (health.healthLayer == 1 << other.collider.gameObject.layer)
                        {
                            health.TakeDamage(200);
                        }
                    }
                }
                else
                {
                    healths[0].TakeDamage(200);
                }
            }

            Explode();
        }
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

	public void RecalculateTrajectory() {
		targetObject = new GameObject();
		targetObject.name = "Missile Track Location";
		targetObject.transform.position = hitTarget.point;
		targetObject.transform.localScale = new Vector3(2, 2, 2);
		targetObject.transform.parent = hitTarget.transform;
		targetObject.AddComponent<PlayerMissileTargetBehaviour>();
		targetObject.AddComponent<SphereCollider>();
		targetObject.GetComponent<PlayerMissileTargetBehaviour>().Initialise(gameObject);
	}
}