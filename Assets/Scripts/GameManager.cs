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

	public GameObject promptMenu;
	private float timeSetActive = 0;
	private bool dispMissileDamage = false;

	public Text framesCounter;

    public List<GameObject> dragonFlies;

    //public GameObject dragonfly;
    //public GameObject dragonflies;
    //private Vector3[] positions;
    //public float[] timeDestroyed = new float[] { -1, -1, -1, -1 };

    void Start() {
		gamePaused = false;
		pauseMenu.SetActive(false);

		Time.timeScale = 1;

		Cursor.visible = false;

		SetStuff(false);
		Cursor.lockState = CursorLockMode.Locked;

		//positions = new Vector3[dragonflies.transform.childCount];
		//for (int i = 0; i < dragonflies.transform.childCount; i++) {
		//	positions[i] = dragonflies.transform.GetChild(i).transform.position;
		//}
	}

	void Update() {
		if (!gamePaused && Time.time % 1 < 0.02f) framesCounter.text = Mathf.Round( 1f / Time.deltaTime ).ToString();

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

        dragonFlies = new List<GameObject>(GameObject.FindGameObjectsWithTag("DragonFly"));

        if (gamePaused || gameOver) {
			Camera.main.transform.rotation = cameraRotation;
		}

		if (salamander && salamander.GetComponent<BossHealth>().HP <= 100 && !dispMissileDamage) {
			dispMissileDamage = true;
			timeSetActive = Time.time;
			promptMenu.transform.GetChild(2).GetComponent<Text>().text = "Good job, now finish it with a missile (2)";
			promptMenu.SetActive(true);
		}

		if (timeSetActive + 3 < Time.time) {
			promptMenu.SetActive(false);
		}

		//if (dragonflies.transform.childCount < positions.Length) {

		//}
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

		player.GetComponent<NewWeaponController>().enabled = !swapTo;
		player.GetComponent<PlayerHealth>().enabled = !swapTo;
		player.GetComponentInChildren<PlayerController>().enabled = !swapTo;
		player.transform.GetChild(0).GetChild(0).GetComponent<Animator>().enabled = !swapTo;
		//player.transform.GetChild(2).gameObject.SetActive(!swapTo);
		//salamander.GetComponent<Sal>().enabled = !swapTo;

		//for (int i = 0; i < smallerEnemies.transform.childCount; i++) {
		//	smallerEnemies.transform.GetChild(i).GetComponent<DragonFly>().enabled = !swapTo;
		//}

		Cursor.visible = swapTo;
	}
}
