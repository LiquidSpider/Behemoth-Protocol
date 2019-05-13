﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class WeaponSelect : MonoBehaviour {

	public Sprite left1;
	public Sprite left2;
	public Sprite left3;
	public Sprite left4;

	public int weaponNumber = -1;
	public int previousWeaponNumber = -1;

	// Update is called once per frame
	void Update() {
		if (Input.GetButtonDown("Weapon1")) {
			previousWeaponNumber = weaponNumber;
			weaponNumber = 1;
			gameObject.GetComponent<Image>().sprite = left4;
		}

		if (Input.GetButtonDown("Weapon2")) {
			previousWeaponNumber = weaponNumber;
			weaponNumber = 2;
			gameObject.GetComponent<Image>().sprite = left3;
		}

		if (Input.GetButtonDown("Weapon3")) {
			previousWeaponNumber = weaponNumber;
			weaponNumber = 3;
			gameObject.GetComponent<Image>().sprite = left2;
		}

		if (Input.GetButtonDown("Weapon4")) {
			previousWeaponNumber = weaponNumber;
			weaponNumber = 4;
			gameObject.GetComponent<Image>().sprite = left1;
		}
	}
}