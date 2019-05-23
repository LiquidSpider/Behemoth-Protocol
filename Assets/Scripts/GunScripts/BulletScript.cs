using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour {
	public float damage = 0f;       // Base damage
	public float speed = 200f;      // How fast the projectile moves
	public float weight = 0.01f;    // Weight of the projectile for impact force
	public bool grav = false;       // Does this projectile use gravity? Take weight into consideration
	public bool explosive = false;  // Does this projectile do splash damage and apply splash force?
	public float expSize = 0f;      // How large the explosion is
	public float expDamage = 0f;    // How much damage the explosion itself does. Will implement splash damage later
	public float lightStr = 20f;    // How bright the light component is
	public float prjSize = 0.1f;    // How large the collider is. Should be visually represented

	public Collider[] creatorsColliders;

	private Animator animator;

	// Start is called before the first frame update
	void Start() {
		SetStats();
	}

	void SetStats() {
		Rigidbody rb = GetComponent<Rigidbody>();
		animator = GetComponent<Animator>();
		rb.AddForce(( transform.forward ) * speed);
		rb.mass = weight;
		rb.useGravity = grav;
		rb.drag = weight * 0.01f;
	}

	/// <summary>
	/// Unity collision engine - On collision enter.
	/// </summary>
	/// <param name="collision">The collision occuring.</param>
	void OnCollisionEnter(Collision collision) {

		if (collision.collider.gameObject.tag == "Environment") {
			Destroy(this.gameObject);
		}

		// Get the index of the collider to see if it exists in the array
		int DoesContain = System.Array.IndexOf(creatorsColliders, collision.collider);

		// Check if the collision is the creators collider or another projectile
		if (DoesContain >= 0 || collision.gameObject.layer == 9) {

		} else {

			// if the object has the health script
			if (collision.gameObject.GetComponent<Health>() != null) {
                // take damage
                GameObject ui = GameObject.FindGameObjectWithTag("UI");
                ui.GetComponent<UISounds>().HitMarker();
                Debug.Log("Bullet hit enemy");
                var health = collision.gameObject.GetComponent<Health>();
				health.TakeDamage(damage);
                // play hit sound
                
            }

			// if the object has a parent
			if (collision.gameObject.transform.parent) {
				// if that objects parent has the Cube script
				if (collision.gameObject.transform.parent.gameObject.GetComponent<Cube>()) {
					// Take cube damage
					var Cube = collision.gameObject.transform.parent.gameObject.GetComponent<Cube>();
					Cube.TakeDamage(damage);
                    // play hit sound
                    var ui = GameObject.FindGameObjectWithTag("UI");
                    ui.GetComponent<UISounds>().HitMarker();
                }
			}

			// Destroy the bullet
			Destroy(this.gameObject);
		}

		if (collision.gameObject.transform.root.gameObject.tag == "Environment") Destroy(gameObject);
	}
}
