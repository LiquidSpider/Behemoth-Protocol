using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class InstructionsChange : MonoBehaviour {
	public Sprite mkControls;
	public Sprite cControls;

	public void MKControls() {
		gameObject.GetComponent<Image>().sprite = mkControls;

		transform.GetChild(0).gameObject.SetActive(false);
		transform.GetChild(1).gameObject.SetActive(true);
	}

	public void CControls() {
		gameObject.GetComponent<Image>().sprite = cControls;

		transform.GetChild(0).gameObject.SetActive(true);
		transform.GetChild(1).gameObject.SetActive(false);
	}

}
