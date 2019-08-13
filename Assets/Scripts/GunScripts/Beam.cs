using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Beam : MonoBehaviour
{

    public float damage = 2f;
    public float damageTickRate = 0.5f;
    private float damageTime;
    private Collider[] colliders;
    private UISounds ui;

    // Start is called before the first frame update
    void Start()
    {

        // Get the parent object 
        GameObject parent = this.transform.root.gameObject;
        // Get all the colliders attached to this object
        colliders = parent.GetComponentsInChildren<Collider>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        // if this object isn't our parent collider
        if (Array.IndexOf(colliders, other) < 0)
        {

            if (other.gameObject.layer == 2 || other.gameObject.layer == 9)
            {

            }
            else
            {

                // Take damage if the object has health
                if (other.gameObject.GetComponent<PlayerDamageableSection>() != null)
                {

                    // Take damage per tickrate
                    if (Time.time > damageTime)
                    {
                        other.gameObject.GetComponent<PlayerDamageableSection>().TakeDamage(damage, null);
                        damageTime = Time.time + damageTickRate;
                        if (other.gameObject.GetComponentInChildren<ShieldBehaviour>())
                        {
                            if (other.gameObject.GetComponentInChildren<ShieldBehaviour>().shieldActive) ui.ShieldHit();
                        }
                    }

                }
            }

        }
    }

    private void OnTriggerEnter(Collider other)
    { 

        // if this object isn't our parent collider
        if (Array.IndexOf(colliders, other) < 0)
        {

            if(other.gameObject.layer == 2 || other.gameObject.layer == 9)
            {

            }
            else
            {

                // Take damage if the object has health
                if (other.gameObject.GetComponent<PlayerDamageableSection>() != null)
                {
                    
                    // Take damage per tickrate
                    if (Time.time > damageTime)
                    {
                        //Debug.Log("Damaging " + other.gameObject.name);
                        other.gameObject.GetComponent<PlayerDamageableSection>().TakeDamage(damage, null);
                        damageTime = Time.time + damageTickRate;
                        if (other.gameObject.GetComponentInChildren<ShieldBehaviour>())
                        {
                            if (other.gameObject.GetComponentInChildren<ShieldBehaviour>().shieldActive) ui.ShieldHit();
                        }
                    }

                }
            }

        }
    }

}
