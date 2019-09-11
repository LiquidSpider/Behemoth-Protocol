using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamageableSection : MonoBehaviour {

	private void OnTriggerEnter(Collider other) {
		if (other.gameObject.transform.tag == "Explosion - Enemy") {
			Transform parent = transform.root;

			parent.GetComponent<PlayerHealth>().TakeDamage(50, other.gameObject);
		}

		if (other.gameObject.tag == "Bullet - Enemy") {
			Transform parent = transform.root;

			parent.GetComponent<PlayerHealth>().TakeDamage(1, other.gameObject);
		}
		
		if (other.gameObject.layer == 17 || other.gameObject.layer == 18) {
			Transform parent = transform.root;

			parent.GetComponent<PlayerHealth>().TakeDamage(5);
		}
	}

    /// <summary>
    /// Handles bullet and beam damage on the player.
    /// </summary>
    /// <param name="damage">Amount of damage taking.</param>
    public void TakeDamage(float damage, GameObject damageSource)
    {
        
        transform.root.GetComponent<PlayerHealth>().TakeDamage(damage, damageSource);

    }

}
