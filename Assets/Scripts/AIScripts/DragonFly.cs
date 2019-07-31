﻿using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class DragonFly : MonoBehaviour
{

    // Player position variable
    public Transform player;
    // Children colliders
    private Collider[] colliders;
    public Rigidbody body;
    public GameObject rubbishPile;

    // Dragon fly Behaviour enum
    public enum DragonFlyBehaviour
    {
        idle,
        Attacking,
        Kamikaze
    }
    public DragonFlyBehaviour currentDragonFlyBehaviour;

    // Movement speeds
    public float defaultSpeed;
    public float defaultRotationSpeed;
    public float targetRadius;
    public float maxVelocity;
    private float sqrMaxVelocity;

    // boid variables
    public float cohesionForce;
    public float cohesionDistance;
    public float seperationDistance;
    public float seperationForce;
    public float alignmentStrength;
    public Boolean applyCohesion;
    public Boolean applySeperation;
    public Boolean applyAlignment;
    public Boolean flyTowardsTarget;

    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        // On spawn setup the collider list
        this.colliders = this.GetComponentsInChildren<Collider>();
        // Get the player object
        player = GameObject.FindWithTag("Player").transform.GetChild(0).transform;
        // Get the game manager
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        // Get the objects rigidbody.
        body = this.GetComponent<Rigidbody>();
        // Setup state
        this.currentDragonFlyBehaviour = DragonFlyBehaviour.idle;
    }
    
    /// <summary>
    /// Called once the object and all other objects have been initialised.
    /// </summary>
    private void Awake()
    {
        sqrMaxVelocity = maxVelocity * maxVelocity;
    }

    // Update is called once per frame
    void Update()
    {
        // maintain the boid behaviour;
        if (currentDragonFlyBehaviour != DragonFlyBehaviour.Kamikaze)
            BoidBehaviour();

        // check if we should kamikaze
        BoidsLeft();

        switch(currentDragonFlyBehaviour)
        {
            case DragonFlyBehaviour.idle:
                Idle();
                break;
            case DragonFlyBehaviour.Attacking:
                Attacking();
                break;
            case DragonFlyBehaviour.Kamikaze:
                Kamikaze();
                break;
        }
    }

    /// <summary>
    /// Checks the left over number of boids and determines if we should kamikaze.
    /// </summary>
    private void BoidsLeft()
    {
        if(gameManager.dragonFlies.Length <= 2)
        {
            currentDragonFlyBehaviour = DragonFlyBehaviour.Kamikaze;
        }
    }

    /// <summary>
    /// Called every fixed frame.
    /// </summary>
    private void FixedUpdate()
    {
        // Clamp the velocity of this object to keep a maximum speed.
        if(body.velocity.sqrMagnitude > sqrMaxVelocity)
        {
            // Given the direction of the objects current velocity multiply it by the maximum speed.
            body.velocity = body.velocity.normalized * maxVelocity;
        }
    }

    /// <summary>
    /// The idle state of the dragonfly.
    /// </summary>
    private void Idle()
    {
        // Check if player exists
        if (player != null)
        {
            this.currentDragonFlyBehaviour = DragonFlyBehaviour.Attacking;
        }
    }

    /// <summary>
    /// The attacking state of the dragonfly.
    /// </summary>
    private void Attacking()
    {
        // if we're outside of the range.
        if (flyTowardsTarget)
        {   
            if (Vector3.Distance(transform.position, player.transform.position) > targetRadius)
                MoveTowardsTarget(player.transform.position);
            else
            {
                // Rotate towards the target.
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(player.transform.position - transform.position), Mathf.Min(defaultRotationSpeed * Time.deltaTime, 1));
            }
        }
    }

    /// <summary>
    /// The kamikaze state of the dragonfly.
    /// </summary>
    private void Kamikaze()
    {
        MoveTowardsTarget(player.transform.position);
    }

    #region BoidBehaviour

    /// <summary>
    /// performs the boid algorithm for this dragon fly inside it's group.
    /// </summary>
    private void BoidBehaviour()
    {
        // The three vectors which perform the boid behaviour.
        if(applySeperation)
        {
            Seperation();
        }

        if(applyAlignment)
        {
            Alignment();
        }

        if(applyCohesion)
        {
            Cohesion();
        }
    }

    /// <summary>
    /// For all the boids maintain a seperation distance.
    /// </summary>
    /// <returns></returns>
    private void Seperation()
    {
        Vector3 seperation = Vector3.zero;
        int counter = 0;

        foreach(GameObject boid in gameManager.dragonFlies)
        {
            if(boid != this)
            {
                float distance = Vector3.Distance(this.transform.position, boid.transform.position);
                if ((distance > 0) && (distance < seperationDistance))
                {
                    Vector3 direction = (this.transform.position - boid.transform.position);
                    direction = Vector3.Normalize(direction);
                    direction = direction * (seperationForce);
                    seperation += direction;
                    counter++;
                }
            }
        }

        if(counter > 0)
        { 
            seperation = seperation * (1.0f / counter);
             this.body.AddForce(seperation);
        }
    }

    /// <summary>
    /// For all the boids average the direction.
    /// </summary>
    /// <returns></returns>
    private void Alignment()
    {
        Vector3 alignment = Vector3.zero;
        //Vector3 speed = Vector3.zero;
        int counter = 0;

        foreach(GameObject boid in gameManager.dragonFlies)
        {
            if(boid != this)
            {
                float distance = Vector3.Distance(this.transform.position, boid.transform.position);
                // Only calculate if withing range.
                if((distance > 0) && (distance < cohesionDistance))
                {
                    //speed = boid.GetComponent<DragonFly>().body.velocity;
                    alignment = boid.transform.forward;
                    counter++;
                }
            }
        }

        if(counter > 0)
        {
            //speed = speed * (1.0f / counter);
            //speed = Vector3.Normalize(speed) * alignmentStrength;
            alignment = alignment * (1.0f / counter);
            alignment = Vector3.Normalize(alignment);
            Quaternion rotation = Quaternion.Euler(alignment);

            // rotate towards average direction.
            this.transform.rotation = Quaternion.RotateTowards(this.transform.rotation, rotation, alignmentStrength);
            // meed average velocity.
        }
    }

    /// <summary>
    /// For all the boids average the position and steer towards that position.
    /// </summary>
    /// <returns></returns>
    private void Cohesion()
    {
        Vector3 cohesion = Vector3.zero;
        int counter = 0;

        foreach(GameObject boid in gameManager.dragonFlies)
        {
            if(boid != this)
            {
                float distance = Vector3.Distance(this.transform.position, boid.transform.position);
                if((distance > 0) && (distance < cohesionDistance))
                {
                    cohesion += boid.transform.position;
                    counter++;
                }
            }
        }
        
        if(counter > 0)
        {
            cohesion = cohesion * (1.0f / (counter));
            // pull towards position
            cohesion = (cohesion - this.transform.position).normalized;
            cohesion *= cohesionForce;
            this.body.AddForce(cohesion);
        }
    }

    ///// <summary>
    ///// Controls the positioning on this dragon fly incomparison to the others.
    ///// </summary>
    //private void BoidBehaviour()
    //{
    //    Vector3 alignment = this.transform.forward;
    //    Vector3 seperation = Vector3.zero;
    //    Vector3 cohesion = this.transform.position;

    //    foreach(GameObject boid in gameManager.dragonFlies)
    //    {
    //        if (boid != this)
    //        {
    //            seperation += GetSeparationVector(boid.transform);
    //            alignment += boid.GetComponent<Rigidbody>().velocity;
    //            cohesion += boid.transform.position;
    //        }
    //    }

    //    alignment /= gameManager.dragonFlies.Length - 1;
    //    alignment = alignment.normalized;
    //    cohesion /= gameManager.dragonFlies.Length - 1;
    //    cohesion = (cohesion - this.transform.position).normalized;

    //    // calculate rotation.
    //    Vector3 direction = alignment + seperation + cohesion;
    //    Quaternion rotation = Quaternion.FromToRotation(this.transform.forward, direction.normalized);

    //    // Apply the rotation.
    //    if (rotation != this.transform.rotation)
    //    {
    //        this.transform.rotation = Quaternion.Slerp(rotation, this.transform.rotation, Mathf.Exp(-this.defaultRotationSpeed * Time.deltaTime));
    //    }        
    //}

    //// Caluculates the separation vector with a target.
    //Vector3 GetSeparationVector(Transform target)
    //{
    //    Vector3 diff = this.transform.position - target.transform.position;
    //    float scaler = Mathf.Clamp01(1.0f - diff.magnitude / this.seperationDistance);
    //    return diff * (scaler / diff.magnitude);
    //}

    #endregion

    /// <summary>
    /// Moves the target towards the position using relative forces.
    /// </summary>
    /// <param name="position"></param>
    private void MoveTowardsTarget(Vector3 position)
    {
        // If kamikaze mode triple speed and ignore distance
        if(this.currentDragonFlyBehaviour == DragonFlyBehaviour.Kamikaze)
        {
            // rotate towards target.
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(position - transform.position), Mathf.Min(defaultRotationSpeed * 3 * Time.deltaTime, 1));
            // add forward momemtum.
            body.AddRelativeForce(Vector3.forward * defaultSpeed * 3 * Time.deltaTime);
        }
        else
        {
            // rotate towards target.
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(position - transform.position), Mathf.Min(defaultRotationSpeed * Time.deltaTime, 1));
            // add forward momemtum.
            body.AddRelativeForce(Vector3.forward * defaultSpeed * Time.deltaTime);
        }
    }

    /// <summary>
    /// Follows the player.
    /// </summary>
    //private void FollowPlayer()
    //{

    //    // Stay within radius of the player
    //    if (CheckDistance(player.transform.position) > FollowRadius)
    //    {

    //        // if we're going to collide
    //        //if (GoingToCollide(this.transform.forward))
    //        //{
    //        //    randomMovementTimer = Time.time - 1.0f;
    //        //}
    //        //else
    //        //{
    //            this.transform.position += this.transform.forward * Time.deltaTime * speed;
    //        //}

    //    }

    //    // always look at the player
    //    Quaternion direction = Quaternion.LookRotation(player.position - transform.position);
    //    this.transform.rotation = Quaternion.Lerp(this.transform.rotation, direction, rotationSpeed * Time.deltaTime);


    //    // Move around randomly
    //    if (Time.time > randomMovementTimer)
    //    {
    //        randomMovementTimer += Time.time + UnityEngine.Random.Range(2, 5);
    //        randomMovement = UnityEngine.Random.Range(0, 5);
    //    }
    //    else
    //    {

    //        Vector3 movementDirection = Vector3.zero;

    //        switch (randomMovement)
    //        {
    //            case 0:
    //                // do nothing
    //                break;
    //            case 1:
    //                // move up
    //                movementDirection = this.transform.up;
    //                break;
    //            case 2:
    //                // move down
    //                movementDirection = -this.transform.up;
    //                break;
    //            case 3:
    //                // move right
    //                movementDirection = this.transform.right;
    //                break;
    //            case 4:
    //                // move left
    //                movementDirection = -this.transform.right;
    //                break;
    //        }

    //        //// if we're going to collide
    //        //if (GoingToCollide(movementDirection))
    //        //{
    //        //    randomMovementTimer = Time.time - 1.0f;
    //        //}
    //        //else
    //        //{
    //            this.transform.position += movementDirection * Time.deltaTime * speed;
    //        //}

    //    }

    //}

    private void OnCollisionEnter(Collision collision)
    {
        // if we're kamikazeing and the player is the collider.
        if(currentDragonFlyBehaviour == DragonFlyBehaviour.Kamikaze)
        {
            // Blowup
            if (collision.collider.gameObject.tag == "Player")
                Destroy(this.gameObject);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
    }

    // On Destroy(this) event.
    private void OnDestroy()
    {
        // create a rubbish pile.
        Instantiate(rubbishPile, this.transform.localPosition, this.transform.localRotation);
    }

#if DEBUG

#endif

}