using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class WeaponSelect : MonoBehaviour {

	public GameObject weapCirc1;
	public GameObject weapBar1;
	public GameObject weapCirc2;
	public GameObject weapBar2;
	public GameObject weapCirc3;
	public GameObject weapBar3;

	public GameObject missile1;
	public GameObject missile2;

	public int weaponNumber = 1;
	public int previousWeaponNumber = 1;

	private void Start() {
		weapCirc1.GetComponent<Animator>().SetBool("Active", true);
		weapBar1.GetComponent<Animator>().SetBool("Active", true);
	}

	// Update is called once per frame
	void Update() {
		if (Input.GetButtonDown("Weapon1")) {
			previousWeaponNumber = weaponNumber;
			weaponNumber = 1;
			//GameObject.FindGameObjectWithTag("UI").transform.GetChild(0).GetComponent<Image>().color = Color.green;
			//play animations here
			weapCirc1.GetComponent<Animator>().SetBool("Active", true);
			weapBar1.GetComponent<Animator>().SetBool("Active", true);
			weapCirc2.GetComponent<Animator>().SetBool("Active", false);
			weapBar2.GetComponent<Animator>().SetBool("Active", false);
			weapCirc3.GetComponent<Animator>().SetBool("Active", false);
			weapBar3.GetComponent<Animator>().SetBool("Active", false);

			missile1.GetComponent<Animator>().SetBool("Active", false);
			missile2.GetComponent<Animator>().SetBool("Active", false);
		}

		if (Input.GetButtonDown("Weapon2")) {
			previousWeaponNumber = weaponNumber;
			weaponNumber = 2;
			//play animations here
			weapCirc1.GetComponent<Animator>().SetBool("Active", false);
			weapBar1.GetComponent<Animator>().SetBool("Active", false);
			weapCirc2.GetComponent<Animator>().SetBool("Active", true);
			weapBar2.GetComponent<Animator>().SetBool("Active", true);
			weapCirc3.GetComponent<Animator>().SetBool("Active", false);
			weapBar3.GetComponent<Animator>().SetBool("Active", false);

			missile1.GetComponent<Animator>().SetBool("Active", true);
			missile2.GetComponent<Animator>().SetBool("Active", true);
		}

		if (Input.GetButtonDown("Weapon3")) {
			previousWeaponNumber = weaponNumber;
			weaponNumber = 3;
			//play animations here
			weapCirc1.GetComponent<Animator>().SetBool("Active", false);
			weapBar1.GetComponent<Animator>().SetBool("Active", false);
			weapCirc2.GetComponent<Animator>().SetBool("Active", false);
			weapBar2.GetComponent<Animator>().SetBool("Active", false);
			weapCirc3.GetComponent<Animator>().SetBool("Active", true);
			weapBar3.GetComponent<Animator>().SetBool("Active", true);

			missile1.GetComponent<Animator>().SetBool("Active", false);
			missile2.GetComponent<Animator>().SetBool("Active", false);
		}
	}
}
