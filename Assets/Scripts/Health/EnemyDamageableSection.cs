﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageableSection : MonoBehaviour {

	private int childNum;
	private Transform parent;

	private void Start() {
		childNum = gameObject.transform.parent.GetSiblingIndex();
		parent = transform.root.GetChild(childNum);
	}

	private void OnCollisionEnter(Collision other) {

		if (other.gameObject.transform.tag == "Explosion - Player") {
			parent.GetComponent<EnemyHealth>().TakeDamage(200, other.gameObject);
		}

		if (other.gameObject.transform.tag == "Bullet - Player") {
			parent.GetComponent<EnemyHealth>().TakeDamage(10);
		}

		parent.GetChild(1).GetComponent<ShowHealth>().DamageTaken();
	}
}