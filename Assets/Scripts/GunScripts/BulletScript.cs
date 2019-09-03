using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour {
	public float damage = 0f;       // Base damage
	public float speed = 500f;      // How fast the projectile moves
	public float weight = 0.01f;    // Weight of the projectile for impact force
	public bool grav = false;       // Does this projectile use gravity? Take weight into consideration

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
                //Debug.Log("Bullet hit enemy");
                var health = collision.gameObject.GetComponent<Health>();
				health.TakeDamage(damage);
                // play hit sound
                
            }

            // If the object is the player
            if(collision.gameObject.GetComponent<PlayerDamageableSection>())
            {

                //Debug.Log("Hit player");
                collision.gameObject.GetComponent<PlayerDamageableSection>().TakeDamage(damage, this.gameObject);

            }

            // If the object is the player
            if (collision.gameObject.GetComponent<PlayerHealth>())
            {

                Debug.Log("Hit player");
                collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(damage, this.gameObject);

            }

            // If the object has base health
            if (collision.transform.root.GetComponent<BaseHealth>())
            {
                BaseHealth[] healths = collision.transform.root.GetComponents<BaseHealth>();
                if (healths.Length > 1)
                {
                    foreach (BaseHealth health in healths)
                    {
                        // If the health that has the same layer as the collided object
                        if (health.healthLayer == 1 << collision.collider.gameObject.layer)
                        {
                            health.TakeDamage(this.damage);
                        }
                    }
                }
                else
                {
                    healths[0].TakeDamage(this.damage);
                }
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

            if(collision.gameObject.GetComponent<BossHealth>())
            {
                collision.gameObject.GetComponent<BossHealth>().TakeDamage(this.damage);
            }

			// Destroy the bullet
			Destroy(this.gameObject);
		}

		if (collision.gameObject.transform.root.gameObject.tag == "Environment") Destroy(gameObject);
	}
}
