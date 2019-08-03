using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageableSection : MonoBehaviour {
	
	public Transform parent;

	private void Start() {
        if(parent == null)
		    parent = gameObject.transform.parent.parent;
	}

	private void OnCollisionEnter(Collision other) {
		if (other.gameObject.transform.tag == "Bullet - Player") {
            parent.GetComponent<EnemyHealth>().TakeDamage(10);
		}

        if(other.collider.gameObject.layer != 13)
		    parent.GetChild(1).GetComponent<ShowHealth>().DamageTaken();
	}

	private void OnTriggerEnter(Collider other) {
		if (other.gameObject.transform.tag == "Explosion - Player" || other.gameObject.transform.tag == "Explosion - Enemy") {
			parent.GetComponent<EnemyHealth>().TakeDamage(200, other.gameObject);
		}
	}
}
