using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuControls : MonoBehaviour {

	public GameObject instructions;
	public GameObject settingsMenu;

	public bool inMenu = false;

	public void StartClicked() {
		SceneManager.LoadScene(1);
	}

	public void MainMenuClicked() {
		SceneManager.LoadScene(0); 
	}

	public void QuitClicked() {
		Application.Quit();
	}

	public void ResumeClicked() {
		instructions.SetActive(false);
		settingsMenu.SetActive(false);
		GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().CheckPause();
	}

	public void InstructionsClicked() {
		if (!inMenu) {
			instructions.SetActive(true);
			inMenu = true; 
		}
	}

	public void CloseInstructions() {
		if (inMenu) {
			instructions.SetActive(false);
			inMenu = false;
		}
	}

	public void SettingsClicked() {
		if (!inMenu) {
			settingsMenu.SetActive(true);
			inMenu = true;
		}
	}

	public void CloseSettings() {
		if (inMenu) {
			settingsMenu.SetActive(false);
			inMenu = false;
		}
	}

	public void CreditsClicked() {
		//if (!inMenu) {
		//	Debug.Log("Credits Clicked");
		//	inMenu = true;
		//}
	}

	public void CloseCredits() {
		//if (inMenu) {
		//	Debug.Log("Credits Clicked");
		//	inMenu = false;
		//}
	}
}
