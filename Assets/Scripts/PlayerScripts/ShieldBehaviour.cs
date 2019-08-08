using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldBehaviour : MonoBehaviour {

	public GameObject shield;
	public bool shieldActive;


	void Start() {
		//shield.SetActive(false);
		shieldActive = false;
	}
	
	void Update() {

		if (gameObject.transform.root.GetComponent<PlayerHealth>().battery > 500 && !gameObject.transform.root.GetComponent<PlayerController>().isCruising) {
			if (Input.GetKey(KeyCode.Z) && shieldActive == false) {
				shieldActive = true;
				shield.SetActive(true);
				shield.transform.GetChild(0).localScale = new Vector3(0.05f, 0.05f, 0.05f);
			} else if (Input.GetKeyUp(KeyCode.Z)) {
				shieldActive = false;
				shield.SetActive(false);
			}
		} else {
			shieldActive = false;
			shield.SetActive(false);
		}


		if (shieldActive) gameObject.transform.root.GetComponent<PlayerHealth>().UseBattery(50f * Time.deltaTime);

	}
}
