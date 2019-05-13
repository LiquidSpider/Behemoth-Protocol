using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour {

	[SerializeField] private GameObject missile;
	[SerializeField] private GameObject missileSpawnLocation;

	[SerializeField] private GameObject bomb;

	private Vector3 currentPosition;
	private Vector3 previousPosition;

	private float timeBetweenBombs = 0.01f;
	private float timeOfLastBomb;

	void Start() {
		currentPosition = transform.GetChild(1).position;
		timeOfLastBomb = -timeBetweenBombs;
	}

	void Update() {
		previousPosition = currentPosition;
		currentPosition = transform.GetChild(1).position;

		if (!transform.GetChild(1).GetComponent<PlayerController>().isCruising) {
			if (GameObject.FindGameObjectWithTag("LeftSelect").GetComponent<WeaponSelect>().weaponNumber == 3) {
				missileSpawnLocation = GameObject.FindGameObjectWithTag("CurrentWeapon").transform.GetChild(0).transform.GetChild(0).gameObject;

				if (Input.GetButtonDown("Attack")) {
					LaunchMissile();
				}
			} else if (GameObject.FindGameObjectWithTag("LeftSelect").GetComponent<WeaponSelect>().previousWeaponNumber == 3) {
				missileSpawnLocation = null;
			}
		}

		if (Input.GetKeyDown(KeyCode.B) && Time.time > timeOfLastBomb + timeBetweenBombs) {
			timeOfLastBomb = Time.time;
			LaunchBomb();
		}
	}

	private void LaunchMissile() {
		GameObject newMissile = Instantiate(missile);

		newMissile.transform.position = missileSpawnLocation.transform.position;

		//newMissile.transform.parent = GameObject.FindGameObjectWithTag("CurrentWeapon").transform;

		newMissile.GetComponent<MissileBehaviour>().playerSpeed = currentPosition - previousPosition;
		newMissile.GetComponent<MissileBehaviour>().Initialise(gameObject, Camera.main.transform.position);

		//newMissile.transform.parent = null;

		print("Missile Launched");
	}

	private void LaunchBomb() {
		GameObject newBomb = Instantiate(bomb);

		Vector3 spawnLocation = transform.GetChild(1).position;
		spawnLocation.y -= 1.5f;
		newBomb.transform.position = spawnLocation;

		newBomb.GetComponent<BombBehaviour>().Initialise(currentPosition - previousPosition);
	}
}
