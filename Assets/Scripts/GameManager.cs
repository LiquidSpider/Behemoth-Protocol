using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	public GameObject player;
	public GameObject salamander;
	public GameObject smallerEnemies;

	private bool gamePaused;
	public GameObject pauseMenu;

	private bool gameOver = false;

	private Quaternion cameraRotation;

	void Start() {
		gamePaused = false;
		pauseMenu.SetActive(false);

		Time.timeScale = 1;

		Cursor.visible = false;

		SetStuff(false);
		Cursor.lockState = CursorLockMode.Locked;
	}

	void Update() {
		if (!gameOver && (Input.GetKeyDown(KeyCode.P) || Input.GetButtonDown("Pause"))) {
			if (gamePaused) {
				gamePaused = false;
				pauseMenu.SetActive(false);

				Time.timeScale = 1;

				Cursor.visible = false;

				SetStuff(false);
				Cursor.lockState = CursorLockMode.Locked;
			} else {
				cameraRotation = Camera.main.transform.rotation;

				gamePaused = true;
				pauseMenu.SetActive(true);

				Time.timeScale = 0;

				Cursor.visible = true;

				SetStuff(true);
				Cursor.lockState = CursorLockMode.None;
			}
		}

		if (gamePaused || gameOver) {
			Camera.main.transform.rotation = cameraRotation;
		}
	}


	public void PlayerLose() {
		gamePaused = true;
		gameOver = true;
		pauseMenu.SetActive(true);
		pauseMenu.transform.GetChild(1).gameObject.SetActive(true);

		Time.timeScale = 0;
		Cursor.visible = true;

		SetStuff(true);
		Cursor.lockState = CursorLockMode.None;

		Destroy(player.transform.GetChild(1).GetChild(3).gameObject);
		Destroy(player.transform.GetChild(1).GetChild(0).gameObject);
		GameObject.FindGameObjectWithTag("PlayerHealthBar").GetComponent<Image>().fillAmount = 0;
	}

	public void PlayerWin() {
		gamePaused = false;
		gameOver = true;
		pauseMenu.SetActive(true);
		pauseMenu.transform.GetChild(0).gameObject.SetActive(true);

		Time.timeScale = 0;
		Cursor.visible = true;

		SetStuff(true);
		Cursor.lockState = CursorLockMode.None;

		Destroy(salamander);
	}

	private void SetStuff(bool swapTo) {
		gamePaused = swapTo;
		pauseMenu.SetActive(swapTo);

		player.GetComponent<WeaponController>().enabled = !swapTo;
		player.GetComponent<PlayerHealth>().enabled = !swapTo;
		player.transform.GetChild(1).GetComponent<PlayerController>().enabled = !swapTo;
		player.transform.GetChild(1).GetChild(0).GetComponent<Animator>().enabled = !swapTo;
		//player.transform.GetChild(2).gameObject.SetActive(!swapTo);
		salamander.GetComponent<Sal>().enabled = !swapTo;

		for (int i = 0; i < smallerEnemies.transform.childCount; i++) {
			smallerEnemies.transform.GetChild(i).GetComponent<DragonFly>().enabled = !swapTo;
		}

		Cursor.visible = swapTo;
	}
}
