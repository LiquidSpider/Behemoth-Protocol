using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class WeaponSelect : MonoBehaviour {

	public Sprite left1;
	public Sprite left2;
	public Sprite left3;
	public Sprite left4;

	// Update is called once per frame
	void Update() {
		if (Input.GetButtonDown("Weapon1")) {
			gameObject.GetComponent<Image>().sprite = left1;
		}

		if (Input.GetButtonDown("Weapon2")) {
			gameObject.GetComponent<Image>().sprite = left2;
		}

		if (Input.GetButtonDown("Weapon3")) {
			gameObject.GetComponent<Image>().sprite = left3;
		}

		if (Input.GetButtonDown("Weapon4")) {
			gameObject.GetComponent<Image>().sprite = left4;
		}
	}
}
