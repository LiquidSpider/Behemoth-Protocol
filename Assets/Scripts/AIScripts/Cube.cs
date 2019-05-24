using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
	public GameObject infoText;

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
    public GameObject missile;
    public GameObject missileSpawnLocation;

    // public object variables
    public GameObject player;
    public GameObject level;
    public GameObject safePosition;

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

    private float radiusMovementReset;
    private int randomDirection;

    private bool sizeIncreaseFinished;
    private Vector3 defaultSize;
    private float startingAnimationTime;

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

        // Get the default size of the object
        defaultSize = transform.localScale;

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
        MoveObjectDirection(this.transform.forward, 1.2f);

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

			LaunchMissile();
			LaunchMissile();
			LaunchMissile();
			fireMissileFirst = false;

        }
        else
        {
             
            // fire a missile every 3 seconds
            if (Time.time > missileTimer)
            {
                
                LaunchMissile();
                missileTimer = Time.time + 3.0f;

            }

        }

        // Move within 2.5x radius of player
        MoveWithinRadiusOfPlayer(250f, 1.5f);

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

        // Move around the object at crazy speed
        MoveWithinRadiusOfPlayer(250, 2.5f);

        // Shoot gattling at player
        if (Time.time > shootTimer)
        {

            this.gattling1.GetComponent<GunTemplate>().Fire();
            this.gattling2.GetComponent<GunTemplate>().Fire();

            shootTimer = Time.time + 0.5f;

        }

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
        if(distance <= 250.0f && distance > 150.0f)
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

            // Handles movement
            MoveWithinRadiusOfPlayer(250, 1.5f);

        }
        // 1.5x cube away Charge
        else if(distance <= 1.5)
        {
            
        }
        // attack with missiles and beam
        else
        {

            // fire a missile every 3 seconds
            if (Time.time > missileTimer)
            {

                LaunchMissile();
                missileTimer = Time.time + 3.0f;

            }

            // Handles movement
            MoveWithinRadiusOfPlayer(250, 1.5f);

        }

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

        if(Vector3.Distance(this.transform.position, safePosition.transform.position) > 10.0f)
        {
            // Get the direction of the safe place
            Quaternion direction = Quaternion.LookRotation(safePosition.transform.position - this.transform.position);
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, direction, rotationSpeed * Time.deltaTime);

            // Run to position
            MoveObjectDirection(this.transform.forward, 2.5f);
        }

        // change the state of the cube depending on the last state
        switch(lastCubeState)
        {
            case CubeStates.Coward:
                // update the weak point
                this.currentWeakSpot = weakSpotTop;
                currentWeakSpot.GetComponent<Renderer>().material = weakspotMaterial;
                weakSpotBack.GetComponent<Renderer>().material = normalMaterial;
                // update the cubes current state and reset it's decision
                if(this.transform.localScale != defaultSize * 2)
                {
                    this.transform.localScale = Vector3.Slerp(defaultSize, defaultSize * 2, (Time.time - startingAnimationTime) / 2);
                }

                if (this.transform.localScale == defaultSize * 2 && Vector3.Distance(this.transform.position, safePosition.transform.position) < 10.0f)
                {
                    this.cubeState = CubeStates.Retaliation;
                }
                // Add the guns

                break;
            case CubeStates.Retaliation:
                // uppdate the weak point
                this.currentWeakSpot = weakSpotTop;
                currentWeakSpot.GetComponent<Renderer>().material = weakspotMaterial;
                weakSpotBack.GetComponent<Renderer>().material = normalMaterial;
                // Update the cubes current state and reset it's decision
                if(this.transform.localScale != defaultSize * 4)
                {
                    this.transform.localScale = Vector3.Slerp(defaultSize * 2, defaultSize * 4, (Time.time - startingAnimationTime) / 2);
                }

                if (this.transform.localScale == defaultSize * 4 && Vector3.Distance(this.transform.position, safePosition.transform.position) < 10.0f)
                {
                    this.cubeState = CubeStates.AngreyBoi;
                }
                break;
            case CubeStates.AngreyBoi:
                // update the weak point
                this.currentWeakSpot = weakSpotBack;
                currentWeakSpot.GetComponent<Renderer>().material = weakspotMaterial;
                weakSpotTop.GetComponent<Renderer>().material = normalMaterial;
                // update the current state and reset it's decision 
                if(this.transform.localScale != defaultSize)
                {
                    this.transform.localScale = Vector3.Slerp(defaultSize * 4, defaultSize, (Time.time - startingAnimationTime) / 2);
                }

                if (this.transform.localScale == defaultSize && Vector3.Distance(this.transform.position, safePosition.transform.position) < 10.0f)
                {
                    this.cubeState = CubeStates.Coward;
                    this.cowardDecision = CowardDecisions.nothing;
                }
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
            MoveObjectDirection(this.transform.forward, roamSpeed);

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
        MoveObjectDirection(-this.transform.forward, speedMultiplier);

    }

    /// <summary>
    /// Move around infront of the player keeping a specific distance.
    /// </summary>
    private void MoveWithinRadiusOfPlayer(float radius, float speedMultiplier)
    {

        // Get the current distance between the player and the cube
        float distance = CheckPlayerDistance();

        // If we're outside the radius
        if(distance > radius)
        {

            // Move towards player
            // Look at player
            Quaternion direction = Quaternion.LookRotation(player.transform.position - this.transform.position);
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, direction, rotationSpeed * Time.deltaTime);

            // Run towards from player
            MoveObjectDirection(this.transform.forward, speedMultiplier);


        }
        // We're inside the radius
        else if(distance < radius)
        {

            // Move away from player
            // Look at player
            Quaternion direction = Quaternion.LookRotation(player.transform.position - this.transform.position);
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, direction, rotationSpeed * Time.deltaTime);

            // Move the cube
            MoveObjectDirection(-this.transform.forward, speedMultiplier);

        }

        // Move a random direction for 2-4 seconds of time around the player
        if(Time.time > radiusMovementReset)
        {
            radiusMovementReset = Time.time + Random.Range(2,5);
            randomDirection = Random.Range(0, 4);
        }
        else
        {

            // Depending on the current direction
            switch(randomDirection)
            {
                // Move up
                case 0:
                    MoveObjectDirection(this.transform.up, 1.5f);
                    break;
                // Move down
                case 1:
                    MoveObjectDirection(-this.transform.up, 1.5f);
                    break;
                // Move left
                case 2:
                    MoveObjectDirection(-this.transform.right, 1.5f);
                    break;
                // Move right
                case 3:
                    MoveObjectDirection(this.transform.right, 1.5f);
                    break;
            }

        }


    }

    /// <summary>
    /// Moves the object a direction whilst maintaining a collision free path.
    /// </summary>
    /// <param name="direction">The direction of movement.</param>
    /// <param name="speedMultiplier">The speed mulitplier</param>
    private void MoveObjectDirection(Vector3 direction, float speedMultiplier)
    {
        // TODO
        Debug.DrawLine(this.transform.position, (direction.normalized * 100) + this.transform.position);
        // Cast a ray in the direction of movement to check for collision
        RaycastHit[] hits = Physics.RaycastAll(this.transform.position, direction, 100.0f);
        RaycastHit hit = new RaycastHit();

        if(hits.Length > 0)
        {
        }

        for(int i = 0; i < hits.Length; i++)
        { 
            if(hits[i].collider.gameObject != player)
            {

                hit = hits[i];
                break;

            }
        }

        // If the ray hit an object check horizontal paths of collision normal
        if(hit.transform != null)
        {

            var rotatedNormal = Quaternion.Euler(90, 90, 90) * hit.normal;
            direction = Vector3.Project(direction, rotatedNormal);

        }

        // move the object
        this.transform.position += (PlayerSpeed * speedMultiplier * direction * Time.deltaTime);

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
            disableFaceTimer = Time.time + 0.05f;

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

        if(cubeState != CubeStates.Running)
        {

            // Take daamge
            TakeDamage(damage);

            // Exeute the no health method as the cube has 0 health and the weak spot is it.
            if (Health <= 0)
            {
                NoHealth();
            }

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
				infoText.SetActive(true);
				Health = 700;
                break;
            case CubeStates.Retaliation:
                Health = 1000;
                break;
            case CubeStates.AngreyBoi:
				GameOver();
				break;
                //Health = 500;
                //break;
        }

        // Store the current state when switching
        this.lastCubeState = this.cubeState;

        // Run away
        this.cubeState = CubeStates.Running;

        // Calculate the safe place to move too
        startingAnimationTime = Time.time;

    }

    /// <summary>
    /// Launches a missile at the player.
    /// </summary>
    private void LaunchMissile()
    {

        // Create a new missile
        GameObject newMissile = Instantiate(missile);
        // set it's location to the missile spawn location
        newMissile.transform.position = this.transform.position;
		newMissile.transform.localScale = new Vector3(2.5f, 2.5f, 2.5f);
        // Set the player in the script
        newMissile.GetComponent<MissileBehaviour>().player = player.gameObject;
        // Create the missilebehaviour
        newMissile.GetComponent<MissileBehaviour>().Initialise(gameObject, null);

    }

	private void GameOver() {
		Destroy(gameObject);
	}

}
