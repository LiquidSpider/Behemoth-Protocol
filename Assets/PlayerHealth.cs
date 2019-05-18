using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour {

	public float HP = 1000;
	private bool critDamageTaken = false;

	private void Start() {
		HP = 1000;
	}

	public void TakeDamage(float damage) {
		HP -= damage;

		if (HP <= 0 && !critDamageTaken) {
			critDamageTaken = true;
		} else if (HP <= 0 && critDamageTaken) {
			Die();
		}
	}

	private void Die() {
		print("Player Killed");
	}
}
