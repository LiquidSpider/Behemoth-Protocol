using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamageableSection : MonoBehaviour {

	private void OnCollisionEnter(Collision other) {
		if (other.gameObject.transform.tag == "Explosion - Enemy") {
			Transform parent = transform.root;

			parent.GetComponent<PlayerHealth>().TakeDamage(100);
		}
	}
}
