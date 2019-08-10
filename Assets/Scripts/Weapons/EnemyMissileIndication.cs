using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyMissileIndication : MonoBehaviour {

	private Vector2 screenPos;
	private float distance;

	private GameObject missileToFollow;

	private float screenWidth = 1920;
	private float screenHeight = 1080;

	private float percentage = 0.6f;

	private float xScale;
	private float yScale;

	void Start() {
		gameObject.GetComponent<Image>().enabled = false;

		screenPos = Camera.main.WorldToScreenPoint(gameObject.transform.parent.parent.position);
		gameObject.transform.position = screenPos;

		screenWidth = Camera.main.pixelWidth;
		screenHeight = Camera.main.pixelHeight;

		xScale = ( screenWidth * percentage ) / 2f;
		yScale = ( screenHeight * percentage ) / 2f;
	}

	void Update() {
		distance = Vector3.Distance(gameObject.transform.parent.parent.position, GameObject.FindGameObjectWithTag("Player").transform.position);
		screenPos = Camera.main.WorldToScreenPoint(gameObject.transform.parent.parent.position);

		
		if (distance <= 1000f) {
			float sFactor = (1.1f - (distance / 1000f));

			gameObject.transform.localScale = new Vector3(sFactor, sFactor, sFactor);

			if (!gameObject.GetComponent<Image>().enabled) gameObject.GetComponent<Image>().enabled = true;

			if (distance < Vector3.Distance(gameObject.transform.parent.parent.position, Camera.main.transform.position)) {
				Color cyan = Color.cyan;
				cyan.a = 100f / 255f;
				GetComponent<Image>().color = cyan;

				float xFactor = Mathf.Pow((screenPos.x - (screenWidth / 2)) / xScale, 2);
				float yFactor = Mathf.Pow((screenPos.y - (screenHeight / 2)) / yScale, 2);

				if (xFactor + yFactor <= 1) {
					gameObject.transform.position = screenPos;
				} else {
					Vector2 pos = (screenPos - new Vector2(screenWidth * 0.5f, screenHeight * 0.5f)).normalized;

					pos.x = pos.x * xScale + ( screenWidth / 2 );
					pos.y = pos.y * yScale + ( screenHeight / 2 );

					//if (screenPos.x - ( screenWidth / 2 ) >= 0) {
					//	if (screenPos.y - ( screenHeight / 2 ) >= 0) {
					//		pos.x = pos.x * xScale + ( screenWidth / 2 );
					//		pos.y = pos.y * yScale + ( screenHeight / 2 );
					//	} else {
					//		pos.x = pos.x * xScale + ( screenWidth / 2 );
					//		pos.y = -pos.y * yScale + ( screenHeight / 2 );
					//	}
					//} else {
					//	if (screenPos.y - ( screenHeight / 2 ) >= 0) {
					//		pos.x = -pos.x * xScale + ( screenWidth / 2 );
					//		pos.y = pos.y * yScale + ( screenHeight / 2 );
					//	} else {
					//		pos.x = -pos.x * xScale + ( screenWidth / 2 );
					//		pos.y = -pos.y * yScale + ( screenHeight / 2 );
					//	}
					//}

					//Vector2 pos = new Vector2(xFactor * xScale + (screenWidth / 2), yFactor * yScale + (screenHeight / 2));
					gameObject.transform.position = pos;
				}

			} else {
				Color red = Color.red;
				red.a = 100f / 255f;
				GetComponent<Image>().color = red;

				Vector2 pos = (screenPos - new Vector2(screenWidth * 0.5f, screenHeight * 0.5f)).normalized;

				pos.x = pos.x * xScale + ( screenWidth / 2 );
				pos.y = pos.y * yScale + ( screenHeight / 2 );

				gameObject.transform.position = pos;
			}
		} else {
			gameObject.GetComponent<Image>().enabled = false;
		}
	}
}
