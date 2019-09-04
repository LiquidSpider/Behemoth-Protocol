using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMissileTargetBehaviour : MonoBehaviour {

	public GameObject associatedMissile;

	private void Update() {
		if (!associatedMissile) Destroy(gameObject);
	}

	public void Initialise(GameObject missile) {
		associatedMissile = missile;
	}

	private void OnDestory() {
		if (associatedMissile) associatedMissile.GetComponent<PlayerMissileBehaviour>().RecalculateTrajectory();
	}
}
