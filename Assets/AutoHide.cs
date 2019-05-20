using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoHide : MonoBehaviour {

	private float time;
	private float appearTime = 10.0f;
	
	private void OnEnable() {
		time = Time.time;
	}

	void Update() {
		if (Time.time > time + appearTime) gameObject.SetActive(false);
	}
}
