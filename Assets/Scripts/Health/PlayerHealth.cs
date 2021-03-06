﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour {

	public float HP = 2500;
	private float maxHP;

	public bool isScanning = false;

	// Used for absorbable to check
	public bool isVacuuming = false;
	public float vacuumRadius;
	public LayerMask absorbableLayerMask;

	public float battery = 10000;
	public float maxB;

	private List<GameObject> TakenDamageFrom = new List<GameObject>();
	
	private bool batteryLowIndication = false;
	private bool damageTakenIndication = false;

    private UISounds ui;

	public GameObject batteryLowMessage;

	private void Start() {
		maxHP = HP;
		maxB = battery;
        ui = FindObjectOfType<UISounds>();
	}

	/// <summary>
	/// On object initialisation.
	/// </summary>
	private void Awake() {
		// Setup vacumm radius if it's not given a value.
		if (vacuumRadius == 0) {
#if DEBUG
			Debug.Log("Vacumm cannot have radius of 0. Given default value of 50.0f");
#endif
			vacuumRadius = 100.0f;
		}
	}

	private bool healthBelow25p = false;
	private bool healthBackUp = true;

	private void Update() {
		//if (Input.GetKey(KeyCode.X) && battery > 0 && HP < maxHP) {
		//	TakeDamage(-0.5f);
		//	UseBattery(50 * Time.deltaTime);
		//}

		if (HP < 0.25f * maxHP) {
			if (healthBelow25p == false) {
				healthBelow25p = true;

				GameObject.FindGameObjectWithTag("UI").GetComponent<NavigatorPrompts>().CallLowHealth();
			}
		} else {
			if (healthBelow25p == true) healthBelow25p = false;
		}

		if (!gameObject.GetComponent<NewWeaponController>().isVacuuming) {
			if (Input.GetButton("Regen") || Input.GetAxis("DPadUD") < 0) {
				if (HP < maxHP && battery > 0) {
					TakeDamage(-1f);
					UseBattery(50f * Time.deltaTime);
				}
			}
		}

		if (battery < 0.1f * maxB) {
			batteryLowMessage.SetActive(true);

			if (!batteryLowIndication) {
				batteryLowIndication = true;

				// Low battery prompt
				GameObject.FindGameObjectWithTag("UI").GetComponent<NavigatorPrompts>().CallBatteryLow();
			}
		} 

		// Stop battery from going into negative
		if (battery < 0) battery = 0;

		//if (HP < maxHP && battery > 0) {
		if (HP / maxHP < battery / maxB && battery > 0) {
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
			if (battery < 5) isScanning = false;
			else UseBattery(25 * Time.deltaTime);
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

	/// <summary>
	/// Every fixed frame.
	/// </summary>
	//public void FixedUpdate() {
	//	// If the player is using the vacumm
	//	if (isVacuuming)
	//		PullAbsorbable();
	//}

	/// <summary>
	/// Checks for surrounding absorbable units and pulls them towards the player.
	/// </summary>
	//public void PullAbsorbable() {
	//	// Place a sphere around the player looking for the absorbables.
	//	RaycastHit[] raycastHits = Physics.SphereCastAll(this.transform.position - (-this.transform.forward * (vacuumRadius / 2)), vacuumRadius, this.transform.forward, vacuumRadius, absorbableLayerMask);

	//	// Pull the objects towards the player.
	//	foreach (RaycastHit absorbable in raycastHits) {
	//		// Pull the absorable objects towards the player.
	//		absorbable.collider.gameObject.GetComponent<Absorbable>().VacuumObject(this.transform.position, this.vacuumRadius);
	//	}
	//}

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
            FindObjectOfType<UISounds>().Replenish();
		}

	}

	public void TakeDamage(float damage) {

		if (gameObject.transform.root.GetComponent<ShieldBehaviour>().shieldActive == true) {
			HP -= damage * 0.25f;
			UseBattery(damage * 0.75f);
            ui.ShieldHit();
		} else {
			HP -= damage;
            GetComponentInChildren<AnimatorManager>().Flinch();
		}

		if (HP < 0) HP = 0;
		
		// Check health and handle accordingly
		if (HP <= 0) {
			Die();
		}
		//GameObject.FindGameObjectWithTag("PlayerHealthBar").GetComponent<Image>().fillAmount = HP / maxHP;
	}

	public void TakeDamage(float damage, GameObject explosion) {

		// Check if this object should only take damage once
		if (explosion) {

			//if (!damageTakenIndication) {
			//	damageTakenIndication = true;

			//	// Nav prompt
			//	GameObject.FindGameObjectWithTag("UI").GetComponent<NavigatorPrompts>().CallTakingMissileDamage();
			//}

			if (!TakenDamageFrom.Contains(explosion)) {
				TakenDamageFrom.Add(explosion);

				if (gameObject.transform.root.GetComponent<ShieldBehaviour>().shieldActive == true) {
					HP -= damage * 0.5f;
					UseBattery(damage * 0.5f);
                    ui.ShieldHit();
                } else {
					HP -= damage;
                    GetComponentInChildren<AnimatorManager>().Flinch();
				}

				if (HP < 0) HP = 0;
			}

		} else {
			// Beam Damage

			if (gameObject.transform.root.GetComponent<ShieldBehaviour>().shieldActive == true) {
				HP -= damage * 0.5f;
				UseBattery(damage * 0.5f);
                ui.ShieldHit();
            } else {
				HP -= damage;
                GetComponentInChildren<AnimatorManager>().Flinch();
            }

			if (HP < 0) HP = 0;

		}

		// Check health and handle accordingly
		if (HP <= 0) {
			Die();
		}

		//GameObject.FindGameObjectWithTag("PlayerHealthBar").GetComponent<Image>().fillAmount = HP / maxHP;
	}

	private void Die() {
		GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().PlayerLose();
	}

#if DEBUG
	/// <summary>
	/// Draws gizmos when selected.
	/// </summary>
	private void OnDrawGizmosSelected() {
		// Draw the vacumm sphere radius.
		Color vacuumRadiusColumn = Color.blue;
		Gizmos.color = vacuumRadiusColumn;
		Gizmos.DrawWireSphere(this.transform.position, vacuumRadius);
	}

#endif

}
