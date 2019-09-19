using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Absorbable : MonoBehaviour {
	// controls the speed to gravitate towards the player
	public float speed;
	// the player object
	public GameObject player;
	// replenish amount
	public float replenishAmount;

	private static Vector3 gravityDirection = new Vector3(0, -1, 0);
	private const float gravityForce = 10.0f;

	/// <summary>
	/// Called when the object is initialised.
	/// </summary>
	void Awake() {
		// setup the player variable.
		if (GameObject.FindGameObjectWithTag("Player")) {
			player = GameObject.FindGameObjectWithTag("Player").gameObject;
		} else // Player doesn't exist cleanup this object.
		  {
#if DEBUG
			Debug.Log("No player found. Deleting this object.");
#endif
			Destroy(this.gameObject);
		}
	}

	/// <summary>
	/// Every physics update.
	/// </summary>
	private void FixedUpdate() {
		// increase gravity.
		this.GetComponent<Rigidbody>().AddForce(gravityDirection * gravityForce);
	}

	private void Update() {
		if (player.GetComponent<PlayerHealth>().isVacuuming) {
			if (Vector3.Distance(player.transform.position, gameObject.transform.position) < 25 ) { // && player.transform.root.GetComponent<PlayerHealth>().battery < 0.95f * player.transform.root.GetComponent<PlayerHealth>().maxB) {
				player.transform.root.GetComponent<PlayerHealth>().AddBattery(replenishAmount);
				Destroy(this.gameObject);
			}
		}
	}

	/// <summary>
	/// Move towards position at given force.
	/// </summary>
	/// <param name="position"></param>
	/// <param name="force"></param>
	private void MoveTo(Vector3 position, float force) {
		// get the direction between the player and this object
		Vector3 direction = (position - this.transform.position).normalized;
		// apply the force to this body.
		this.GetComponent<Rigidbody>().AddForce(direction * force);
	}

	/// <summary>
	/// Move towards position at default froce.
	/// </summary>
	/// <param name="position"></param>
	private void MoveTo(Vector3 position) {
		// get the direction between
		Vector3 direction = (position - this.transform.position).normalized;
		// apply the force to this body.
		this.GetComponent<Rigidbody>().AddForce(direction * speed);
	}

	/// <summary>
	/// Pulls the object given a radius to increment speed by.
	/// The closer the object, the more force.
	/// </summary>
	public void VacuumObject(Vector3 position, float radius) {
		// Calculate the incremental speed.
		float distance = Vector3.Distance(position, this.transform.position);
		float weighting = radius / distance;
		// Pull the object.
		this.MoveTo(position, speed * weighting);
	}

	/// <summary>
	/// Collision handler.
	/// </summary>
	/// <param name="collision"></param>
	private void OnCollisionEnter(Collision collision) {
		// If we collide with the player
		if (collision.collider == player.GetComponent<Collider>()) {
			// fill the battery
			player.transform.root.GetComponent<PlayerHealth>().AddBattery(replenishAmount);
			Destroy(this.gameObject);
		}
	}

#if DEBUG

	/// <summary>
	/// Draws gizmos when selected.
	/// </summary>
	private void OnDrawGizmosSelected() {

	}

#endif

}
