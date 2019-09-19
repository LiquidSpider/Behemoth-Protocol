using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamageableSection : MonoBehaviour {

	private void OnTriggerEnter(Collider other) {
		if (other.gameObject.transform.tag == "Explosion - Enemy") {
			Transform parent = transform.root;

			parent.GetComponent<PlayerHealth>().TakeDamage(50, other.gameObject);

			// Nav prompt
			GameObject.FindGameObjectWithTag("UI").GetComponent<NavigatorPrompts>().CallTakingMissileDamage();
		}

		if (other.gameObject.tag == "Bullet - Enemy") {
			Transform parent = transform.root;

			parent.GetComponent<PlayerHealth>().TakeDamage(10, other.gameObject);

			// Nav prompt
			//GameObject.FindGameObjectWithTag("UI").GetComponent<NavigatorPrompts>().CallTakingMissileDamage();
		}

		if ((other.gameObject.layer == 17 || other.gameObject.layer == 18) && !other.gameObject.name.Contains("Laser")) {
			Transform parent = transform.root;

			parent.GetComponent<PlayerHealth>().TakeDamage(100);

			// Nav prompt
			GameObject.FindGameObjectWithTag("UI").GetComponent<NavigatorPrompts>().CallTakingPhysicalDamage();
		}
	}

	private void OnTriggerStay(Collider other) {
		if (other.gameObject.layer == 17 || other.gameObject.layer == 18) {
			Transform parent = transform.root;

			parent.GetComponent<PlayerHealth>().TakeDamage(30 * Time.deltaTime);
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
