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
	public float maxB;

	private List<GameObject> TakenDamageFrom = new List<GameObject>();

	private void Start() {
		maxHP = HP;
		maxB = battery;
	}

	private void Update() {
		// Stop battery from going into negative
		if (battery < 0) battery = 0;

		if (HP < maxHP && battery > 0) {
			if (HP < maxHP * 0.1f) {
				TakeDamage(-1f);
				UseBattery(100 * Time.deltaTime);
			} else {
				TakeDamage(-0.5f);
				UseBattery(50 * Time.deltaTime);
			}
		}

		//if (battery < maxB) {
		//	AddBattery( (10000f / 10f) * Time.deltaTime );
		//}

		if (Input.GetMouseButtonDown(1)) {
			isScanning = true;
		} else if (Input.GetMouseButtonUp(1)) {
			isScanning = false;
		}

		if (isScanning) {
			if (battery < 500) isScanning = false;
			else UseBattery(60 * Time.deltaTime);
		}

		GameObject.FindGameObjectWithTag("PlayerHealthBar").GetComponent<Image>().fillAmount = HP / maxHP;
		GameObject.FindGameObjectWithTag("PlayerBatteryBar").GetComponent<Image>().fillAmount = battery / maxB;


		//  Battery Bar Colour Change
		Color temp;

		if (battery / maxB > 0.75f) {
			temp = Color.cyan;
		} else if (battery / maxB > 0.5f) {
			temp = Color.green;
		} else if (battery / maxB > 0.25f) {
			temp = Color.yellow;
		} else {
			temp = Color.red;
		}

		temp.a = 100f / 255f;
		GameObject.FindGameObjectWithTag("PlayerBatteryBar").GetComponent<Image>().color = temp;
	}

	public void UseBattery(float reduction) {
		battery -= reduction;
		//GameObject.FindGameObjectWithTag("PlayerBatteryBar").GetComponent<Image>().fillAmount = battery / maxB;
	}

	/// <summary>
	/// Adds an amount to the battery.
	/// </summary>
	/// <param name="amount">Amount to add.</param>
	public void AddBattery(float amount) {

		// ensure we don't over max the battery amount
		if (this.battery + amount >= maxB) {
			battery = maxB;
		} else {
			battery += amount;
		}

	}

	private void TakeDamage(float damage) {

		if (gameObject.transform.root.GetComponent<ShieldBehaviour>().shieldActive == true) {
			HP -= damage * 0.5f;
			UseBattery(damage * 0.5f);
		} else {
			HP -= damage;
		}

		
		//GameObject.FindGameObjectWithTag("PlayerHealthBar").GetComponent<Image>().fillAmount = HP / maxHP;
	}

	public void TakeDamage(float damage, GameObject explosion) {

		// Check if this object should only take damage once
		if (explosion) {

			if (!TakenDamageFrom.Contains(explosion)) {
				TakenDamageFrom.Add(explosion);

				if (gameObject.transform.root.GetComponent<ShieldBehaviour>().shieldActive == true) {
					HP -= damage * 0.5f;
					UseBattery(damage * 0.5f);
				} else {
					HP -= damage;
				}

				if (HP < 0) HP = 0;
			}

		} else {
			// Beam Damage

			if (gameObject.transform.root.GetComponent<ShieldBehaviour>().shieldActive == true) {
				HP -= damage * 0.5f;
				UseBattery(damage * 0.5f);
			} else {
				HP -= damage;
			}

			if (HP < 0) HP = 0;

		}

		// Check health and handle accordingly
		if (HP <= 0 && !critDamageTaken) {
			critDamageTaken = true;





			// --------------------------------------------------------------------------------------------------- Navigator Prompt Here 





		} else if (HP <= 0 && critDamageTaken) {
			Die();
		}

		//GameObject.FindGameObjectWithTag("PlayerHealthBar").GetComponent<Image>().fillAmount = HP / maxHP;
	}

	private void Die() {
		GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().PlayerLose();
	}
}
