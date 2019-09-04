using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightBehaviour : MonoBehaviour {
	public GameObject[] lightsOnObject;
	
	private void Start() {

	}

	public void LightsOn() {
		for (int i = 0; i < lightsOnObject.Length; i++) {
			lightsOnObject[i].GetComponent<Light>().enabled = true;
			lightsOnObject[i].GetComponent<MeshRenderer>().material.color = Color.cyan;
		}
	}

	public void LightsOff() {
		for (int i = 0; i < lightsOnObject.Length; i++) {
			lightsOnObject[i].GetComponent<Light>().enabled = false;
			lightsOnObject[i].GetComponent<MeshRenderer>().material.color = Color.white;
		}
	}
}
