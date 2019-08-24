using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class giantBehaviour : MonoBehaviour
{

    /// <summary>
    /// This objects rigidbody.
    /// </summary>
    private Rigidbody body;

    /// <summary>
    /// The player object.
    /// </summary>
    private GameObject player;

    /// <summary>
    /// The gamemanger object.
    /// </summary>
    private GameManager gameManager;

    /// <summary>
    /// The dragonfly spawners
    /// </summary>
    public GameObject[] DFspawners = new GameObject[4];

    /// <summary>
    /// The missile object to instantiate upon firing missile
    /// </summary>\
    [Header("Missile Objects")]
    public GameObject missile;

    /// <summary>
    /// A missile launcher objects.
    /// </summary>
    public GameObject[] missileLaunchers = new GameObject[2];

    private float launchTime = 0.0f;

    /// <summary>
    /// The active launchers to shoot from upon fire missile.
    /// </summary>
    public List<GameObject> activeLaunchers = new List<GameObject>();

    [Header("Hand collider and Mesh Parents")]
    public GameObject RightHandCollider;
    public GameObject RightHandMesh;
    public GameObject LeftHandCollider;
    public GameObject LeftHandMesh;

    private GiantAnimator animator;

    [System.NonSerialized]
    public BaseHealth LegsHealth;
    [System.NonSerialized]
    public BaseHealth LeftArmHealth;
    [System.NonSerialized]
    public BaseHealth RightArmHealth;
    [System.NonSerialized]
    public BaseHealth baseHealth;

    // Moving variables
    private int currentMoveableIndex = 0;
    //public float totalTimeTaken;
    private float maxVelocity;
    private float movementSpeed;
    private float rotationSpeed;
    private float pathingDistance;
    private GameObject pathwayParent;
    private GameObject[] pathways;

    // Lazer Variables
    [Header("Laser Variables")]
    public float lazerWindUpTime;
    public float lazerShootTime;
    [System.NonSerialized]
    public float LaserTimer;

    [Header("Smoke Variables")]
    public GameObject[] RightArmSmoke;
    public GameObject[] LeftArmSmoke;
    public GameObject[] LegSmoke;
    public GameObject[] BodySmoke;

    /// <summary>
    /// The different states the enemy can be in.
    /// </summary>
    public enum EnemyState
    {
        idle,
        moving,
        attacking,
        repairing,
        destoryingDam
    }
    /// <summary>
    /// The current state that the enemy is in.
    /// </summary>
    [Header("Enemy State Variables")]
    public EnemyState currentEnemyState;

    public enum EnemyArmStates
    {
        both,
        left,
        right,
        none
    }
    public EnemyArmStates armState;

    private enum PlayerPosition
    {
        unknown,
        infront,
        infrontRight,
        infrontLeft,
        right,
        left,
        back,
        backRight,
        backLeft
    }

    private float attackTimer = 0;

    /// <summary>
    /// The last enemy attack.
    /// </summary>
    private GiantAnimator.Animation lastAnimation;

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
                this.gameObject.SetActive(false);
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

        // get the game manager.
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        Setup();

    }

    /// <summary>
    /// Performs the initial setup of the giant.
    /// </summary>
    private void Setup()
    {
        // setup the rotation and speed.
        rotationSpeed = 100.0f;
        movementSpeed = 1.0f / 4.0f;
        pathingDistance = 10.0f;

        // get this objects rigidbody.
        body = this.GetComponent<Rigidbody>();

        // Default state.
        currentEnemyState = EnemyState.idle;

        // Get the animator script
        animator = this.GetComponent<GiantAnimator>();

        // Setup the first attack timer
        attackTimer = Random.Range(5, 10);

        // Setup the arm state
        armState = EnemyArmStates.both;

        // Get the player
        this.player = GameObject.FindGameObjectWithTag("Player").transform.root.gameObject;

        // Debug
        if (!player)
        {
            Debug.Log("No Object with 'Player' tag found. Giant Disabled");
            this.gameObject.SetActive(false);
        }

        // setup the health
        BaseHealth[] healths = this.GetComponents<BaseHealth>();
        foreach (BaseHealth health in healths)
        {
            Debug.Log(health.healthLayer.value);
            if (health.healthLayer == 1 << 14)
            {
                baseHealth = health;
            }
            else if (health.healthLayer == 1 << 16)
            {
                LegsHealth = health;
            }
            else if (health.healthLayer == 1 << 17)
            {
                LeftArmHealth = health;
            }
            else if (health.healthLayer == 1 << 18)
            {
                RightArmHealth = health;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

        CheckHealth();

        RunAIState();
    }

    /// <summary>
    /// Performs the AI state.
    /// </summary>
    private void RunAIState()
    {
        switch (currentEnemyState)
        {
            case EnemyState.idle:
                currentEnemyState = EnemyState.moving;
                break;
            case EnemyState.moving:
                // Run all the conditional statements for the movement step.
                RunMovementChecks();

                // Move on the path.
                MoveOnPath();

                // Fire missiles at the player.
                fireMissiles();
                break;
            case EnemyState.attacking:
                // Shoot laser if laser animation is at correct state
                if (animator.currentAnimation == GiantAnimator.Animation.Laser)
                {
                    if (animator.currentLaserState == GiantAnimator.LaserAnimationState.shoot)
                    {
                        ShootLaser();
                    }
                }
                // If the animation is complete.
                if (animator.isComplete || animator.currentAnimation == GiantAnimator.Animation.idle)
                {
                    animator.isComplete = false;
                    animator.currentAnimation = GiantAnimator.Animation.idle;
                    currentEnemyState = EnemyState.idle;
                    attackTimer = Time.time + Random.Range(5, 10);
                }
                // perform an attack.
                break;
            case EnemyState.repairing:
                // perform the repair.
                break;
            case EnemyState.destoryingDam:
                // Destroy the dam.
                DestroyDam();
                break;
        }
    }

    #region Conditions

    /// <summary>
    /// Checks for the health of the objects
    /// </summary>
    private void CheckHealth()
    {

        // Check for smoke enabled
        if (LegsHealth.health < LegsHealth.startingHealth * 0.8f)
        {
            SmokeActivity(true, LegSmoke);
        }
        if (LeftArmHealth.health < LeftArmHealth.startingHealth * 0.8f)
        {
            SmokeActivity(true, LeftArmSmoke);
        }
        if (RightArmHealth.health < RightArmHealth.startingHealth * 0.8f)
        {
            SmokeActivity(true, RightArmSmoke);
        }
        if (baseHealth.health < baseHealth.startingHealth * 0.8f)
        {
            SmokeActivity(true, BodySmoke);
        }

        // Check if arms should be disabled
        switch (armState)
        {
            case EnemyArmStates.both:
                if (LeftArmHealth.isDead)
                {
                    armState = EnemyArmStates.right;
                    DestroyHand(GiantAnimator.Hand.left);
                }
                if (RightArmHealth.isDead)
                {
                    armState = EnemyArmStates.left;
                    DestroyHand(GiantAnimator.Hand.right);
                }
                break;
            case EnemyArmStates.left:
                if (LeftArmHealth.isDead)
                {
                    armState = EnemyArmStates.none;
                    DestroyHand(GiantAnimator.Hand.left);
                }
                break;
            case EnemyArmStates.right:
                if (RightArmHealth.isDead)
                {
                    armState = EnemyArmStates.none;
                    DestroyHand(GiantAnimator.Hand.right);
                }
                break;
            case EnemyArmStates.none:
                if (baseHealth.isDead)
                {
                    gameManager.PlayerWin();
                }
                break;
        }

        // Check if the leg has been killed.
        if (LegsHealth.isDead)
        {
            if (armState != EnemyArmStates.none)
            {
                // Reset the legs health.
                LegsHealth.Revive();
                // Reset the legs smoke
                SmokeActivity(false, LegSmoke);
            }
            else
            { 
                // Enabled chest damage.
                gameManager.PlayerWin();
            }
        }

        // Check if the base is dead.
        //if (baseHealth.isDead)
        //{
        //    gameManager.PlayerWin();
        //}
    }

    /// <summary>
    /// Runs the conditional checks on the movement step.
    /// </summary>
    private void RunMovementChecks()
    {
        // Check for current pathway.
        CheckPath();

        // Determine the position of the player
        PlayerPosition playerPostion = CheckPlayerPosition();

        // Determine if we should attack and which attack
        AttackConditioner(playerPostion);
    }

    /// <summary>
    /// Determines if we should attack and what attack.
    /// </summary>
    private void AttackConditioner(PlayerPosition playerPosition)
    {
        // if the player is in range.
        if (playerPosition != PlayerPosition.unknown)
        {
            // Check if we can attack
            if (Time.time >= attackTimer)
            {
                // decide an attack
                switch (playerPosition)
                {
                    case PlayerPosition.infront:
                        // Perform the infront attacks.
                        if (armState != EnemyArmStates.both && armState != EnemyArmStates.none)
                        {
                            animator.currentAnimation = GiantAnimator.Animation.Laser;

                            if (armState == EnemyArmStates.left)
                            {
                                animator.hand = GiantAnimator.Hand.left;
                            }
                            else
                            {
                                animator.hand = GiantAnimator.Hand.right;
                            }

                            // Update the state
                            currentEnemyState = EnemyState.attacking;
                        }
                        else
                        {
                            if (Random.Range(0, 2) == 0)
                            {
                                animator.currentAnimation = GiantAnimator.Animation.GiantClap;
                            }
                            else
                            {
                                animator.currentAnimation = GiantAnimator.Animation.Laser;
                                if (Random.Range(0, 2) == 0)
                                {
                                    animator.hand = GiantAnimator.Hand.left;
                                }
                                else
                                {
                                    animator.hand = GiantAnimator.Hand.right;
                                }
                            }
                            // Update the state
                            currentEnemyState = EnemyState.attacking;
                        }
                        break;
                    case PlayerPosition.infrontLeft:
                        // Perform the infront left attacks.
                        if (armState != EnemyArmStates.none && armState != EnemyArmStates.right)
                        {
                            animator.hand = GiantAnimator.Hand.left;

                            switch (Random.Range(0, 3))
                            {
                                case 0:
                                    animator.currentAnimation = GiantAnimator.Animation.GiantSwing;
                                    break;
                                case 1:
                                    animator.currentAnimation = GiantAnimator.Animation.GiantSwipeUp;
                                    break;
                                case 2:
                                    animator.currentAnimation = GiantAnimator.Animation.Laser;
                                    break;
                            }
                            // Update the state;
                            currentEnemyState = EnemyState.attacking;
                        }
                        break;
                    case PlayerPosition.left:
                    case PlayerPosition.backLeft:
                        // Perform the left attack.
                        if (armState != EnemyArmStates.none && armState != EnemyArmStates.right)
                        {
                            animator.hand = GiantAnimator.Hand.left;

                            switch (Random.Range(0, 2))
                            {
                                case 0:
                                    animator.currentAnimation = GiantAnimator.Animation.GiantSwing;
                                    break;
                                case 1:
                                    animator.currentAnimation = GiantAnimator.Animation.GiantSwipeUp;
                                    break;
                            }
                            // Update the state;
                            currentEnemyState = EnemyState.attacking;
                        }
                        break;
                    case PlayerPosition.infrontRight:
                        // Perform the infront right attacks.
                        if (armState != EnemyArmStates.none && armState != EnemyArmStates.left)
                        {
                            animator.hand = GiantAnimator.Hand.right;

                            switch (Random.Range(0, 3))
                            {
                                case 0:
                                    animator.currentAnimation = GiantAnimator.Animation.GiantSwing;
                                    break;
                                case 1:
                                    animator.currentAnimation = GiantAnimator.Animation.GiantSwipeUp;
                                    break;
                                case 2:
                                    animator.currentAnimation = GiantAnimator.Animation.Laser;
                                    break;
                            }
                            // Update the state;
                            currentEnemyState = EnemyState.attacking;
                        }
                        break;
                    case PlayerPosition.right:
                    case PlayerPosition.backRight:
                        // Perform the right attacks.
                        // Perform the left attack.
                        if (armState != EnemyArmStates.none && armState != EnemyArmStates.left)
                        {
                            animator.hand = GiantAnimator.Hand.right;

                            switch (Random.Range(0, 2))
                            {
                                case 0:
                                    animator.currentAnimation = GiantAnimator.Animation.GiantSwing;
                                    break;
                                case 1:
                                    animator.currentAnimation = GiantAnimator.Animation.GiantSwipeUp;
                                    break;
                            }
                            // Update the state;
                            currentEnemyState = EnemyState.attacking;
                        }
                        break;
                    case PlayerPosition.back:
                        // Perform the back attacks.
                        // Update the state
                        currentEnemyState = EnemyState.attacking;
                        break;
                }
            }
        }
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
                    currentEnemyState = EnemyState.destoryingDam;
                }
            }
        }
    }

    /// <summary>
    /// Checks the position of the player.
    /// </summary>
    private PlayerPosition CheckPlayerPosition()
    {
        PlayerPosition position;

        // Check if the player is close enough to the Giant.
        if (Vector3.Distance(player.transform.position, this.transform.position) < 500.0f)
        {
            Vector3 from = player.transform.position;
            from.y = 0;
            Vector3 to = this.transform.position;
            to.y = 0;
            Vector3 heading = (from - to).normalized;
            // left
            float angle = -Vector3.SignedAngle(heading, -this.transform.right, Vector3.up);

            // infront
            if (angle >= 60 && angle <= 120)
            {
                position = PlayerPosition.infront;
            }
            // infront-right
            else if (angle >= 120 && angle <= 150)
            {
                position = PlayerPosition.infrontRight;
            }
            // right
            else if ((angle >= 150 && angle <= 180) || (angle >= -180 && angle <= -150))
            {
                position = PlayerPosition.right;
            }
            // back-right
            else if (angle >= -150 && angle <= -120)
            {
                position = PlayerPosition.backRight;
            }
            // back
            else if (angle >= -120 && angle <= -60)
            {
                position = PlayerPosition.back;
            }
            // back-left
            else if (angle >= -60 && angle <= -30)
            {
                position = PlayerPosition.backLeft;
            }
            // left
            else if ((angle >= -30 && angle <= 0) || (angle >= 0 && angle <= 30))
            {
                position = PlayerPosition.left;
            }
            // infront-left
            else if (angle >= 30 && angle <= 60)
            {
                position = PlayerPosition.infrontLeft;
            }
            else
            {
                position = PlayerPosition.unknown;
            }
        }
        else
        {
            position = PlayerPosition.unknown;
        }

        return position;
    }
    #endregion

    #region ActionMethods

    /// <summary>
    /// Enemy has reached the dam.
    /// </summary>
    private void DestroyDam()
    {
        if (animator.currentAnimation != GiantAnimator.Animation.GiantDamPunch)
            animator.currentAnimation = GiantAnimator.Animation.GiantDamPunch;

        if (animator.isComplete)
        {
            gameManager.PlayerLose();
        }
    }

    /// <summary>
    /// Moves the gaint along the path
    /// </summary>
    private void MoveOnPath()
    {
        if (currentEnemyState == EnemyState.moving)
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
    /// Destroys the hand.
    /// </summary>
    /// <param name="hand"></param>
    private void DestroyHand(GiantAnimator.Hand hand)
    {
        switch (hand)
        {
            case GiantAnimator.Hand.left:
                LeftHandCollider.SetActive(false);
                LeftHandMesh.SetActive(false);
                Renderer[] Lrenderers = LeftHandMesh.GetComponentsInChildren<SkinnedMeshRenderer>();
                foreach (Renderer renderer in Lrenderers)
                {
                    renderer.enabled = false;
                }
                SmokeActivity(false, LeftArmSmoke);
                break;
            case GiantAnimator.Hand.right:
                RightHandCollider.SetActive(false);
                RightHandMesh.SetActive(false);
                Renderer[] Rrenderers = RightHandMesh.GetComponentsInChildren<SkinnedMeshRenderer>();
                foreach (Renderer renderer in Rrenderers)
                {
                    renderer.enabled = false;
                }
                SmokeActivity(false, RightArmSmoke);
                break;
        }
    }

    /// <summary>
    /// Handles the shooting of missiles.
    /// </summary>
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
    private void ShootLaser()
    {
        switch (animator.hand)
        {
            case GiantAnimator.Hand.left:
                break;
            case GiantAnimator.Hand.right:
                break;
        }
    }

    /// <summary>
    /// Changes the active state of a list of smoke objects.
    /// </summary>
    /// <param name="state"></param>
    /// <param name="smokeArray"></param>
    private void SmokeActivity(bool state, GameObject[] smokeArray)
    {
        foreach (GameObject smoke in smokeArray)
        {
            if (smoke.activeSelf != state)
            {
                smoke.SetActive(state);
            }
        }
    }
    #endregion

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

    //void locatePlayer()
    //{
    //    //Calc distance to determine which missiles to launch
    //    distToFront = Vector3.Distance(player.transform.GetChild(0).position, missileLaunchers[0].transform.position);
    //    distToBack = Vector3.Distance(player.transform.GetChild(0).position, missileLaunchers[1].transform.position);
    //    //Debug.Log(distToFront + " " + distToBack);


    //    //Assign missiles and turrents to be 'awake' based on where player is
    //    //Missile launchers being assigned
    //    if (Mathf.Sqrt((distToBack - distToFront) * (distToBack - distToFront)) < 15.0f)
    //    {
    //        //When the player is near the sides of the machine
    //        activeLaunchers = new List<GameObject>();
    //    }
    //    else if (distToFront > distToBack)
    //    {
    //        //The player is behind the giant so make the front launchers active
    //        int launchNum = missileLaunchers[0].transform.childCount;
    //        activeLaunchers = new List<GameObject>();
    //        for (int i = 0; i < launchNum; i++)
    //        {
    //            activeLaunchers.Add(missileLaunchers[0].transform.GetChild(i).gameObject);
    //        }
    //    }
    //    else if (distToBack > distToFront)
    //    {
    //        //The player is in front of the giant so make the front launchers active
    //        int launchNum = missileLaunchers[1].transform.childCount;
    //        activeLaunchers = new List<GameObject>();
    //        for (int i = 0; i < launchNum; i++)
    //        {
    //            activeLaunchers.Add(missileLaunchers[1].transform.GetChild(i).gameObject);
    //        }
    //    }
    //}
}
