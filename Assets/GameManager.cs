using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public GameObject player;

	void Start() {
		player = GameObject.FindGameObjectWithTag("Player").transform.GetChild(1).gameObject;
	}
}
