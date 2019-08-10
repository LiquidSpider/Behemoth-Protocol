using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	// player object
	public GameObject player;

	// pause menu variables.
	public GameObject pauseMenu;
	private bool gamePaused;

	// win condition variables
	private bool gameOver = false;

	private Quaternion cameraRotation;
	public GameObject promptMenu;
	private float timeSetActive = 0;
	private bool dispMissileDamage = false;
	public Text framesCounter;

	// List of dragon flies in the scene.
	[System.NonSerialized]
	public List<GameObject> dragonFlies = new List<GameObject>();

	/// <summary>
	/// Called when the script is enabled.
	/// </summary>
	void Start() {
		// initial setup of pause variables.
		gamePaused = false;
		pauseMenu.SetActive(false);

		// setup time scale.
		Time.timeScale = 1;

		// Setup cursor.
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;

		Pause(false);
	}

	/// <summary>
	/// Called when the object is initialised.
	/// </summary>
	private void Awake() {
		// check if we have a player.
		if (!player) {
			player = GameObject.FindGameObjectWithTag("Player");
			// if the player still isn't found. Close the game.
			if (!player) {
				Debug.Log("Game Manager has no player and no object with the player tag exists.");
				Application.Quit();
			}
		}
	}

	/// <summary>
	/// Called every frame.
	/// </summary>
	void Update() {
		//#if DEBUG
		// Display the frame counter.
		if (!gamePaused && Time.time % 1 < 0.02f)
			framesCounter.text = Mathf.Round(1f / Time.deltaTime).ToString();
		//#endif

		// Moved this check out here, so I can call CheckPause from the Pause Menu
		if (!gameOver && ( Input.GetKeyDown(KeyCode.P) || Input.GetButtonDown("Pause") )) {
			CheckPause();
		}

		dragonFlies = new List<GameObject>(GameObject.FindGameObjectsWithTag("DragonFly"));

		if (gamePaused || gameOver) {
			Camera.main.transform.rotation = cameraRotation;
		}

		if (timeSetActive + 3 < Time.time) {
			promptMenu.SetActive(false);
		}
	}

	/// <summary>
	/// Handles the check condition and execution of pausing the game.
	/// </summary>
	public void CheckPause() {
		if (gamePaused) {
			gamePaused = false;
			pauseMenu.SetActive(false);

			Time.timeScale = 1;

			Cursor.visible = false;

			Pause(false);
			Cursor.lockState = CursorLockMode.Locked;
		} else {
			cameraRotation = Camera.main.transform.rotation;

			gamePaused = true;
			pauseMenu.SetActive(true);

			Time.timeScale = 0;

			Cursor.visible = true;

			Pause(true);
			Cursor.lockState = CursorLockMode.None;
		}
	}

	/// <summary>
	/// When the player has lost.
	/// </summary>
	public void PlayerLose() {
		gamePaused = true;
		gameOver = true;
		pauseMenu.SetActive(true);
		pauseMenu.transform.GetChild(1).gameObject.SetActive(true);

		Time.timeScale = 0;
		Cursor.visible = true;

		Pause(true);
		Cursor.lockState = CursorLockMode.None;

		Destroy(player.transform.GetChild(1).GetChild(3).gameObject);
		Destroy(player.transform.GetChild(1).GetChild(0).gameObject);
		GameObject.FindGameObjectWithTag("PlayerHealthBar").GetComponent<Image>().fillAmount = 0;
	}

	/// <summary>
	/// When the player has won.
	/// </summary>
	public void PlayerWin() {
		gamePaused = false;
		gameOver = true;
		pauseMenu.SetActive(true);
		pauseMenu.transform.GetChild(0).gameObject.SetActive(true);

		Time.timeScale = 0;
		Cursor.visible = true;

		Pause(true);
		Cursor.lockState = CursorLockMode.None;
	}

	/// <summary>
	/// Handles the pausing and unpausing of scripts and objects.
	/// </summary>
	/// <param name="swapTo"></param>
	private void Pause(bool swapTo) {
		gamePaused = swapTo;
		pauseMenu.SetActive(swapTo);

		player.GetComponent<NewWeaponController>().enabled = !swapTo;
		player.GetComponent<PlayerHealth>().enabled = !swapTo;
		player.GetComponent<PlayerController>().enabled = !swapTo;
		player.transform.GetChild(0).GetComponent<Animator>().enabled = !swapTo;
		//player.transform.GetChild(2).gameObject.SetActive(!swapTo);
		//salamander.GetComponent<Sal>().enabled = !swapTo;

		//for (int i = 0; i < smallerEnemies.transform.childCount; i++) {
		//	smallerEnemies.transform.GetChild(i).GetComponent<DragonFly>().enabled = !swapTo;
		//}

		Cursor.visible = swapTo;
	}
}
