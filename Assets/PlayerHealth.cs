using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour {

	public float HP = 1000;
	private bool critDamageTaken = false;


	public float battery = 10000;

	private List<GameObject> TakenDamageFrom = new List<GameObject>();

	private void Start() {
		HP = 1000;
	}

	private void Update() {
		if (HP < 1000) {
			HP += 0.5f;
			battery -= 5;

			GameObject.FindGameObjectWithTag("PlayerHealthBar").GetComponent<Image>().fillAmount = HP / 1000f;
			GameObject.FindGameObjectWithTag("PlayerBatteryBar").GetComponent<Image>().fillAmount = battery / 10000f;
		}

		if (battery < 10000) {
			battery += 1f;
			GameObject.FindGameObjectWithTag("PlayerBatteryBar").GetComponent<Image>().fillAmount = battery / 10000f;
		}
	}

	public void TakeDamage(float damage, GameObject explosion) {
		if (!TakenDamageFrom.Contains(explosion)) {
			TakenDamageFrom.Add(explosion);

			HP -= damage;

			if (HP <= 0 && !critDamageTaken) {
				critDamageTaken = true;
			} else if (HP <= 0 && critDamageTaken) {
				Die();
			}
		}

		GameObject.FindGameObjectWithTag("PlayerHealthBar").GetComponent<Image>().fillAmount = battery / 1000f;
	}

	public void UseBattery(float reduction) {
		battery -= reduction;

		GameObject.FindGameObjectWithTag("PlayerBatteryBar").GetComponent<Image>().fillAmount = battery / 10000f;
	}

	private void Die() {
		print("Player Killed");
	}
}
