using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public GameObject player;

	private bool gamePaused;
	public GameObject pauseMenu;

	void Start() {
		player = GameObject.FindGameObjectWithTag("Player").transform.GetChild(1).gameObject;
		gamePaused = false;
	}

	void Update() {
		if (Input.GetKeyDown(KeyCode.P)) {
			if (gamePaused) {
				gamePaused = false;
				pauseMenu.SetActive(false);
				Time.timeScale = 1;
				Cursor.lockState = CursorLockMode.Locked;
				Cursor.visible = false;
			} else {
				gamePaused = true;
				pauseMenu.SetActive(true);
				Time.timeScale = 0;
				Cursor.lockState = CursorLockMode.None;
				Cursor.visible = true;
			}
		}

		if (gamePaused) {
			if (Input.GetKeyDown(KeyCode.Q)) {
				Application.Quit();
			}
		}
	}
}
