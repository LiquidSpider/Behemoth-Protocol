using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class missileLaunch : MonoBehaviour {

	public void fire() {
		StartCoroutine(firing(Random.Range(0f, 0.5f)));
	}

	private IEnumerator firing(float WaitTime) {
		yield return new WaitForSeconds(WaitTime);

		GameObject newMissile = Instantiate(transform.root.GetComponent<giantBehaviour>().missile);
		newMissile.transform.position = gameObject.transform.position;
		newMissile.transform.rotation = Quaternion.LookRotation(gameObject.transform.forward);
		newMissile.transform.GetChild(0).GetComponent<TrailRenderer>().material.color = Color.red;
		newMissile.GetComponent<EnemyMissileBehaviour>().Instantiate();
	}
}
