using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour {

	public float HP = 2500;
	private float maxHP;
	private bool critDamageTaken = false;

	public bool isScanning = false;

	public float battery = 10000;
	private float maxB;

	private List<GameObject> TakenDamageFrom = new List<GameObject>();

	private void Start() {
		maxHP = HP;
		maxB = battery; 
	}

	private void Update() {
		// Stop battery from going into negative
		if (battery < 0) battery = 0;

		if (HP < maxHP) {
			if (HP < maxB / 10f) {
				TakeDamage(-1f);
				UseBattery(30);
			} else {
				TakeDamage(-0.5f);
				UseBattery(15);
			}
		}

		if (battery < maxB) {
			UseBattery( -(10000f / 10f) * Time.deltaTime );
		}
		
		if (Input.GetMouseButtonDown(1)) {
			isScanning = true;
		} else if (Input.GetMouseButtonUp(1)) {
			isScanning = false;
		}

		if (isScanning) {
			if (battery < 500) isScanning = false;
			else UseBattery(10);
		}

	}

	private void TakeDamage(float damage) {
		HP -= damage;
		GameObject.FindGameObjectWithTag("PlayerHealthBar").GetComponent<Image>().fillAmount = HP / maxHP;
	}

	public void TakeDamage(float damage, GameObject explosion) {

		// Check if this object should only take damage once
		if (explosion) {

			if (!TakenDamageFrom.Contains(explosion)) {
				TakenDamageFrom.Add(explosion);

				HP -= damage;
				if (HP < 0)
					HP = 0;
			}

		} else // beam damage
		  {

			HP -= damage;
			if (HP < 0)
				HP = 0;

		}

		// Check health and handle accordingly
		if (HP <= 0 && !critDamageTaken) {
			critDamageTaken = true;
		} else if (HP <= 0 && critDamageTaken) {
			Die();
		}

		GameObject.FindGameObjectWithTag("PlayerHealthBar").GetComponent<Image>().fillAmount = HP / maxHP;
	}

	public void UseBattery(float reduction) {
		battery -= reduction;

		GameObject.FindGameObjectWithTag("PlayerBatteryBar").GetComponent<Image>().fillAmount = battery / maxB;
	}

	private void Die() {
		GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().PlayerLose();
	}
}
