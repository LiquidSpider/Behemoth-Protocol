using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBehaviour : MonoBehaviour {

	public float resourceAvailable;
	private float maxResourceAvailable;
	private float waterLevel;

	private float yStartLocation;

	private Vector3 position;

	void Start() {
		resourceAvailable = 5000;
		maxResourceAvailable = resourceAvailable;

		waterLevel = resourceAvailable / maxResourceAvailable;

		position = gameObject.transform.position;
		yStartLocation = gameObject.transform.position.y;
	}

	void Update() {
		if (resourceAvailable < maxResourceAvailable) {
			resourceAvailable += 1 * Time.deltaTime;
		}

		position.y = yStartLocation - (( 1 - (resourceAvailable / maxResourceAvailable) ) * 10);
		transform.position = position;

        //Material mat = gameObject.GetComponent<MeshRenderer>().material;
        //Color wat = mat.color;
        //float alpha = (resourceAvailable / maxResourceAvailable) * 2;
        //if (alpha > 1) alpha = 1;
        //wat.a = alpha;
        //mat.color = wat;
	}

	public bool TakeWater(float waterUsed) {
		if (resourceAvailable > waterUsed) {
			resourceAvailable -= waterUsed;
			return true;
		} else {
			return false;
		}
	}
}
