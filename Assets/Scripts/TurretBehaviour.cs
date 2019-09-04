using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretBehaviour : MonoBehaviour {
	private GameObject player;

	public GameObject bullet;
	public GameObject shootPosition;

	private float shootTimer;
	private float shootTime = 0.25f;

	private void Start() {
		player = GameObject.FindGameObjectWithTag("Player");
		shootTimer = 0;
	}

	private void Update() {
		gameObject.transform.GetChild(0).GetChild(1).LookAt(player.transform.position);

		Vector3 rotation = gameObject.transform.GetChild(0).GetChild(1).localEulerAngles;

		rotation.z = 0;
		if (rotation.x > 180) rotation.x -= 360;
		if (rotation.y > 180) rotation.y -= 360;

		if (rotation.magnitude > 75) rotation = rotation.normalized * 75;

		gameObject.transform.GetChild(0).GetChild(1).localEulerAngles = rotation;

		if (Vector3.Distance(gameObject.transform.position, player.transform.position) < 1500 && rotation.magnitude < 75) Shoot();
		//Debug.Log(rotation);
	}

	private void Shoot() {
		if (shootTimer < Time.time) {
			// Shoot
			GameObject bulletNew = Instantiate(bullet, shootPosition.transform.position, gameObject.transform.GetChild(0).GetChild(1).rotation);
			bulletNew.GetComponent<BulletScript>().speed = 500;
			shootTimer = Time.time + shootTime;
		}
	}
}
