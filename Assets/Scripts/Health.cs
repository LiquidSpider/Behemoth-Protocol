using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {
	// Start is called before the first frame update
	public float maxHealth = 200f;
	public float health = 200f;
	public bool soundRandomPitch = false;
	public AudioClip death;
	public GameObject sndSrc;

	private Material mainMaterial;
	public Material changedMaterial;


	private float timeOfScan = -1;
	private float scanDistance;
	private float scanTime = 3;

	private int materialMode = 0;


	void Start() {
		mainMaterial = gameObject.GetComponent<MeshRenderer>().material;
	}

	// Update is called once per frame
	void Update() {
		if (Input.GetMouseButtonDown(1) && materialMode == 0) {
			timeOfScan = Time.time;
			scanDistance = Vector3.Magnitude(transform.position - GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().player.transform.position) / 500f;
			materialMode = 1;
		}

		if (materialMode == 1 && Time.time > timeOfScan + scanDistance) {
			changedMaterial.color = DetermineColour();
			gameObject.GetComponent<MeshRenderer>().material = changedMaterial;
			materialMode = 2;
		} else if (materialMode == 2) {
			if (Time.time > timeOfScan + scanDistance + scanTime) {
				gameObject.GetComponent<MeshRenderer>().material = mainMaterial;
				materialMode = 0;
			}
		}
	}

	public void TakeDamage(float damage) {   // damage = Base Damage, pen = Armour Penetration
		changedMaterial.color = DetermineColour();

		health -= damage;
		if (health <= 0) {
			Die();
		}
	}

	void Die() {
		MakeSound(death, death.length, false);
		Destroy(gameObject);
	}

	void MakeSound(AudioClip sound, float sLength, bool isRandom) {
		AudioSource source = sndSrc.GetComponent<AudioSource>();
		source.clip = sound;
		source.volume = 0.5f;
		if (isRandom) source.pitch = Random.Range(0.75f, 1.25f);
		GameObject oSound = Instantiate(sndSrc, transform.position, transform.rotation);
		oSound.GetComponent<TimedDestroy>().maxTime = sLength;
	}

	private Color DetermineColour() {
		float temp = health / maxHealth;
		Color tempColour = new Color();

		if (temp <= 0.05f) {
			tempColour = new Color(1, 0, 0);
		} else if (temp <= 0.48f) {
			float rFactor = (0.48f - temp) / 0.43f;
			float yFactor = (temp - 0.05f) / 0.43f;

			tempColour = new Color(rFactor + yFactor, yFactor, 0);
		} else if (temp <= 0.53f) {
			tempColour = new Color(1, 1, 0);
		} else if (temp <= 0.95f) {
			float yFactor = (0.95f - temp) / 0.42f;
			float gFactor = (temp - 0.53f) / 0.42f;

			tempColour = new Color(yFactor, yFactor + gFactor, 0);
		} else {
			tempColour = new Color(0, 1, 0);
		}

		return tempColour;
	}

	private void OnCollisionEnter(Collision other) {
		if (other.transform.tag == "Explosion") {
			TakeDamage(30);
		}
	}
}
