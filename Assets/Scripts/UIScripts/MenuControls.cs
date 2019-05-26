using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class MenuControls : MonoBehaviour {
	
	public void StartClicked() {
		SceneManager.LoadScene(1);
	}

	public void QuitClicked() {
		Application.Quit();
	}

}
