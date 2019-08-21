using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMissileTargetBehaviour : MonoBehaviour {

	public GameObject associatedMissile;

	public void Initialise(GameObject missile) {
		associatedMissile = missile;
	}

	private void OnDestory() {
		associatedMissile.GetComponent<PlayerMissileBehaviour>().RecalculateTrajectory();
	}
}
