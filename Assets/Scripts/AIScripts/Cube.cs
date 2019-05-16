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
        AngreyBoi,
        Running
    }

    public enum CowardDecisions
    {
        nothing,
        stop,
        move,
        runAway,
        hit
    }

    public enum RetaliationDecisions
    {
        nothing,
        stop,
        move,
        aggressive,
        backAway,
    }

    public enum AngryBoiDecisions
    {
        nothing,
        aggressive,
        reposition
    }

    // Enum variables
    public CubeStates cubeState;
    public CubeStates lastCubeState;
    public CowardDecisions cowardDecision;
    public RetaliationDecisions retaliationDecision;
    public AngryBoiDecisions angryBoiDecision;

    private float retaliationAggressionTimer;

    // The guns
    private GameObject gattling1;
    private GameObject gattling2;
    private GameObject missle1;

    // public object variables
    public GameObject player;
    public GameObject level;

    // Face variables
    private GameObject currentFace;
    private GameObject[] faces;
    private GameObject defaultFace;
    private GameObject backwardsFace;
    private GameObject hitFace;
    private GameObject angryFace;
    private GameObject pissedFace;
    private float disableFaceTimer;

    // private object variables
    private float StopTimer;
    private int[] StopRotations;
    private float MoveTimer;

    // Weak spot variables
    private GameObject weakSpotBack;
    private GameObject weakSpotTop;
    private GameObject currentWeakSpot;
    public Material weakspotMaterial;
    public Material normalMaterial;

    public float Health = 500.0f;
    public float PlayerSpeed = 15.0f;
    public float rotationSpeed = 5.0f;
    private float roamSpeed = 0.7f;

    // Shooting variables
    private float shootTimer;
    private float gattlingCooldown;
    private float shotforTime;
    private float missileTimer;
    private bool fireMissileFirst;

    private bool _PlayerDetected = false;
    private float PlayerMissing;
    private bool PlayerDetected
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

        // I should really be using FindChild() and use error conditions but I'm super lazy....
        // Get the weak spot game objects
        this.weakSpotTop = this.transform.GetChild(0).transform.GetChild(0).gameObject;
        this.weakSpotBack = this.transform.GetChild(0).transform.GetChild(1).gameObject;
        
        // Store the current weakspot
        currentWeakSpot = weakSpotBack;

        // setup the cubes weakspots
        currentWeakSpot.GetComponent<Renderer>().material = weakspotMaterial;

        // Get the guns
        this.gattling1 = this.transform.GetChild(0).transform.GetChild(4).gameObject;
        this.gattling2 = this.transform.GetChild(0).transform.GetChild(5).gameObject;

        // Get all the faces
        this.angryFace = this.transform.GetChild(0).transform.GetChild(3).transform.GetChild(0).gameObject;
        this.backwardsFace = this.transform.GetChild(0).transform.GetChild(3).transform.GetChild(1).gameObject;
        this.defaultFace = this.transform.GetChild(0).transform.GetChild(3).transform.GetChild(2).gameObject;
        this.hitFace = this.transform.GetChild(0).transform.GetChild(3).transform.GetChild(3).gameObject;
        this.pissedFace = this.transform.GetChild(0).transform.GetChild(3).transform.GetChild(4).gameObject;

        // Add all the faces to a list
        faces = new GameObject[] { angryFace, backwardsFace, defaultFace, hitFace, pissedFace };

        // Set the current face to the default
        this.currentFace = defaultFace;

    }

    // Update is called once per frame
    void Update()
    {

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
            case CubeStates.Running:
                RunAway();
                break;
        }

        UpdateFace();

    }

    /// <summary>
    /// Handles the updating of the faces to match the current state.
    /// </summary>
    private void UpdateFace()
    {

        // Make sure not to override the hit face too quickly
        if (Time.time > disableFaceTimer)
        {

            // depending on the current cube state
            switch (cubeState)
            {

                case CubeStates.Coward:
                    // depending on the coward decisions
                    switch (cowardDecision)
                    {
                        case CowardDecisions.move:
                        case CowardDecisions.stop:
                        case CowardDecisions.nothing:
                        case CowardDecisions.hit:
                            ActiveFace(defaultFace);
                            break;
                        case CowardDecisions.runAway:
                            ActiveFace(backwardsFace);
                            break;
                    }
                    break;
                case CubeStates.Retaliation:
                    ActiveFace(angryFace);
                    break;
                case CubeStates.AngreyBoi:
                    ActiveFace(pissedFace);
                    break;

            }

        }

    }

    /// <summary>
    /// Actives the selected face and deactives all the others.
    /// </summary>
    /// <param name="face">The face to active.</param>
    private void ActiveFace(GameObject face)
    {

        // Active the current face
        face.SetActive(true);

        // Disable all the other faces
        for(int i = 0; i < faces.Length; i++)
        {

            if(faces[i].gameObject != face)
            {
                faces[i].SetActive(false);
            }

        }

    }

    /// <summary>
    /// Perform the coward state
    /// </summary>
    public void Coward()
    {

        // Check if player is within radius
        CheckPlayer(100.0f);
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
                    MoveTimer = Time.time + Random.Range(6, 10);
                    break;
                case 2:
                    cowardDecision = CowardDecisions.stop;
                    BuildStop();
                    break;
            }

        }

        // Execute current state
        switch (cowardDecision)
        {
            case CowardDecisions.stop:
                Stop();
                break;
            case CowardDecisions.move:
                Move();
                break;
            case CowardDecisions.runAway:
                CowardRunAway();
                break;
            case CowardDecisions.hit:
                CowardHit();
                break;
        }

    }

    #region CowardSpecific Decision

    /// <summary>
    /// Handles the coward run away state
    /// </summary>
    public void CowardRunAway()
    {

        // Check if player still within area
        CheckPlayer(100.0f);
        if (!PlayerDetected)
        {

            if((Time.time - PlayerMissing) > 5.0f )
            {
                this.cowardDecision = CowardDecisions.nothing;
            }
        }

        MoveAwayWhileLookingAtPlayer(1.1f);

    }

    /// <summary>
    /// Handles the coward hit state
    /// </summary>
    public void CowardHit()
    {

        // Check if player still within area
        CheckPlayer(100.0f);
        if (!PlayerDetected)
        {

            if ((Time.time - PlayerMissing) > 5.0f)
            {
                this.cowardDecision = CowardDecisions.nothing;
            }
        }

        // Look away from player
        Quaternion direction = Quaternion.LookRotation(player.transform.position - this.transform.position);
        Vector3 euler = direction.eulerAngles;
        euler.y -= 180;
        euler.x = -euler.x;
        direction = Quaternion.Euler(euler);
        this.transform.rotation = Quaternion.Lerp(this.transform.rotation, direction, rotationSpeed * Time.deltaTime);

        // Run away from player
        this.transform.position += (PlayerSpeed * 1.2f * this.transform.forward * Time.deltaTime);

    }

    #endregion

    /// <summary>
    /// Perform the retaliation state
    /// </summary>
    public void Retaliation()
    {

        // Check if player is within radius
        CheckPlayer(100.0f);
        if (PlayerDetected && retaliationDecision != RetaliationDecisions.aggressive)
            this.retaliationDecision = RetaliationDecisions.backAway;

        // If the cube is in "chill mode" go back to doing the coward stop / move methods
        if (this.retaliationDecision == RetaliationDecisions.nothing)
        {
            int DetermineChange = Random.Range(0, 3);

            switch (DetermineChange)
            {
                case 0:
                case 1:
                    retaliationDecision = RetaliationDecisions.move;
                    MoveTimer = Time.time + Random.Range(6, 10);
                    break;
                case 2:
                    retaliationDecision = RetaliationDecisions.stop;
                    BuildStop();
                    break;
            }

        }

        // Execute current state
        switch (retaliationDecision)
        {
            case RetaliationDecisions.stop:
                Stop();
                break;
            case RetaliationDecisions.move:
                Move();
                break;
            case RetaliationDecisions.backAway:
                RetaliationBackAway();
                break;
            case RetaliationDecisions.aggressive:
                RetaliationAggression();
                break;
        }

    }

    #region RetaliationSpecific Decisions

    /// <summary>
    /// Handles the normal backing away of the cube during the
    /// retaliation phase.
    /// This phase handles the gattling gun.
    /// </summary>
    public void RetaliationBackAway()
    {

        // Check if player still within area
        CheckPlayer(200.0f);
        if (!PlayerDetected)
        {

            if ((Time.time - PlayerMissing) > 5.0f)
            {
                this.retaliationDecision = RetaliationDecisions.nothing;
                fireMissileFirst = true;
            }
        }

        //Determine how close the player is
        float distance = CheckPlayerDistance();

        //If the player is within 1x the cube
        if (distance < 100.0f)
        {

            // Back away from the player at 1.3x player speed while looking at them and shooting
            MoveAwayWhileLookingAtPlayer(1.3f);

            // If we've waited for the 5 second cooldown
            if (Time.time > gattlingCooldown)
            {

                // Shoot for 5 seconds
                if(Time.time > shotforTime)
                {

                    // ResetCooldown
                    gattlingCooldown = Time.time + 5;
                    shotforTime = gattlingCooldown + 5;

                }

                // Shoot gattling at player
                if (Time.time > shootTimer)
                {

                    this.gattling1.GetComponent<GunTemplate>().Fire();
                    this.gattling2.GetComponent<GunTemplate>().Fire();

                    shootTimer = Time.time + 0.5f;

                }

            }

        }
        else
        {

            // Back away from the player at 1x player speed while looking at them
            MoveAwayWhileLookingAtPlayer(1f);

        }

    }

    /// <summary>
    /// Handles the reposition of the cube when taking damage during
    /// the retaliation phase.
    /// </summary>
    public void RetaliationAggression()
    {

        // Amount of time to be aggressive for
        if (Time.time > retaliationAggressionTimer)
            retaliationDecision = RetaliationDecisions.nothing;

        // Upon entering this state shoot 4 missiles
        if (fireMissileFirst)
        {

            Debug.Log("Fire missile 1 of 4");
            Debug.Log("Fire missile 2 of 4");
            Debug.Log("Fire missile 3 of 4");
            fireMissileFirst = false;

        }
        else
        {
             
            // fire a missile every 3 seconds
            if (Time.time > missileTimer)
            {

                Debug.Log("Fire Missile");

                missileTimer = Time.time + 3.0f;

            }

        }

        // Move within 2.5x radius of player

    }

    #endregion

    /// <summary>
    /// Perform the AngreyBoi state
    /// </summary>
    public void AngreyBoi()
    {

        if (angryBoiDecision == AngryBoiDecisions.nothing)
            angryBoiDecision = AngryBoiDecisions.aggressive;

        // Execute current state
        switch (angryBoiDecision)
        {
            case AngryBoiDecisions.reposition:
                AngryBoiReposition();
                break;
            case AngryBoiDecisions.aggressive:
                AngryBoiAggression();
                break;
        }

    }

    #region AngryBoi Decisions

    /// <summary>
    /// Performs the backaway state for the cube.
    /// </summary>
    private void AngryBoiReposition()
    {

    }

    /// <summary>
    /// Performs the aggressive state for the cube.
    /// </summary>
    private void AngryBoiAggression()
    {

        // Get the distance of the player from the cube
        float distance = CheckPlayerDistance();

        // Handle the weapons
        // 2.5x cube away Turret Fire
        if(distance <= 2.5 && distance > 1.5)
        {

            // If we've waited for the 5 second cooldown
            if (Time.time > gattlingCooldown)
            {

                // Shoot for 5 seconds
                if (Time.time > shotforTime)
                {

                    // ResetCooldown
                    gattlingCooldown = Time.time + 5;
                    shotforTime = gattlingCooldown + 5;

                }

                // Shoot gattling at player
                if (Time.time > shootTimer)
                {

                    this.gattling1.GetComponent<GunTemplate>().Fire();
                    this.gattling2.GetComponent<GunTemplate>().Fire();

                    shootTimer = Time.time + 0.5f;

                }

            }

        }
        // 1.5x cube away Charge
        else if(distance <= 1.5)
        {
            
        }
        // attack with missiles and beam
        else
        {

        }

        // Handles movement
        // Look at player
        Quaternion direction = Quaternion.LookRotation(player.transform.position - this.transform.position);

    }

    #endregion

    /// <summary>
    /// Returns the distance between the cube and the player
    /// </summary>
    private float CheckPlayerDistance()
    {

        // TODO apply offest to distance

        // Get the bounds of the object
        //Vector3 offset = this.transform.GetChild(0).gameObject.transform.GetChild(2).GetComponent<Mesh>().bounds.size / 2;

        // Get the distance from this object and the player
        float distance = Vector3.Distance(this.transform.position, player.transform.position);

        return distance;

    }

    /// <summary>
    /// Performs the state change run away
    /// </summary>
    public void RunAway()
    {
        
        // TODO Fix this so that the size / face / gun animations are played
        // Also add a 'safe' position to run too.

        // If we're safe change the state of the cube depending on the last state
        switch(lastCubeState)
        {
            case CubeStates.Coward:
                // update the weak point
                this.currentWeakSpot = weakSpotTop;
                currentWeakSpot.GetComponent<Renderer>().material = weakspotMaterial;
                weakSpotBack.GetComponent<Renderer>().material = normalMaterial;
                // update the cubes current state and reset it's decision
                this.cubeState = CubeStates.Retaliation;
                // Add the guns

                break;
            case CubeStates.Retaliation:
                // uppdate the weak point
                this.currentWeakSpot = weakSpotTop;
                currentWeakSpot.GetComponent<Renderer>().material = weakspotMaterial;
                weakSpotBack.GetComponent<Renderer>().material = normalMaterial;
                // Update the cubes current state and reset it's decision
                this.cubeState = CubeStates.AngreyBoi;
                break;
            case CubeStates.AngreyBoi:
                // update the weak point
                this.currentWeakSpot = weakSpotBack;
                currentWeakSpot.GetComponent<Renderer>().material = weakspotMaterial;
                weakSpotTop.GetComponent<Renderer>().material = normalMaterial;
                // update the current state and reset it's decision 
                this.cubeState = CubeStates.Coward;
                this.cowardDecision = CowardDecisions.nothing;
                break;
        }

    }

    #region NothingDecisionMovements

    /// <summary>
    /// Builds the variables to determine the rotation for the cube
    /// </summary>
    public void BuildStop()
    {
        // Determine how long to stop for
        StopTimer = Time.time + Random.Range(6, 10);
        // Determine which way to rotate
        int NumberOfRotations = Random.Range(0, 4);
        if (NumberOfRotations != 0)
        {
            StopRotations = new int[NumberOfRotations];
            if (NumberOfRotations == 3)
            {
                StopRotations[0] = Random.Range(0, 2);
                StopRotations[1] = Random.Range(2, 4);
                StopRotations[2] = Random.Range(4, 6);
            }
            else if (NumberOfRotations == 2)
            {
                StopRotations[0] = Random.Range(0, 4);
                StopRotations[1] = Random.Range(4, 6);
            }
            else
            {
                StopRotations[0] = Random.Range(0, 6);
            }
        }
        else
        {
            StopRotations = null;
        }
    }

    /// <summary>
    /// Handles the coward stop state
    /// </summary>
    public void Stop()
    {

        // If it's waited long enough reset stage
        if (Time.time > StopTimer)
        {
            switch (cubeState)
            {
                case CubeStates.Coward:
                    cowardDecision = CowardDecisions.nothing;
                    break;
                case CubeStates.Retaliation:
                    retaliationDecision = RetaliationDecisions.nothing;
                    break;
                case CubeStates.AngreyBoi:
                    angryBoiDecision = AngryBoiDecisions.nothing;
                    break;
            }
        }
        else
        {
            // Rotate the direction
            if (StopRotations != null)
            {

                foreach (int Rotation in StopRotations)
                {

                    switch (Rotation)
                    {
                        // Left
                        case 0:
                            this.gameObject.transform.Rotate(rotationSpeed * -this.transform.right * Time.deltaTime);
                            break;
                        // Right
                        case 1:
                            this.gameObject.transform.Rotate(rotationSpeed * this.transform.right * Time.deltaTime);
                            break;
                        // Up
                        case 2:
                            this.gameObject.transform.Rotate(rotationSpeed * this.transform.up * Time.deltaTime);
                            break;
                        // Down
                        case 3:
                            this.gameObject.transform.Rotate(rotationSpeed * -this.transform.up * Time.deltaTime);
                            break;
                        // Forward
                        case 4:
                            this.gameObject.transform.Rotate(rotationSpeed * this.transform.forward * Time.deltaTime);
                            break;
                        // Back
                        case 5:
                            this.gameObject.transform.Rotate(rotationSpeed * -this.transform.forward * Time.deltaTime);
                            break;
                    }

                }

            }

        }

    }

    /// <summary>
    /// Handles the coward move state
    /// </summary>
    public void Move()
    {

        // Check if we've move long enough
        if (Time.time > MoveTimer)
        {
            switch (cubeState)
            {
                case CubeStates.Coward:
                    cowardDecision = CowardDecisions.nothing;
                    break;
                case CubeStates.Retaliation:
                    retaliationDecision = RetaliationDecisions.nothing;
                    break;
                case CubeStates.AngreyBoi:
                    angryBoiDecision = AngryBoiDecisions.nothing;
                    break;
            }
        }
        else
        {
            // Check if we're going to run into an object
            RaycastHit[] hits = Physics.RaycastAll(this.transform.position, this.transform.forward, 100.0f);

            for (int i = 0; i < hits.Length; i++)
            {
                // We're going to run into the level reset
                if (hits[i].collider.gameObject == level)
                {
                    cowardDecision = CowardDecisions.nothing;
                    break;
                }
            }

            // Move the object
            this.transform.position += (PlayerSpeed * roamSpeed * this.transform.forward * Time.deltaTime);
        }


    }

    /// <summary>
    /// Moves the object backwards while looking at the player.
    /// </summary>
    /// <param name="speedMultiplier">The playerspeed multiplier amount.</param>
    public void MoveAwayWhileLookingAtPlayer(float speedMultiplier)
    {

        // Look at player
        Quaternion direction = Quaternion.LookRotation(player.transform.position - this.transform.position);
        this.transform.rotation = Quaternion.Lerp(this.transform.rotation, direction, rotationSpeed * Time.deltaTime);

        // Run away from player
        this.transform.position += (PlayerSpeed * speedMultiplier * -this.transform.forward * Time.deltaTime);

    }

    #endregion

    /// <summary>
    /// Checks if the player is within radius
    /// </summary>
    private void CheckPlayer(float distance)
    {
        // Get the radius
        Collider[] colliders = Physics.OverlapSphere(this.gameObject.transform.position, distance);

        // If the player is within the radius
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject == player)
            {
                if (!PlayerDetected)
                {
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
        Color color = Color.red;
        color.a /= 100.0f;
        Gizmos.color = color;
        Gizmos.DrawSphere(transform.position, 100.0f);
    }

    /// <summary>
    /// Handles the bullet collision damage.
    /// </summary>
    /// <param name="damage">The amount of damage to take.</param>
    public void TakeDamage(float damage)
    {

        if(cubeState != CubeStates.Running)
        {

            // Depeding on the current state do hit event
            switch(cubeState)
            {
                case CubeStates.Coward:
                    this.cowardDecision = CowardDecisions.hit;
                    break;
                case CubeStates.Retaliation:
                    if (retaliationDecision != RetaliationDecisions.aggressive)
                        fireMissileFirst = true;
                    this.retaliationDecision = RetaliationDecisions.aggressive;
                    // be aggressive for 10-20 seconds
                    retaliationAggressionTimer = Time.time + Random.Range(10, 20);
                    break;
                case CubeStates.AngreyBoi:
                    this.angryBoiDecision = AngryBoiDecisions.aggressive;
                    break;
            }

            // Take daamge
            Health -= damage;
        
            // Set the face of the object to being hit
            ActiveFace(hitFace);
            // Disable the updating of the current face for 0.3 seconds
            disableFaceTimer = Time.time + 0.1f;

            if(Health <= 0)
            {
                Health = 0;
                NoHealth();
            }

        }

    }

    /// <summary>
    /// Handles the missiles colliding with the weakspot.
    /// </summary>
    /// <param name="damage">The amount of damage to take.</param>
    public void MissileHitWeakSpot(float damage)
    {

        // Take daamge
        TakeDamage(damage);

        // Exeute the no health method as the cube has 0 health and the weak spot is it.
        if (Health <= 0)
        {
            NoHealth();
        }

    }
    
    /// <summary>
    /// Executes the stage changes upon hitting 0 health.
    /// </summary>
    public void NoHealth()
    {

        // Reset the health depending no what state this is
        switch(this.cubeState)
        {
            case CubeStates.Coward:
                Health = 700;
                break;
            case CubeStates.Retaliation:
                Health = 1000;
                break;
            case CubeStates.AngreyBoi:
                Health = 500;
                break;
        }

        // Store the current state when switching
        this.lastCubeState = this.cubeState;

        // Run away
        this.cubeState = CubeStates.Running;

    }

}
