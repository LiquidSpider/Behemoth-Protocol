using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour {

	[SerializeField] private GameObject missile;
	[SerializeField] private GameObject missileSpawnLocation;

	private Vector3 currentPosition;
	private Vector3 previousPosition;

	void Start() {
		currentPosition = transform.position;
	}

	void Update() {
		previousPosition = currentPosition;
		currentPosition = transform.position;

		if (GameObject.FindGameObjectWithTag("LeftSelect").GetComponent<WeaponSelect>().weaponNumber == 3) {
			missileSpawnLocation = GameObject.FindGameObjectWithTag("CurrentWeapon").transform.GetChild(0).transform.GetChild(0).gameObject;

			if (Input.GetButtonDown("Attack")) {
				LaunchMissile();
			}
		} else if (GameObject.FindGameObjectWithTag("LeftSelect").GetComponent<WeaponSelect>().previousWeaponNumber == 3) {
			missileSpawnLocation = null;
		}
	}

	private void LaunchMissile() {
		GameObject newMissile = Instantiate(missile);

		newMissile.transform.position = missileSpawnLocation.transform.position;

		newMissile.transform.parent = GameObject.FindGameObjectWithTag("CurrentWeapon").transform;

		newMissile.GetComponent<MissileBehaviour>().playerSpeed = currentPosition - previousPosition;
		newMissile.GetComponent<MissileBehaviour>().Initialise(gameObject, Camera.main.transform.position);

		//newMissile.transform.parent = null;

		print("Missile Launched");
	}

	//private IEnumerator Fire() {
	//	LaunchMissile();

	//	yield return new WaitForSecondsRealtime(1);

	//	StartCoroutine(Fire());
	//}
}
