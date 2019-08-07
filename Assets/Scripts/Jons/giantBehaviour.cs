using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class giantBehaviour : MonoBehaviour
{

    private Rigidbody body;

    //Variables to track where the player is located easily
    public GameObject player;
    private GameManager gameManager;
    public float distToFront = 0.0f;
    public float distToBack = 0.0f;

    public GameObject[] DFspawners = new GameObject[4];

    public GameObject missile;
    public GameObject[] missileLaunchers = new GameObject[2];
    public List<GameObject> activeLaunchers = new List<GameObject>();
    private float launchTime = 0.0f;

    // Moving variables
    private bool Moveable;
    public int currentMoveableIndex = 0;
    public float totalTimeTaken;
    private float maxVelocity;
    private float movementSpeed;
    private float rotationSpeed;
    private float pathingDistance;
    public GameObject pathwayParent;
    public GameObject[] pathways;

    private bool reachedDam = false;

    // Lazer Variables
    public bool _shootLazer;
    private bool shootLazer
    {
        get { return _shootLazer; }
        set { if (value == true && _shootLazer != value) { timerLazerStart = Time.time; } _shootLazer = value; }
    }
    public bool _shootingLazer;
    private bool shootingLazer
    {
        get { return _shootingLazer; }
        set { if (value == true && _shootingLazer != value) { timerLazerShootStart = Time.time; } _shootingLazer = value; }
    }
    public float lazerWindUpTime;
    public float lazerShootTime;
    public float lazerCooldownTime;
    private float timerLazerShootStart;
    private float timerLazerStart;



    // Start is called before the first frame update
    void Start()
    {
        // Collect the pathway.
        if (!pathwayParent)
        {
            pathwayParent = GameObject.FindGameObjectWithTag("PathWay");
            if (!pathwayParent)
            {
                Debug.Log("No gaint pathway exists.");
            }
            else
            {
                // get the pathways
                List<GameObject> pathwayList = new List<GameObject>();
                foreach (Transform child in pathwayParent.transform)
                {
                    pathwayList.Add(child.gameObject);
                }
                pathways = pathwayList.ToArray();
                pathwayList.Clear(); pathwayList = null;
            }
        }

        // allow movement
        Moveable = true;

        // get the game manager.
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        // get this objects rigidbody.
        body = this.GetComponent<Rigidbody>();

        // setup the rotation and speed.
        rotationSpeed = 100.0f;
        movementSpeed = 1.0f / 4.0f;
        pathingDistance = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        // Check Path
        CheckPath();

        // Move
        MoveOnPath();

        if (reachedDam)
            DestroyDam();

        locatePlayer();
        fireMissiles();

        CheckWeaponConditions();

        if (shootLazer)
            ShootLazers();
    }

    /// <summary>
    /// Enemy has reached the dam.
    /// </summary>
    private void DestroyDam()
    {
        gameManager.PlayerLose();
    }

    /// <summary>
    /// Checks when the increment the path.
    /// </summary>
    private void CheckPath()
    {
        // Check the distance from this object to the current pathing index excluding the Y axis
        if (currentMoveableIndex <= pathways.Length - 1)
        {
            Vector2 pathPos = new Vector2(pathways[currentMoveableIndex].transform.position.x, pathways[currentMoveableIndex].transform.position.z);
            Vector2 currentPos = new Vector2(transform.position.x, transform.position.z);

            // if this distance is within range increment path.
            if (Vector2.Distance(pathPos, currentPos) < pathingDistance)
            {
                currentMoveableIndex++;
                // reached the dam.
                if (currentMoveableIndex > pathways.Length - 1)
                {
                    Moveable = false;
                    reachedDam = true;
                }
            }
        }
    }

    /// <summary>
    /// Moves the gaint along the path
    /// </summary>
    private void MoveOnPath()
    {
        // If we can move
        if (Moveable)
        {
            // Move towards current path index
            // rotate towards target.
            Vector3 lookAt = pathways[currentMoveableIndex].transform.position;
            lookAt.y = 0;
            Vector3 currentPostion = this.transform.position;
            currentPostion.y = 0;
            Quaternion lookRotation = Quaternion.LookRotation(lookAt - currentPostion);
            transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, Mathf.Min(rotationSpeed * 20 * Time.deltaTime, 1));
            // add forward momemtum.
            this.body.MovePosition(this.transform.position + (this.transform.forward * movementSpeed));
        }
    }

    /// <summary>
    /// Runs the conditions to determine which attack is being used.
    /// </summary>
    private void CheckWeaponConditions()
    {
        // Lazer
        if (timerLazerStart <= Time.time - lazerCooldownTime)
        {
            shootLazer = true;
        }
    }

    //// getPathLength retreives the total distance to Giant has to move, in order to calculate how much it needs to move each frame
    //void getPathLength()
    //{
    //    float pathDisX = 0;
    //    float pathDisZ = 0;
    //    for (int i = 1; i < pathway.Length; i++)
    //    {
    //        pathDisX = pathway[i].transform.position.x - pathway[i - 1].transform.position.x;
    //        pathDisZ = pathway[i].transform.position.z - pathway[i - 1].transform.position.z;

    //        pathLength += Mathf.Sqrt(pathDisX * pathDisX + pathDisZ * pathDisZ);
    //    }
    //}

    //// getMoveDistance finds the distance the Giant should travel each frame
    //void getMoveDistance()
    //{
    //    // Total travel time should be 12min * 60
    //    int travTime = 10 * 60; //actually going for 10 mins
    //    moveDistance = pathLength / travTime;
    //}

    //// moveToTarget moves the Giant along its path to the dam wall
    //void moveToTarget()
    //{
    //    myself.transform.LookAt(currentTarget.transform);

    //    //Change target if giant has reached its current target
    //    if (Vector3.Distance(currentTarget.transform.position, myself.transform.position) <= 0.5f)
    //    {
    //        targetIndex++;
    //        currentTarget = pathway[targetIndex];
    //    }
    //    else
    //    {
    //        //Otherwise, head towards next target
    //        myself.transform.Translate(Vector3.forward * moveDistance * Time.deltaTime);
    //    }
    //}

    //// leftArmGone adjusts variables when the giant loses its arm
    //public void leftArmGone()
    //{
    //    leftArmSafe = false;
    //    LeftArm.SetActive(false);
    //}

    //// rightArmGone adjusts variables when the giant loses its arm
    //public void rightArmGone()
    //{
    //    rightArmSafe = false;
    //    RightArm.SetActive(false);
    //}

    //// legsGame adjusts variables when the giant loses its legs
    //public void legsGone()
    //{
    //    legsSafe = false;
    //    legDamagedTime = 0;
    //}

    //// fixLegs runs a timer that will return the giant to walking when the timer is up
    //void fixLegs()
    //{
    //    //Increment Timer
    //    legDamagedTime += Time.deltaTime;
    //    //If both arms are still operational
    //    if (rightArmSafe && leftArmSafe)
    //    {
    //        //Fix in 15 seconds
    //        if (legDamagedTime > 15.0f)
    //        {
    //            legsSafe = true;
    //            Legs.GetComponent<legsBehaviour>().legsFixed();
    //        }
    //        //If only 1 arm is operational - fix in 30 secs
    //    }
    //    else if (rightArmSafe && !leftArmSafe || leftArmSafe && !rightArmSafe)
    //    {
    //        if (legDamagedTime > 30.0f)
    //        {
    //            legsSafe = true;
    //            Legs.GetComponent<legsBehaviour>().legsFixed();
    //        }
    //        //if no arms are operational, don't do anything here
    //        //Although here it should do something else (in another script or later on)
    //    }
    //    else
    //    {
    //        return;
    //    }
    //}

    void locatePlayer()
    {
        //Calc distance to determine which missiles to launch
        distToFront = Vector3.Distance(player.transform.GetChild(0).position, missileLaunchers[0].transform.position);
        distToBack = Vector3.Distance(player.transform.GetChild(0).position, missileLaunchers[1].transform.position);
        //Debug.Log(distToFront + " " + distToBack);


        //Assign missiles and turrents to be 'awake' based on where player is
        //Missile launchers being assigned
        if (Mathf.Sqrt((distToBack - distToFront) * (distToBack - distToFront)) < 15.0f)
        {
            //When the player is near the sides of the machine
            activeLaunchers = new List<GameObject>();
        }
        else if (distToFront > distToBack)
        {
            //The player is behind the giant so make the front launchers active
            int launchNum = missileLaunchers[0].transform.childCount;
            activeLaunchers = new List<GameObject>();
            for (int i = 0; i < launchNum; i++)
            {
                activeLaunchers.Add(missileLaunchers[0].transform.GetChild(i).gameObject);
            }
        }
        else if (distToBack > distToFront)
        {
            //The player is in front of the giant so make the front launchers active
            int launchNum = missileLaunchers[1].transform.childCount;
            activeLaunchers = new List<GameObject>();
            for (int i = 0; i < launchNum; i++)
            {
                activeLaunchers.Add(missileLaunchers[1].transform.GetChild(i).gameObject);
            }
        }
    }

    //Put in the script for firing missiles here
    void fireMissiles()
    {
        launchTime += Time.deltaTime;

        if (launchTime > 10.0f)
        {
			float r;

            launchTime = 0.0f;
            foreach (GameObject launcher in activeLaunchers)
            {
				r = Random.Range(0f, 1f);

				if (r < 0.4f) launcher.GetComponent<missileLaunch>().fire();
            }
        }
    }

    /// <summary>
    /// Shoots lazers from the specified location.
    /// </summary>
    private void ShootLazers()
    {
        // wait the 2 second windup
        if (timerLazerStart <= Time.time - lazerWindUpTime)
        {
            // we can begin shooting
            if (!shootingLazer)
            {
                shootingLazer = true;
            }

            if (timerLazerShootStart >= Time.time - lazerShootTime)
            {
                // shoot the lazer
                //Debug.Log("Shooting Lazer");
            }
            else
            {
                // stop shooting
                shootLazer = false;
                shootingLazer = false;
                timerLazerStart = Time.time;
            }

        }
        else
        {
            // play windup sound
        }
    }
}
