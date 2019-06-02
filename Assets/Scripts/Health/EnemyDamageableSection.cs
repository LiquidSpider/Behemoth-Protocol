using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageableSection : MonoBehaviour {
	
	private Transform parent;

	private void Start() {
		parent = gameObject.transform.parent.parent;
	}

	private void OnCollisionEnter(Collision other) {

		if (other.gameObject.transform.tag == "Explosion - Player") {
			parent.GetComponent<EnemyHealth>().TakeDamage(200, other.gameObject);
		}

		if (other.gameObject.transform.tag == "Bullet - Player") {
			parent.GetComponent<EnemyHealth>().TakeDamage(10);
		}

        if(other.collider.gameObject.layer != 13)
		    parent.GetChild(1).GetComponent<ShowHealth>().DamageTaken();
	}
}
