using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{

    // Enum creation
    public enum CubeStates
    {
        Coward,
        Retaliation,
        AngreyBoi
    }

    public enum CowardDecisions
    {
        nothing,
        stop,
        move,
        runAway,
        hit
    }

    // Enum variables
    public CubeStates cubeState;
    public CowardDecisions cowardDecision;

    // public object variables
    public GameObject player;
    public GameObject level;
    public GameObject Iris;
    public Vector3 eyeStartingPosition;

    // private object variables
    private float CowardStopTimer;
    private int[] CowardStopRotations;
    private float CowardMoveTimer;

    public bool _PlayerDetected = false;
    private float PlayerMissing;
    public bool PlayerDetected
    {
        get
        {
            return _PlayerDetected;
        }
        set
        {
            _PlayerDetected = value;
            if (_PlayerDetected == false)
                PlayerMissing = Time.time;
        }
    }

    // Start is called before the first frame update
    void Start()
    {

        this.cubeState = CubeStates.Coward;

        if (!this.player)
        {
            throw new System.Exception("Please assign an object as the " + this.gameObject.name + " 's player in the Cube script.");
        }

        this.eyeStartingPosition = Iris.transform.localPosition;

    }

    // Update is called once per frame
    void Update()
    {

        //EyeTest();

        // Determine what to do
        switch(cubeState)
        {
            case CubeStates.Coward:
                Coward();
                break;
            case CubeStates.Retaliation:
                Retaliation();
                break;
            case CubeStates.AngreyBoi:
                AngreyBoi();
                break;
        }        

    }

    public void EyeTest()
    {
        Vector3 lookDirection = (this.player.transform.position - this.eyeStartingPosition).normalized;
        Debug.Log("Vector3(" + lookDirection.x + "," + lookDirection.y + "," + lookDirection.z + ")" );
        Iris.transform.localPosition = this.eyeStartingPosition + (lookDirection / 100.0f);
    }

    /// <summary>
    /// Perform the coward state
    /// </summary>
    public void Coward()
    {

        // Check if player is within radius
        CheckPlayer();
        if (PlayerDetected && cowardDecision != CowardDecisions.hit)
            this.cowardDecision = CowardDecisions.runAway;

        // Determine which state to be in
        if (cowardDecision == CowardDecisions.nothing)
        {

            int DetermineChange = Random.Range(0, 3);

            switch(DetermineChange)
            {
                case 0:
                case 1:
                    cowardDecision = CowardDecisions.move;
                    CowardMoveTimer = Time.time + Random.Range(6, 10);
                    break;
                case 2:
                    cowardDecision = CowardDecisions.stop;
                    // Determine how long to stop for
                    CowardStopTimer = Time.time + Random.Range(6, 10);
                    // Determine which way to rotate
                    int NumberOfRotations = Random.Range(0, 4);
                    if(NumberOfRotations != 0)
                    {
                        CowardStopRotations = new int[NumberOfRotations];
                        if (NumberOfRotations == 3)
                        {
                            CowardStopRotations[0] = Random.Range(0, 2);
                            CowardStopRotations[1] = Random.Range(2, 4);
                            CowardStopRotations[2] = Random.Range(4, 6);
                        }else if(NumberOfRotations == 2)
                        {
                            CowardStopRotations[0] = Random.Range(0, 4);
                            CowardStopRotations[1] = Random.Range(4, 6);
                        }
                        else
                        {
                            CowardStopRotations[0] = Random.Range(0, 6);
                        }
                    }
                    else
                    {
                        CowardStopRotations = null;
                    }
                    break;
            }

        }

        // Execute current state
        switch (cowardDecision)
        {
            case CowardDecisions.stop:
                CowardStop();
                break;
            case CowardDecisions.move:
                CowardMove();
                break;
            case CowardDecisions.runAway:
                CowardRunAway();
                break;
            case CowardDecisions.hit:
                CowardHit();
                break;
        }

    }

    /// <summary>
    /// Handles the coward stop state
    /// </summary>
    public void CowardStop()
    {

        // If it's waited long enough reset stage
        if (Time.time > CowardStopTimer)
            cowardDecision = CowardDecisions.nothing;
        else
        {
            // Rotate the direction
            if(CowardStopRotations != null)
            {

                foreach(int Rotation in CowardStopRotations)
                {

                    switch (Rotation)
                    {
                        // Left
                        case 0:
                            this.gameObject.transform.Rotate(15.0f * -this.transform.right * Time.deltaTime);
                            break;
                        // Right
                        case 1:
                            this.gameObject.transform.Rotate(15.0f * this.transform.right * Time.deltaTime);
                            break;
                        // Up
                        case 2:
                            this.gameObject.transform.Rotate(15.0f * this.transform.up * Time.deltaTime);
                            break;
                        // Down
                        case 3:
                            this.gameObject.transform.Rotate(15.0f * -this.transform.up * Time.deltaTime);
                            break;
                        // Forward
                        case 4:
                            this.gameObject.transform.Rotate(15.0f * this.transform.forward * Time.deltaTime);
                            break;
                        // Back
                        case 5:
                            this.gameObject.transform.Rotate(15.0f * -this.transform.forward * Time.deltaTime);
                            break;
                    }

                }

            }

        }

    }

    /// <summary>
    /// Handles the coward move state
    /// </summary>
    public void CowardMove()
    {

        // Check if we've move long enough
        if (Time.time > CowardMoveTimer)
            cowardDecision = CowardDecisions.nothing;
        else
        {
            // Check if we're going to run into an object
            RaycastHit[] hits = Physics.RaycastAll(this.transform.position, this.transform.forward, 100.0f);

            for(int i = 0; i < hits.Length; i++)
            {
                // We're going to run into the level reset
                if(hits[i].collider.gameObject == level)
                {
                    cowardDecision = CowardDecisions.nothing;
                    Debug.Log("Going to run into something!");
                    break;
                }
            }

            // Move the object
            this.transform.position += (15.0f * this.transform.forward * Time.deltaTime);
        }


        }

    /// <summary>
    /// Handles the coward run away state
    /// </summary>
    public void CowardRunAway()
    {

        // Check if player still within area
        CheckPlayer();
        if (!PlayerDetected)
        {

            if((Time.time - PlayerMissing) > 5.0f )
            {
                Debug.Log("Player outside radius for 5 seconds.");
                this.cowardDecision = CowardDecisions.nothing;
            }
        }

        // Look at player
        Quaternion direction = Quaternion.LookRotation(player.transform.position - this.transform.position);
        this.transform.rotation = Quaternion.Lerp(this.transform.rotation, direction, 15.0f * Time.deltaTime);

        // Run away from player
        this.transform.position += (15.0f * -this.transform.forward * Time.deltaTime);

    }

    /// <summary>
    /// Handles the coward hit state
    /// </summary>
    public void CowardHit()
    {

        // Check if player still within area
        CheckPlayer();
        if (!PlayerDetected)
        {

            if ((Time.time - PlayerMissing) > 5.0f)
            {
                Debug.Log("Player outside radius for 5 seconds.");
                this.cowardDecision = CowardDecisions.nothing;
            }
        }

        // Look away from player
        Quaternion direction = Quaternion.LookRotation(player.transform.position - this.transform.position);
        Vector3 euler = direction.eulerAngles;
        euler.y -= 180;
        euler.x = -euler.x;
        direction = Quaternion.Euler(euler);
        this.transform.rotation = Quaternion.Lerp(this.transform.rotation, direction, 5.0f * Time.deltaTime);

        // Run away from player
        this.transform.position += (30.0f * this.transform.forward * Time.deltaTime);

    }

    /// <summary>
    /// Perform the retaliation state
    /// </summary>
    public void Retaliation()
    {



    }

    /// <summary>
    /// Perform the AngreyBoi state
    /// </summary>
    public void AngreyBoi()
    {



    }

    /// <summary>
    /// Checks if the player is within radius
    /// </summary>
    private void CheckPlayer()
    {
        // Get the radius
        Collider[] colliders = Physics.OverlapSphere(this.gameObject.transform.position, 100.0f);

        // If the player is within the radius
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject == player)
            {
                if (!PlayerDetected)
                {
                    Debug.Log("Player in radius!");
                    PlayerDetected = true;
                    return;
                }
                return;

            }
        }

        if (PlayerDetected)
            PlayerDetected = false;
    }

    private void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, 100.0f);
    }

    public void TakeDamage()
    {
        Debug.Log("Take Damage");
        this.cowardDecision = CowardDecisions.hit;
    }

}
