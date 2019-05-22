using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour {

	[SerializeField] private GameObject missile;
	[SerializeField] private GameObject missileSpawnLocation;

	[SerializeField] private GameObject bomb;

	private Vector3 currentPosition;
	private Vector3 previousPosition;
	public Vector3 playerSpeed;

	public Vector3 playerSpeed;

	private float timeBetweenBombs = 1.5f;
	private float timeOfLastBomb;

	private float timeBetweenMissiles = 2f;
	private float timeOfLastMissile;

	private float timeBetweenFlares = 3f;
	private float timeOfLastFlare;

	void Start() {
		currentPosition = transform.GetChild(1).position;
		timeOfLastBomb = -timeBetweenBombs;
		timeOfLastMissile = -timeBetweenMissiles;
		timeOfLastFlare = -timeBetweenFlares;
	}

	void Update() {
		previousPosition = currentPosition;
		currentPosition = transform.GetChild(1).position;
		playerSpeed = currentPosition - previousPosition;

		if (!transform.GetChild(1).GetComponent<PlayerController>().isCruising) {
			if (GameObject.FindGameObjectWithTag("LeftSelect").GetComponent<WeaponSelect>().weaponNumber == 2) {
				missileSpawnLocation = GameObject.FindGameObjectWithTag("CurrentWeapon").transform.GetChild(0).transform.GetChild(0).gameObject;

				if (Input.GetButtonDown("Attack") && Time.time > timeOfLastMissile + timeBetweenMissiles) {
					timeOfLastMissile = Time.time;
					LaunchMissile();
				}
			} else if (GameObject.FindGameObjectWithTag("LeftSelect").GetComponent<WeaponSelect>().previousWeaponNumber == 2) {
				missileSpawnLocation = null;
			}

			if (Input.GetKeyDown(KeyCode.Q) && Time.deltaTime > timeOfLastFlare + timeBetweenFlares) {
				timeOfLastFlare = Time.time;
				LaunchFlare();
			}

		} else {
			if (Input.GetKeyDown(KeyCode.B) && Time.time > timeOfLastBomb + timeBetweenBombs) {
				timeOfLastBomb = Time.time;
				LaunchBomb();
			}
		}
	}

	private void LaunchMissile() {
		GameObject newMissile = Instantiate(missile);

		newMissile.transform.position = missileSpawnLocation.transform.position;

		//newMissile.transform.parent = GameObject.FindGameObjectWithTag("CurrentWeapon").transform;

		newMissile.GetComponent<MissileBehaviour>().Initialise(gameObject, Camera.main.transform.position);
		newMissile.GetComponent<MissileBehaviour>().playerSpeed = playerSpeed;

		print("Missile Launched");
	}

	private void LaunchBomb() {
		GameObject newBomb = Instantiate(bomb);

		Vector3 spawnLocation = transform.GetChild(1).position;
		spawnLocation.y -= 1.5f;
		newBomb.transform.position = spawnLocation;

		newBomb.GetComponent<BombBehaviour>().Initialise(currentPosition - previousPosition);
	}

	private void LaunchFlare() {
		print("Flare Launched");
	}
}
