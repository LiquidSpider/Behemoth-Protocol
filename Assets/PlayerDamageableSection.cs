using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamageableSection : MonoBehaviour {

	private void OnCollisionEnter(Collision other) {
		if (other.gameObject.transform.tag == "Explosion - Enemy") {
			Transform parent = transform.root;

			parent.GetComponent<PlayerHealth>().TakeDamage(100, other.gameObject);
		}
	}

    /// <summary>
    /// Handles bullet and beam damage on the player.
    /// </summary>
    /// <param name="damage">Amount of damage taking.</param>
    public void TakeDamage(float damage, GameObject damageSource)
    {

        Debug.Log("Dealing Damage to player");
        transform.root.GetComponent<PlayerHealth>().TakeDamage(damage, damageSource);

    }

}
