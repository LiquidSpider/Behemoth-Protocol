using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Sal : MonoBehaviour
{

    // Stores the pathways
    private List<Path> paths;
    private Path currentPath;
    private GameObject currentMoveTo;
    private int pathIndex = 0;

    // Enemy variables
    public float speed;
    public float rotationSpeed;

    // player
    public GameObject player;

    private bool FinishedStartingPath = false;

    // Gattling Guns
    private GunTemplate[] gatlingGuns;
    private float gatlingShootTimer;
    private float gatlineReloadTimer;
    private int gatlingAmmo;
    public int gatlingMaxAmmo = 30;
    public float gatlingCooldown = 3.0f;
    public float gatlingShootPerSecond = 0.5f;
    private int gatlingShooter;

    // Missiles
    private MissileLauncher[] missileLaunchers;
    private float missileShootTimer;
    public GameObject missile;
    public float ShootMissileEveryXSeconds;

    private GameObject head;
    public GameObject beamStart;
    private float beamVelocity = 0.0f;
    private float beamChargeTime;
    public bool shootBeam;
    private bool shootingBeam;
    private bool facingPlayer
    {
        set
        {
            if (shootingBeam != true && value == true)
            {
                beamChargeTime = Time.time + 3.0f;
            }
            shootingBeam = value;
            beamStart.SetActive(value);
        }
        get
        {
            return shootingBeam;
        }
    }
    public float headRotationSpeed = 5.0f;
    private GameObject beam;
    private bool startedShooting;
    private float startedShootingTime;
    private float RotationIncrease = 1.0f;

    // body parts
    public List<GameObject> bodyParts;
    public List<float> bodyPartDistances;
    private GameObject currentBodyPart;
    private GameObject previousBodyPart;
    private float maxTime = 100.0f;
    private Collider[] colliders;

    public float BeamCheckCooldown;
    private float beamCheckTimer;

    private LayerMask IngoreLayer = ~(1 << 2 | 1 << 9 | 1 << 12);

    // Start is called before the first frame update
    void Start()
    {

        CollectPaths();
        GetGuns();

        this.gatlingAmmo = gatlingMaxAmmo;

        beam = beamStart.transform.GetChild(0).gameObject;
        head = this.gameObject.transform.GetChild(4).gameObject;

        beamCheckTimer = Time.time + BeamCheckCooldown;

        // Get all of this objects colliders
        colliders = this.transform.root.GetComponentsInChildren<Collider>();

    }

    /// <summary>
    /// Collect all the guns attached to the object.
    /// </summary>
    private void GetGuns()
    {

        gatlingGuns = this.gameObject.GetComponentsInChildren<GunTemplate>();
        missileLaunchers = this.gameObject.GetComponentsInChildren<MissileLauncher>();

    }

    /// <summary>
    /// Collect all the paths and create the pathways.
    /// </summary>
    private void CollectPaths()
    {

        // Get the paths
        GameObject sal_pathing = GameObject.Find("Sal-pathing");

        // Handle exceptions
        if (!sal_pathing)
        {
            throw new Exception("No sal_pathing object which contains the pathways for sal to move on!");
        }
        else if (sal_pathing.transform.childCount <= 0)
        {
            throw new Exception("No paths (child objects) within sal pathing. Create a new path.");
        }

        paths = new List<Path>();

        // for each path create the path
        for (int i = 0; i < sal_pathing.transform.childCount; i++)
        {

            List<GameObject> pathways = new List<GameObject>();

            for (int j = 0; j < sal_pathing.transform.GetChild(i).transform.childCount; j++)
            {

                pathways.Add(sal_pathing.transform.GetChild(i).transform.GetChild(j).gameObject);

            }

            //Debug.Log(pathways);

            // Make sure there is a path
            if(pathways.Count > 0)
            {

                // Create the path
                Path path = new Path(pathways);

                // Add the path to the lists
                paths.Add(path);

            }

        }

    }

    // Update is called once per frame
    void Update()
    {

        if(FinishedStartingPath)
        {

            if (!shootBeam)
            {
                MoveAroundPathWay();
                ShootPlayer();
            }
            else
            {
                ShootBeam();
            }

            // Wait for beam cool downbefore checking to shoot beam.
            if (Time.time > beamCheckTimer)
            {

                // Randomly Check whether to shoot beam
                if (UnityEngine.Random.Range(0, 500) <= 1)
                {

                    shootBeam = true;
                    beamCheckTimer = Time.time + BeamCheckCooldown;

                }

            }

        }
        else
        {

            StartingAnimation();

        }

    }

    /// <summary>
    /// Performs the starting animation.
    /// </summary>
    private void StartingAnimation()
    {

        if(currentPath == null)
        {
            currentPath = paths[0];
            CheckPath();
        }

        // If we're not at the current path
        if (Vector3.Distance(currentMoveTo.transform.position, bodyParts[0].transform.position) > 10.0f)
        {

            Quaternion direction = Quaternion.LookRotation(currentMoveTo.transform.position - bodyParts[0].transform.position, currentMoveTo.transform.up);
            bodyParts[0].transform.rotation = Quaternion.Lerp(bodyParts[0].transform.rotation, direction, rotationSpeed * Time.deltaTime);

            bodyParts[0].transform.position += bodyParts[0].transform.forward * Time.deltaTime * speed;


            // Move body parts with it
            for (int i = 1; i < bodyParts.Count; i++)
            {

                currentBodyPart = bodyParts[i];
                previousBodyPart = bodyParts[i - 1];

                // Get the distance between thos object and it's parent
                float distance = Vector3.Distance(previousBodyPart.transform.position, currentBodyPart.transform.position);

                // The new position is to be the position of the parent infront of it.
                Vector3 newPosition = previousBodyPart.transform.position;

                // Calcalate the speed if the time it takes is the distance to travel at the speed;
                float T = Time.deltaTime * distance / bodyPartDistances[i] * speed;

                // Lock speed
                if (T > maxTime)
                    T = maxTime;

                currentBodyPart.transform.position = Vector3.Slerp(currentBodyPart.transform.position, newPosition, T);
                currentBodyPart.transform.rotation = Quaternion.Slerp(currentBodyPart.transform.rotation, previousBodyPart.transform.rotation, T);

            }

        }
        else
        {

            // Increment the path
            pathIndex++;
            // Check finished
            FinishedStartingPath = FinishedPath();
            // Check the path for next path
            CheckPath();

        }

    }

    /// <summary>
    /// Select a path and move around the pathway.
    /// </summary>
    private void MoveAroundPathWay()
    {

        // If we don't have a path
        //if (currentPath == null)
		if (pathIndex == 0)
        {
			// get a random path - Do not get the first path
            int i = UnityEngine.Random.Range(1, paths.Count - 1);
            currentPath = paths[i];
            CheckPath();
        }

        // If we're not at the current path
        if (Vector3.Distance(currentMoveTo.transform.position, bodyParts[0].transform.position) > 10.0f)
        {

            Quaternion direction = Quaternion.LookRotation(currentMoveTo.transform.position - bodyParts[0].transform.position, currentMoveTo.transform.up);
            bodyParts[0].transform.rotation = Quaternion.Lerp(bodyParts[0].transform.rotation, direction, rotationSpeed * Time.deltaTime);

            bodyParts[0].transform.position += bodyParts[0].transform.forward * Time.deltaTime * speed;


            // Move body parts with it
            for (int i = 1; i < bodyParts.Count; i++)
            {

                currentBodyPart = bodyParts[i];
                previousBodyPart = bodyParts[i - 1];

                // Get the distance between thos object and it's parent
                float distance = Vector3.Distance(previousBodyPart.transform.position, currentBodyPart.transform.position);

                // The new position is to be the position of the parent infront of it.
                Vector3 newPosition = previousBodyPart.transform.position;

                // Calcalate the speed if the time it takes is the distance to travel at the speed;
                float T = Time.deltaTime * distance / bodyPartDistances[i] * speed;

                // Lock speed
                if (T > maxTime)
                    T = maxTime;

                currentBodyPart.transform.position = Vector3.Slerp(currentBodyPart.transform.position, newPosition, T);
                currentBodyPart.transform.rotation = Quaternion.Slerp(currentBodyPart.transform.rotation, previousBodyPart.transform.rotation, T);

            }

        }
        else
        {

            // Increment the path
            pathIndex++;
            // Check the path for next path
            CheckPath();

        }

    }

    /// <summary>
    /// Check if we're done with the path
    /// </summary>
    private void CheckPath()
    {
        if(pathIndex < currentPath.pathways.Count)
        {
            currentMoveTo = currentPath.pathways[pathIndex];
        }
        else
        {
            // reset the current path
            //currentPath = null;
            pathIndex = 0;
        }
    }

    /// <summary>
    /// Check if we've finished the path
    /// </summary>
    /// <returns>True if we finished</returns>
    private bool FinishedPath()
    {
        if (pathIndex < currentPath.pathways.Count)
        {
            return false;
        }
        else
        { 
            return true;
        }
    }

    /// <summary>
    /// Shoots the player.
    /// </summary>
    private void ShootPlayer()
    {

        // Move the gattling guns so they always look at the player
        foreach (GunTemplate gatlingGun in gatlingGuns)
        {

            Quaternion direction = Quaternion.LookRotation(player.transform.position - gatlingGun.gameObject.transform.position);
            gatlingGun.transform.rotation = Quaternion.Lerp(gatlingGun.gameObject.transform.rotation, direction, 50.0f * Time.deltaTime);

        }

        if(Time.time > missileShootTimer)
        {
         
            // Shoot Missiles
            foreach (MissileLauncher missileLauncher in missileLaunchers)
            {

                LaunchMissile(missileLauncher.gameObject);
                LaunchMissile(missileLauncher.gameObject);

            }

            missileShootTimer = Time.time + ShootMissileEveryXSeconds;

        }

        // GatlingGuns
        // Reload
        if (gatlingAmmo <= 0)
        {
            gatlingAmmo = gatlingMaxAmmo;
            gatlineReloadTimer = Time.time + gatlingCooldown;
        }

        // Don't shoot if reloading
        if(Time.time > gatlineReloadTimer)
        {
         
            // shoot gattlings
            if (Time.time > gatlingShootTimer)
            {
                
                RaycastHit hit;
                Physics.Raycast(gatlingGuns[gatlingShooter].gameObject.transform.position, gatlingGuns[gatlingShooter].gameObject.transform.forward, out hit, 5.0f);
                //if(hit.collider)
                //{
                //    Debug.Log(hit.collider.gameObject.name);
                //}

                if (Array.IndexOf(colliders, hit.collider) < 0)
                    gatlingGuns[gatlingShooter].Fire();

                if (gatlingShooter >= gatlingGuns.Length - 1)
                {
                    gatlingShooter = 0;
                }
                else
                {
                    gatlingShooter++;
                }

                // Remove this ammo from ammobank
                if (gatlingAmmo > 0)
                    gatlingAmmo--;

                // wait per shot
                gatlingShootTimer = Time.time + gatlingShootPerSecond;

            }

        }

    }

    /// <summary>
    /// Launches a missile at the player.
    /// </summary>
    private void LaunchMissile(GameObject spawner)
    {
		int spawnLoc = UnityEngine.Random.Range(5, 11);

        // Create a new missile
        GameObject newMissile = Instantiate(missile);
        // set it's location to the missile spawn location
        newMissile.transform.position = transform.GetChild(spawnLoc).position;
        // Set the player in the script
        newMissile.GetComponent<MissileBehaviour>().player = player.gameObject;
        // Create the missilebehaviour
        newMissile.GetComponent<MissileBehaviour>().Initialise(gameObject, spawner);

    }

    /// <summary>
    /// Shoots the beam at the player.
    /// </summary>
    private void ShootBeam()
    {

        // once we started shooting
        if(!startedShooting)
        {
            startedShooting = true;
            startedShootingTime = Time.time;
        }

        if(CheckFacingPlayer() || facingPlayer)
        {

            // if we aren't already facing the player.
            if(!facingPlayer)
            {
                facingPlayer = true;
                RotationIncrease = 1.0f;
            }

            // Shoot for 3 seconds to charge
            if (Time.time > beamChargeTime)
            {
                shootBeam = false;
                facingPlayer = false;
                RotationIncrease = 1.0f;
                startedShooting = false;
            }
            else
            { 
                // Shoot the beam until it hits an object
                RaycastHit hit;

                if (Physics.SphereCast(beamStart.transform.position, 3.0f, head.transform.forward, out hit, Mathf.Infinity, IngoreLayer))
                {

                    if (hit.collider)
                    {

                        if (hit.collider.gameObject.transform.root.gameObject.tag == "Environment")
                        {

                            // Smooth movement
                            float xscale1 = Mathf.SmoothDamp(beam.transform.localScale.x, hit.distance / 2 + 5.0f, ref beamVelocity, 0.01f);
                            beam.transform.localScale = new Vector3(xscale1, beam.transform.localScale.y, beam.transform.localScale.z);

                        }
                        else
                        {

                            // Smooth movement
                            float xscale = Mathf.SmoothDamp(beam.transform.localScale.x, hit.distance / 2 + 50.0f, ref beamVelocity, 0.01f);
                            beam.transform.localScale = new Vector3(xscale, beam.transform.localScale.y, beam.transform.localScale.z);

                        }

                    }

                }
            }

        }
        else
        {

            // Increase head rotation speed until we find the player
            RotationIncrease += (Time.time - startedShootingTime) / 5.0f;

        }

        // make the head look at the player
        Quaternion direction = Quaternion.LookRotation(player.transform.position - head.gameObject.transform.position);
        head.transform.rotation = Quaternion.Lerp(head.gameObject.transform.rotation, direction, headRotationSpeed * RotationIncrease * Time.deltaTime);

    }

    /// <summary>
    /// Checks if the head is facing the player
    /// </summary>
    public bool CheckFacingPlayer()
    {

        if(Vector3.Dot(bodyParts[0].transform.forward, (player.transform.position - bodyParts[0].transform.position).normalized) > 0.999f)
        {
            return true;
        }
        else
        {
            return false;
        }

    }

}

public class Path
{

    public List<GameObject> pathways;

    public Path(List<GameObject> pathways)
    {

        this.pathways = pathways;

    }

}
