using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour {

	public float HP = 250;
	public float maxHP;
	public GameObject smoke;

	private List<GameObject> damageTakenFrom = new List<GameObject>();

	private void Start() {
		maxHP = HP;
		smoke = transform.GetChild(4).gameObject;
	}

	public void TakeDamage(float damage) {
		HP -= damage;

		if (HP <= 0) {
			Die();
		}
		
		smoke.SetActive(true);
	}

	public void TakeDamage(float damage, GameObject explosion) {
		if (!damageTakenFrom.Contains(explosion)) {
			damageTakenFrom.Add(explosion);

			HP -= damage;

			if (HP <= 0) {
				Die();
			}

			smoke.SetActive(true);
		}
	}

	private void Die() {
		this.gameObject.GetComponent<DragonFly>().ToDie();
	}
}
