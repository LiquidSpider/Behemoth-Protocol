﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealth : MonoBehaviour {

	public float HP = 3000;
	public float maxHP;

	private List<GameObject> damageTakenFrom = new List<GameObject>();

	private void Start() {
		maxHP = HP;
	}

	public void TakeDamage(float damage) {
		HP -= damage;

		if (HP <= 0) {
			Die();
		}
	}

	public void TakeDamage(float damage, GameObject explosion) {
		if (!damageTakenFrom.Contains(explosion)) {
			HP -= damage;

			if (HP <= 0) {
				Die();
			}
		}
	}

	private void Die() {
		//Destroy(gameObject);
		GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().PlayerWin();
	}
}