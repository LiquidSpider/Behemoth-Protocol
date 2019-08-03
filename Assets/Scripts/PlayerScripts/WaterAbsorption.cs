using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterAbsorption : MonoBehaviour {
	public GameObject water;

	private void OnTriggerStay(Collider collider) {
		// Vacuum
		if (!GameObject.FindGameObjectWithTag("Player").transform.GetChild(0).GetComponent<PlayerController>().isCruising) {
			if (collider.gameObject.tag == "Water") {
				if (GameObject.FindGameObjectWithTag("LeftSelect").GetComponent<WeaponSelect>().weaponNumber == 3) {
					if (Input.GetButton("Attack")) {
						if (transform.root.GetComponent<PlayerHealth>().battery < transform.root.GetComponent<PlayerHealth>().maxB * 0.95f) {
							if (water.GetComponent<WaterBehaviour>().TakeWater(50 * Time.deltaTime)) {
								transform.root.GetComponent<PlayerHealth>().AddBattery(250 * Time.deltaTime);
							}
						}
					}
				}
			}
		}
	}
}
