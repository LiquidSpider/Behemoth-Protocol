using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseHealth : MonoBehaviour
{

    /// <summary>
    /// The objects health.
    /// </summary>
    public float health;

    /// <summary>
    /// The starting health of this object.
    /// </summary>
    public float startingHealth;

    /// <summary>
    /// Used to identify which arm.
    /// </summary>
    public LayerMask healthLayer;

    /// <summary>
    /// Determines if the object can take damage or not.
    /// </summary>
    public bool takeDamage;

    /// <summary>
    /// Used to identify if the arm is dead.
    /// </summary>
    public bool isDead = false;

    private void Start()
    {
        this.startingHealth = health;
    }

    /// <summary>
    /// Applies damage to the object.
    /// </summary>
    /// <param name="damage"></param>
    public void TakeDamage(float damage)
    {
        if(takeDamage)
        {
            this.health -= damage;
            if (health <= 0)
            {
                health = 0;
                Die();
            }
        }
    }

    /// <summary>
    /// Called when the objects health hits zero.
    /// </summary>
    public void Die()
    {
        this.isDead = true;
    }

    /// <summary>
    /// Used to revive the object.
    /// </summary>
    public void Revive()
    {
        this.health = startingHealth * 0.8f;
        this.isDead = false;
    }

}
