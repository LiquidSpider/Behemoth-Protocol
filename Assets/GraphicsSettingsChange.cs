using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class GraphicsSettingsChange : MonoBehaviour {
	public void SetGraphicLevel() {
		int levelOfQuality = transform.GetChild(1).GetComponent<TMPro.TMP_Dropdown>().value;
		//int levelOfQuality = transform.GetChild(2).GetComponent<Dropdown>().value;
		QualitySettings.SetQualityLevel(levelOfQuality);
	}
}
