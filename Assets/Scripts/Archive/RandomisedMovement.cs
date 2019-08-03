using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomisedMovement : MonoBehaviour
{

    // Designated Locations
    public float accelerationForce;
    public float maximumSpeed;
    public float rotationalForce;
    public float maximumRotation;

    // this objects rigidbody
    private Rigidbody Rigidbody;

    // Movement Variables
    public enum Movement
    {
        idle,
        rotation,
        translation
    }
    public Movement currentState;
    private bool movementReset = false;

    // Idle Timer Variables
    private float idleTimer;
    private float currentTime;

    // Rotational Movement Variables
    private Vector3 rotationDirection;
    private float rotationTimer;

    // Translational Movement Variables
    private Vector3 translationDirection;
    private float movingTimer;

    // Start is called before the first frame update
    void Start()
    {

        // Check if rigidbody exists
        if (!this.gameObject.GetComponent<Rigidbody>())
        {
            throw new System.Exception(this.gameObject.name + " has no rigidbody attached. Please attach rigidbody.");
        }
        else
        {
            this.Rigidbody = this.gameObject.GetComponent<Rigidbody>();
        }

    }

    private void FixedUpdate()
    {

        // Lock all translation but gravity
        if (currentState != Movement.translation)
        {
            // Get the velocity in gravity direction
            float magnitude = Vector3.Dot(-this.gameObject.transform.up.normalized, Rigidbody.velocity);

            this.Rigidbody.velocity = -this.gameObject.transform.up.normalized * magnitude;

        }

        // Lock all rotation
        if (currentState != Movement.rotation)
        {

            float magnitude = Vector3.Dot(-this.gameObject.transform.up.normalized, Rigidbody.angularVelocity);

            this.Rigidbody.angularVelocity = -this.gameObject.transform.up.normalized * magnitude;

        }
            //this.Rigidbody.angularVelocity = Vector3.zero;

    }

    // Update is called once per frame
    void Update()
    {

        DetermineState();

        // Execute current state methods
        switch (currentState)
        {
            case Movement.idle:
                Idle();
                break;
            case Movement.rotation:
                RotationalMovement();
                break;
            case Movement.translation:
                PositionalMovement();
                break;
        }

    }

    /// <summary>
    /// Determine which state the AI should switch too.
    /// </summary>
    void DetermineState()
    {

        // If the AI has finished it's previous state or forced to transition
        if (movementReset)
        {

            // Randomly assign a decision
            int number = Random.Range(1, 5);

            // Depending on the number chnage the current state
            switch (number)
            {
                case 1:
                    // Determine a time to be idle for
                    idleTimer = Random.Range(1, 2);
                    currentTime = Time.time;
                    this.currentState = Movement.idle;
                    break;
                case 2:
                    // Determine which direction to rotate in
                    int Rotation = Random.Range(0, 2);
                    switch(Rotation)
                    {
                        case 0:
                            rotationDirection = this.gameObject.transform.up;
                            break;
                        case 1:
                            rotationDirection = -this.gameObject.transform.up;
                            break;
                    }
                    // Amount rotating
                    rotationTimer = Random.Range(2, 4);
                    currentTime = Time.time;
                    this.currentState = Movement.rotation;
                    break;
                case 3:
                case 4:
                    // Determine the direction to translate towards
                    int Translation = Random.Range(0, 4);
                    switch(Translation)
                    {
                        case 0:
                            translationDirection = this.gameObject.transform.forward;
                            break;
                        case 1:
                            translationDirection = -this.gameObject.transform.forward;
                            break;
                        case 2:
                            translationDirection = this.gameObject.transform.right;
                            break;
                        case 3:
                            translationDirection = -this.gameObject.transform.right;
                            break;
                    }
                    // Timer to move
                    movingTimer = Random.Range(2, 6);
                    currentTime = Time.time;
                    this.currentState = Movement.translation;
                    break;
            }

            // State has been determined
            movementReset = false;
        }

    }

    /// <summary>
    /// Perform the positional movement state.
    /// </summary>
    void PositionalMovement()
    {

        // Check if we've moved for long enough
        if(Time.time > currentTime + movingTimer)
        {

            // Reset movement
            movementReset = true;

        }
        else
        {

            // Check if the current direction we're moving in has hit an object
            RaycastHit hit;

            if(!Physics.Raycast(this.gameObject.transform.position, translationDirection, out hit, 10.0f))
            {
                
                // If the object has reached max speed
                if(Vector3.Dot(translationDirection, Rigidbody.velocity) < maximumSpeed)
                {
                    //Increase speed
                    Rigidbody.velocity += translationDirection * accelerationForce;
                }
                else
                {

                    // Get the velocity of the object in the direction
                    Vector3 velocity = Rigidbody.velocity;

                    // Get the speed it's moving towards the direction
                    float speed = Vector3.Dot(translationDirection, velocity);
                    // remove that speed from the objects velocity
                    velocity -= speed * translationDirection;
                    // Add the maximum speed;
                    velocity += maximumSpeed * translationDirection;

                    Rigidbody.velocity = velocity;

                }

            }
            else
            {

                // Reset movement
                movementReset = true;

            }

        }

    }

    /// <summary>
    /// Perform the AI Rotational Movement state.
    /// </summary>
    void RotationalMovement()
    {

        // Check if we've rotated for long enough
        if(Time.time > currentTime + rotationTimer)
        {

            // Reset Movement
            movementReset = true;
        }
        else
        {

            // If the object has reached max speed
            if (Vector3.Dot(rotationDirection, Rigidbody.angularVelocity) < maximumRotation)
            {
                //Increase speed
                Rigidbody.angularVelocity += rotationDirection * rotationalForce;
            }
            else
            {

                // Get the velocity of the object in the direction
                Vector3 velocity = Rigidbody.angularVelocity;

                // Get the speed it's moving towards the direction
                float speed = Vector3.Dot(rotationDirection, velocity);
                // remove that speed from the objects velocity
                velocity -= speed * rotationDirection;
                // Add the maximum speed;
                velocity += maximumRotation * rotationDirection;

                Rigidbody.angularVelocity = velocity;

            }

            //// Rotate the object
            //Rigidbody.angularVelocity += rotationDirection * rotationalForce;

        }

    }

    /// <summary>
    /// Perform th AI idle state.
    /// </summary>
    void Idle()
    {

        // Check if we shoud still be idling
        if(Time.time > currentTime + idleTimer)
        {
            // Reset Idle
            movementReset = true;
        }

    }

}
