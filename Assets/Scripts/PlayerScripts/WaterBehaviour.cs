using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBehaviour : MonoBehaviour {

	public float resourceAvailable;
	private float maxResourceAvailable;
	private float waterLevel;

	private float yStartLocation;

	private Vector3 position;

	private GameObject player;
	public GameObject particle;

	private float timeBetweenParticles = 1;
	private float timeOfLastParticle = -1;

	void Start() {
		resourceAvailable = 5000;
		maxResourceAvailable = resourceAvailable;

		waterLevel = resourceAvailable / maxResourceAvailable;

		position = gameObject.transform.position;
		yStartLocation = gameObject.transform.position.y;

		player = GameObject.FindGameObjectWithTag("Player");
	}

	void Update() {
		if (resourceAvailable < maxResourceAvailable) {
			resourceAvailable += 10 * Time.deltaTime;
		}

		position.y = yStartLocation - (( 1 - (resourceAvailable / maxResourceAvailable) ) * 10);
		transform.position = position;

        //Material mat = gameObject.GetComponent<MeshRenderer>().material;
        //Color wat = mat.color;
        //float alpha = (resourceAvailable / maxResourceAvailable) * 2;
        //if (alpha > 1) alpha = 1;
        //wat.a = alpha;
        //mat.color = wat;

		if (player.GetComponent<PlayerHealth>().isVacuuming) {
			if (timeOfLastParticle + timeBetweenParticles < Time.time) {
				int numberOfParticles = (int) (2500f / Vector3.Distance(player.transform.position, ClosestPlayerPosition()));

				if (numberOfParticles > 30) numberOfParticles = 30;
				if (numberOfParticles < 1) numberOfParticles = 1;

				for (int i = 0; i < numberOfParticles; i++) {
					GameObject newParticle = Instantiate(particle);
					particle.transform.position = ClosestPlayerPosition() + Variation();
				}
				timeOfLastParticle = Time.time;
			}
		}
	}

	private Vector3 ClosestPlayerPosition() {
		return gameObject.transform.position;
	}

	private Vector3 Variation() {
		return gameObject.transform.right * Random.Range(-25f, 25f) + gameObject.transform.forward * Random.Range(-25f, 25f);
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
