﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	// player object
	public GameObject player;

	// pause menu variables.
	public GameObject pauseMenu;
	public GameObject instructionsMenu;
	public GameObject settingsMenu;
	[System.NonSerialized]
	public bool gamePaused;

	public GameObject winMenu;
	public GameObject loseMenu;

	// win condition variables
	public bool gameOver = false;

	private Quaternion cameraRotation;
	public GameObject promptMenu;
	public GameObject instructionsPanel;
	private float timeSetActive = 0;
	private bool dispMissileDamage = false;
	public Text framesCounter;

	// List of dragon flies in the scene.
	//[System.NonSerialized]
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
#if DEBUG
		// Display the frame counter.
		if (!gamePaused && Time.time % 1 < 0.02f)
			framesCounter.gameObject.SetActive(true);
		framesCounter.text = Mathf.Round(1f / Time.deltaTime).ToString();
#endif

		// Moved this check out here, so I can call CheckPause from the Pause Menu
		if (!gameOver && ( Input.GetButtonDown("Pause") )) {
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
			instructionsPanel.SetActive(false);

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
		if (!gameOver) StartCoroutine(COLose());
	}

	private IEnumerator COLose() {
		gameOver = true;
		Debug.Log("Game Over");

		//yield return new WaitForSeconds(3);
		yield return new WaitForSeconds(0);

		gamePaused = true;
		loseMenu.SetActive(true);
		pauseMenu.transform.GetChild(1).gameObject.SetActive(true);

		Time.timeScale = 0;
		Cursor.visible = true;

		Pause(true);
		pauseMenu.SetActive(false);
		Cursor.lockState = CursorLockMode.None;

		//Destroy(player.transform.GetChild(1).GetChild(3).gameObject);
		Destroy(player.transform.GetChild(1).GetChild(0).gameObject);
		GameObject.FindGameObjectWithTag("PlayerHealthBar").GetComponent<Image>().fillAmount = 0;
	}

	/// <summary>
	/// When the player has won.
	/// </summary>
	public void PlayerWin() {
		if (!gameOver) StartCoroutine(COWin());
	}

	private IEnumerator COWin() {
		gameOver = true;
		//Debug.Log("Game Over");

		GameObject.FindGameObjectWithTag("UI").GetComponent<NavigatorPrompts>().CallWin();

		//yield return new WaitForSeconds(3);
		yield return new WaitForSeconds(0);

		gamePaused = false;
		winMenu.SetActive(true);
		pauseMenu.transform.GetChild(0).gameObject.SetActive(true);

		Time.timeScale = 0;
		Cursor.visible = true;

		Pause(true);
		pauseMenu.SetActive(false);
		Cursor.lockState = CursorLockMode.None;
	}

	/// <summary>
	/// Handles the pausing and unpausing of scripts and objects.
	/// </summary>
	/// <param name="swapTo"></param>
	private void Pause(bool swapTo) {
		gamePaused = swapTo;
		pauseMenu.SetActive(swapTo);
		if (!swapTo) {
			instructionsMenu.SetActive(false);
			settingsMenu.SetActive(false);

			pauseMenu.GetComponent<MenuControls>().inMenu = false;
		}

		player.GetComponent<NewWeaponController>().enabled = !swapTo;
		player.GetComponent<PlayerHealth>().enabled = !swapTo;
		player.GetComponent<PlayerController>().enabled = !swapTo;
		player.transform.GetChild(0).GetComponent<Animator>().enabled = !swapTo;

		foreach (GameObject df in dragonFlies) {
			df.GetComponent<DragonFly>().enabled = !swapTo;
		}

		GameObject missiles = GameObject.FindGameObjectWithTag("MissileParent");

		for (int i = 0; i < missiles.transform.childCount; i++) {
			GameObject missile = missiles.transform.GetChild(i).gameObject;

			if (missile.GetComponent<PlayerMissileBehaviour>()) {
				missile.GetComponent<PlayerMissileBehaviour>().enabled = !swapTo;
			}

			if (missile.GetComponent<EnemyMissileBehaviour>()) {
				missile.GetComponent<EnemyMissileBehaviour>().enabled = !swapTo;

			}

			if (missile.GetComponentInChildren<AudioSource>()) {
				if (!swapTo) missile.GetComponentInChildren<AudioSource>().Pause();
				if (swapTo) missile.GetComponentInChildren<AudioSource>().UnPause();

			}

			//missiles.transform.GetChild(i).gameObject.SetActive(!swapTo);
		}

		//player.transform.GetChild(2).gameObject.SetActive(!swapTo);
		//salamander.GetComponent<Sal>().enabled = !swapTo;

		//for (int i = 0; i < smallerEnemies.transform.childCount; i++) {
		//	smallerEnemies.transform.GetChild(i).GetComponent<DragonFly>().enabled = !swapTo;
		//}

		Cursor.visible = swapTo;
	}

	public void SpawnFakeArm(GameObject inputHandMesh) {
		//GameObject test = Instantiate(inputHandMesh);

		//test.transform.localScale = inputHandMesh.transform.root.localScale;
		//test.transform.position = inputHandMesh.transform.position;

		//if (inputHandMesh.name.Contains("Left")) {
		//	Vector3 scale = test.transform.localScale;
		//	scale.x *= -1;

		//	test.transform.localScale = scale;
		//}

		//foreach (SkinnedMeshRenderer temp in test.GetComponentsInChildren<SkinnedMeshRenderer>()) {
		//	temp.enabled = true;
		//	temp.gameObject.SetActive(true);

		//	temp.gameObject.AddComponent<MeshRenderer>();
		//	temp.gameObject.GetComponent<MeshRenderer>().material = temp.material;

		//	temp.gameObject.AddComponent<MeshFilter>();
		//	temp.gameObject.GetComponent<MeshFilter>().mesh = temp.sharedMesh;

		//	temp.gameObject.AddComponent<MeshCollider>();
		//	temp.gameObject.GetComponent<MeshCollider>().sharedMesh = temp.sharedMesh;
		//	temp.gameObject.GetComponent<MeshCollider>().convex = true;
		//	//temp.gameObject.GetComponent<MeshCollider>().isTrigger = true;

		//	temp.gameObject.AddComponent<Rigidbody>();
		//	temp.gameObject.GetComponent<Rigidbody>().mass = 100000;
		//	temp.gameObject.GetComponent<Rigidbody>().drag = 0.5f;
		//	temp.gameObject.GetComponent<Rigidbody>().angularDrag = 0.5f;
		//	temp.gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(0, -100000, 0));

		//	temp.transform.parent = null;

		//	Destroy(temp.gameObject.GetComponent<SkinnedMeshRenderer>());
		//}

		foreach (SkinnedMeshRenderer sMR in inputHandMesh.GetComponentsInChildren<SkinnedMeshRenderer>()) {
			GameObject newPart = new GameObject();
			newPart.name = sMR.gameObject.name;

			newPart.AddComponent<MeshFilter>();
			newPart.GetComponent<MeshFilter>().mesh = sMR.sharedMesh;

			newPart.AddComponent<MeshRenderer>();
			newPart.GetComponent<MeshRenderer>().material = sMR.material;


			//newPart.AddComponent<SkinnedMeshRenderer>();
			//newPart.GetComponent<SkinnedMeshRenderer>().sharedMesh = sMR.sharedMesh;
			//newPart.GetComponent<SkinnedMeshRenderer>().material = sMR.material;
			//newPart.GetComponent<SkinnedMeshRenderer>().localBounds = sMR.bounds;

			newPart.transform.position = sMR.gameObject.transform.position;

			//newPart.transform.position = sMR.transform.parent.position;

			newPart.transform.rotation = sMR.gameObject.transform.rotation;
			newPart.transform.localScale = sMR.gameObject.transform.lossyScale;

			if (inputHandMesh.name.Contains("Right")) {
				Vector3 temp = newPart.transform.localScale;
				temp.x *= -1;
				newPart.transform.localScale = temp;
			}

			newPart.AddComponent<MeshCollider>();
			newPart.GetComponent<MeshCollider>().sharedMesh = sMR.sharedMesh;
			newPart.GetComponent<MeshCollider>().convex = true;
			//newPart.GetComponent<MeshCollider>().isTrigger = true;

			newPart.AddComponent<Rigidbody>();
			newPart.GetComponent<Rigidbody>().mass = 0.1f;
			newPart.GetComponent<Rigidbody>().drag = 0.9f;
			newPart.GetComponent<Rigidbody>().angularDrag = 0.2f;
			newPart.GetComponent<Rigidbody>().useGravity = true;
			newPart.GetComponent<Rigidbody>().AddForce(new Vector3(0, -1000, 0));

			newPart.AddComponent<RobotPartFall>();

			//Exploder.explode(newPart.transform);
		}
	}
}
