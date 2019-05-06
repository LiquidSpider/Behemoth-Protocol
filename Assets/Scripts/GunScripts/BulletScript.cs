using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float damage = 0f;       // Base damage
    public float speed = 200f;      // How fast the projectile moves
    public float weight = 0.01f;    // Weight of the projectile for impact force
    public bool grav = false;       // Does this projectile use gravity? Take weight into consideration
    public bool explosive = false;  // Does this projectile do splash damage and apply splash force?
    public float expSize = 0f;      // How large the explosion is
    public float expDamage = 0f;    // How much damage the explosion itself does. Will implement splash damage later
    public float lightStr = 20f;    // How bright the light component is
    public float prjSize = 0.1f;    // How large the collider is. Should be visually represented

    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        SetStats();
    }

    void SetStats() {
        Rigidbody rb = GetComponent<Rigidbody>();
        Light light = GetComponent<Light>();
        animator = GetComponent<Animator>();
        rb.AddForce((transform.forward) * speed);
        rb.mass = weight;
        rb.useGravity = grav;
        rb.drag = weight * 0.01f;
        light.intensity = lightStr;
        light.range = lightStr;
    }
    void OnCollisionEnter(Collision collision) {
        if(collision.gameObject.layer != 9) { // Layer 9 is for player projectiles
            if(collision.gameObject.GetComponent<Health>() != null) {
                var health = collision.gameObject.GetComponent<Health>();
                health.TakeDamage(damage);
            }
            Destroy(this.gameObject);
        }
    }
}
