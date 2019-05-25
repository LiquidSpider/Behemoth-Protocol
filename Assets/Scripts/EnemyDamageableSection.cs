using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageableSection : MonoBehaviour {

	private void OnCollisionEnter(Collision other) {
		if (other.gameObject.transform.tag == "Explosion - Player") {
			int childNum = gameObject.transform.GetSiblingIndex();

			Transform parent = transform.root;
			parent.GetComponent<EnemyHealth>().TakeDamage(200, other.gameObject);
		}

		if (other.gameObject.transform.tag == "Bullet - Player") {
			int childNum = gameObject.transform.GetSiblingIndex();

			Transform parent = transform.root;

			parent.GetComponent<EnemyHealth>().TakeDamage(10);
		}

		gameObject.transform.root.GetChild(1).GetComponent<ShowHealth>().DamageTaken();
	}
}
