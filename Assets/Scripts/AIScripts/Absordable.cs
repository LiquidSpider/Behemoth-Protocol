using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Absordable : MonoBehaviour
{

    // controls the speed to gravitate towards the player
    public float speed;
    // the player object
    public GameObject player;
    // replenish amount
    public float replenishAmount;

    // ignore this layer
    private int ignoreLayer;

    // Start is called before the first frame update
    void Start()
    {

        // ignore this objects layer
        ignoreLayer = this.gameObject.layer;

        player = GameObject.FindObjectOfType<PlayerController>().gameObject;

    }

    private void FixedUpdate()
    {
        CheckSurroundings();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// check surroudings for player
    /// </summary>
    private void CheckSurroundings()
    {

        // overlap self with a sphere
        Collider[] colliders = Physics.OverlapSphere(this.transform.position, 50.0f);

        // if the player is in the sphere
        if (Array.IndexOf(colliders, player.GetComponent<Collider>()) >= 0)
        {
            Debug.Log("moving towards player");
            MoveToPlayer();
            this.GetComponent<Rigidbody>().useGravity = false;
        }
        else
        {
            this.GetComponent<Rigidbody>().useGravity = true;
        }
    }

    /// <summary>
    /// Gravitate towards the player
    /// </summary>
    private void MoveToPlayer()
    {

        // get the direction between the player and this object
        Vector3 direction = (this.transform.position - player.transform.position).normalized;

        this.transform.position += -direction * speed;

    }

    // collision handler
    private void OnCollisionEnter(Collision collision)
    {

        // If we collide with the player
        if (collision.collider == player.GetComponent<Collider>())
        {
            // fill the battery
            player.transform.root.GetComponent<PlayerHealth>().AddBattery(replenishAmount);
            Destroy(this.gameObject);
        }

    }

    private void OnDrawGizmosSelected()
    {

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(this.transform.position, 50.0f);

    }

}
