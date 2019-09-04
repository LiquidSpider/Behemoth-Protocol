using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    /// <summary>
    /// The position of the chest.
    /// </summary>
    public Transform ChestPosition;

    /// <summary>
    /// The game object for the pulse attack.
    /// </summary>
    public GameObject PulseEmitter;

    [Header("AttackTimer Variables")]
    public float minimumTimeBetweenAttac;
    public float maximumTimeBetweenAttack;

    [Header("Hand collider and Mesh Parents")]
    public GameObject RightHandCollider;
    public GameObject RightHandMesh;
    public GameObject LeftHandCollider;
    public GameObject LeftHandMesh;

    private GiantAnimator animator;
    private Animator thisAnimator;

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

    // Final Laser Variables
    private float FinalLaserWindUpTime = 15;
    private float FinalLaserShootTime = 5;
    [SerializeField]
    private float FinalLaserTime = 0;

    [Header("Laser Time Variables")]
    public GameObject[] LLaserObjects;
    public GameObject[] RLaserObjects;
    public GameObject ChestLaser;

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
        destoryingDam,
        lastStand
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

    public enum LastStandState
    {
        Idle,
        Default,
        FinalLaser,
        Pulse,
    }

    private enum Kinematics
    {
        leftArm,
        leftHand,
        rightArm,
        rightHand,
        leftLeg,
        rightLeg,
        Chest
    }

    /// <summary>
    /// The state of the last stand.
    /// </summary>
    [Header("Last Stand Variables")]
    public LastStandState lastStandState;

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
        backLeft,
        farinfront
    }

    private float attackTimer = 0;

    /// <summary>
    /// The last enemy attack.
    /// </summary>
    private GiantAnimator.Animation lastAnimation;

    /// <summary>
    /// The duration of the last stand before losing.
    /// </summary>
    [SerializeField]
    private float LastStandDuration;

    /// <summary>
    /// Final stand AOE emitter conditions.
    /// </summary>
    private bool FirstEmitterUsed = false;
    private bool SecondEmitterUsed = false;

    /// <summary>
    /// The timer for the emitter timer.
    /// </summary>
    private float EmitterStartTime = 0;

    /// <summary>
    /// The delay timer to wind up the emitter.
    /// </summary>
    public float EmitterWindUp;

    public Text promptText;

    private bool canMove;

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
        rotationSpeed = 1000.0f;
        movementSpeed = 1.0f / 10.0f;
        pathingDistance = 10.0f;

        // Last Stand is 3 minutes (180 seconds).
        LastStandDuration = 180.0f;

        // get this objects rigidbody.
        body = this.GetComponent<Rigidbody>();

        // Default state.
        currentEnemyState = EnemyState.idle;

        // Emitter disable
        FirstEmitterUsed = false;
        SecondEmitterUsed = false;

        // Get the animator script
        animator = this.GetComponent<GiantAnimator>();

        // Setup the first attack timer
        attackTimer = Random.Range(minimumTimeBetweenAttac, maximumTimeBetweenAttack);

        // Setup the arm state
        armState = EnemyArmStates.both;

        // Get the player
        this.player = GameObject.FindGameObjectWithTag("Player").transform.root.gameObject;

        // get the objects animator
        this.thisAnimator = this.GetComponent<Animator>();

        // Disable movement
        canMove = false;

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
        //pause
        if(gameManager.gamePaused)
        {

        }
        else
        {
            CheckHealth();

            RunAIState();
        }
    }

    /// <summary>
    /// Performs the AI state.
    /// </summary>
    private void RunAIState()
    {
        switch (currentEnemyState)
        {
            case EnemyState.idle:
                canMove = false;
                currentEnemyState = EnemyState.moving;
                break;
            case EnemyState.moving:
                canMove = true;
                // Run all the conditional statements for the movement step.
                RunMovementChecks();

                // Move on the path.
                MoveOnPath();

                // Fire Turrets

                // Fire Missiles
                fireMissiles();
                break;
            case EnemyState.attacking:
                // Check the paths
                CheckPath();

                // Move on the path.
                MoveOnPath();

                // Shoot laser if laser animation is at correct state
                if (animator.currentAnimation == GiantAnimator.Animation.Laser)
                {
                    if (animator.currentLaserState == GiantAnimator.LaserAnimationState.shoot)
                    {
                        ShootLaser();
                    }

                    if (animator.currentLaserState == GiantAnimator.LaserAnimationState.recover)
                    {
                        DisableLaser();
                    }
                }
                // If the animation is complete.
                if (animator.isComplete || animator.currentAnimation == GiantAnimator.Animation.idle)
                {
                    animator.isComplete = false;
                    animator.currentAnimation = GiantAnimator.Animation.idle;
                    this.thisAnimator.enabled = true;
                    currentEnemyState = EnemyState.idle;
                    attackTimer = Time.time + Random.Range(minimumTimeBetweenAttac, maximumTimeBetweenAttack);
                }
                // perform an attack.
                break;
            case EnemyState.repairing:
                canMove = false;
                // perform the repair.
                if (this.thisAnimator.GetBool("IsMoving"))
                {
                    // Enable animation.
                    this.thisAnimator.SetBool("IsMoving", false);
                }

                // Enable animation.
                this.thisAnimator.SetBool("IsRepairing", true);
                this.thisAnimator.SetBool("IsStanding", true);
                this.thisAnimator.SetBool("IsFalling", true);

                if (this.thisAnimator.GetCurrentAnimatorStateInfo(0).IsName("Stand"))
                {
                    // Reset the legs health.
                    LegsHealth.Revive();
                    // Reset the legs smoke
                    SmokeActivity(false, LegSmoke);
                }

                if(this.thisAnimator.GetCurrentAnimatorStateInfo(0).IsName("FinishStand"))
                {
                    // Reset the legs health.
                    LegsHealth.Revive();
                    // Reset the legs smoke
                    SmokeActivity(false, LegSmoke);
                    // Disable animation.
                    this.thisAnimator.SetBool("IsRepairing", false);
                    this.thisAnimator.SetBool("IsStanding", false);
                    this.thisAnimator.SetBool("IsFalling", false);
                    this.currentEnemyState = EnemyState.moving;
                }
                break;
            case EnemyState.destoryingDam:
                canMove = false;
                // Destroy the dam.
                DestroyDam();
                break;
            case EnemyState.lastStand:
                canMove = false;
                // Perform the last stand stage
                LastStand();
                if (!this.thisAnimator.GetBool("IsFalling"))
                {
                    this.thisAnimator.SetBool("IsFalling", true);
                }
                this.thisAnimator.SetBool("IsMoving", false);
                break;
        }
    }

    #region Conditions

    /// <summary>
    /// Runs the check conditions for the last stand state.
    /// </summary>
    private void LastStandCheckConditions()
    {
        // Second minute emittor
        if (this.LastStandDuration <= 120 && !this.FirstEmitterUsed)
        {
            this.lastStandState = LastStandState.Pulse;
            FirstEmitterUsed = true;
        }

        // 1 minute emittor.
        if (this.LastStandDuration <= 60 && !this.SecondEmitterUsed)
        {
            this.lastStandState = LastStandState.Pulse;
            SecondEmitterUsed = true;
        }

        // Final Stand
        if (this.LastStandDuration <= 20 && this.lastStandState != LastStandState.FinalLaser)
        {
            this.lastStandState = LastStandState.FinalLaser;
        }
    }

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

					// Navigator Prompt
					promptText.text = "Good job! Now you just have to destroy the other one.";
					promptText.gameObject.transform.parent.gameObject.SetActive(true);

				}
                if (RightArmHealth.isDead)
                {
                    armState = EnemyArmStates.left;
                    DestroyHand(GiantAnimator.Hand.right);

					// Navigator Prompt
					promptText.text = "Good job! Now you just have to destroy the other one.";
					promptText.gameObject.transform.parent.gameObject.SetActive(true);
				}
				break;
            case EnemyArmStates.left:
                if (LeftArmHealth.isDead)
                {
                    armState = EnemyArmStates.none;
                    DestroyHand(GiantAnimator.Hand.left);

					// Navigator Prompt
					promptText.text = "Nice! Now destroy the legs and it won't be able to go anywhere.";
					promptText.gameObject.transform.parent.gameObject.SetActive(true);
				}
				break;
            case EnemyArmStates.right:
                if (RightArmHealth.isDead)
                {
                    armState = EnemyArmStates.none;
                    DestroyHand(GiantAnimator.Hand.right);

					// Navigator Prompt
					promptText.text = "Nice! Now destroy the legs and it won't be able to go anywhere.";
					promptText.gameObject.transform.parent.gameObject.SetActive(true);
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
                currentEnemyState = EnemyState.repairing;
            }
            else
            {
                this.currentEnemyState = EnemyState.lastStand;
                baseHealth.takeDamage = true;
            }
        }

        //Check if the base is dead.
        if (baseHealth.isDead)
        {
            gameManager.PlayerWin();
        }
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
                    case PlayerPosition.back:
                    case PlayerPosition.infront:
                        // Perform the infront attacks.
                        if (armState != EnemyArmStates.both && armState != EnemyArmStates.none)
                        {
                            canMove = true;
                            animator.currentAnimation = GiantAnimator.Animation.Laser;

                            if (armState == EnemyArmStates.left)
                            {
                                animator.hand = GiantAnimator.Hand.left;
                                EnableKinematics(true, Kinematics.Chest);
                                EnableKinematics(true, Kinematics.leftArm);
                                EnableKinematics(true, Kinematics.leftHand);
                            }
                            else
                            {
                                animator.hand = GiantAnimator.Hand.right;
                                EnableKinematics(true, Kinematics.Chest);
                                EnableKinematics(true, Kinematics.rightArm);
                                EnableKinematics(true, Kinematics.rightHand);
                            }

                            // Update the state
                            currentEnemyState = EnemyState.attacking;
                        }
                        else
                        {
                            if (Random.Range(0, 2) == 0)
                            {
                                animator.currentAnimation = GiantAnimator.Animation.GiantClap;
                                
                                canMove = false;
                                EnableKinematics(true, Kinematics.Chest);
                                EnableKinematics(true, Kinematics.rightArm);
                                EnableKinematics(true, Kinematics.leftArm);
                                EnableKinematics(true, Kinematics.rightHand);
                                EnableKinematics(true, Kinematics.leftHand);
                                EnableKinematics(true, Kinematics.leftLeg);
                                EnableKinematics(true, Kinematics.rightLeg);
                            }
                            else
                            {
                                canMove = true;
                                animator.currentAnimation = GiantAnimator.Animation.Laser;
                                if (Random.Range(0, 2) == 0)
                                {
                                    animator.hand = GiantAnimator.Hand.left;
                                    EnableKinematics(true, Kinematics.Chest);
                                    EnableKinematics(true, Kinematics.leftArm);
                                    EnableKinematics(true, Kinematics.leftHand);
                                }
                                else
                                {
                                    animator.hand = GiantAnimator.Hand.right;
                                    EnableKinematics(true, Kinematics.Chest);
                                    EnableKinematics(true, Kinematics.rightArm);
                                    EnableKinematics(true, Kinematics.rightHand);
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

                            canMove = true;
                            switch (Random.Range(0, 3))
                            {
                                case 0:
                                    animator.currentAnimation = GiantAnimator.Animation.GiantSwing;
                                    EnableKinematics(true, Kinematics.Chest);
                                    EnableKinematics(true, Kinematics.leftArm);
                                    EnableKinematics(true, Kinematics.leftHand);
                                    break;
                                case 1:
                                    animator.currentAnimation = GiantAnimator.Animation.GiantSwipeUp;
                                    EnableKinematics(true, Kinematics.Chest);
                                    EnableKinematics(true, Kinematics.leftArm);
                                    EnableKinematics(true, Kinematics.leftHand);
                                    break;
                                case 2:
                                    animator.currentAnimation = GiantAnimator.Animation.Laser;
                                    EnableKinematics(true, Kinematics.Chest);
                                    EnableKinematics(true, Kinematics.leftArm);
                                    EnableKinematics(true, Kinematics.leftHand);
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

                            canMove = true;
                            switch (Random.Range(0, 2))
                            {
                                case 0:
                                    animator.currentAnimation = GiantAnimator.Animation.GiantSwing;
                                    EnableKinematics(true, Kinematics.Chest);
                                    EnableKinematics(true, Kinematics.leftArm);
                                    EnableKinematics(true, Kinematics.leftHand);
                                    break;
                                case 1:
                                    animator.currentAnimation = GiantAnimator.Animation.GiantSwipeUp;
                                    EnableKinematics(true, Kinematics.Chest);
                                    EnableKinematics(true, Kinematics.leftArm);
                                    EnableKinematics(true, Kinematics.leftHand);
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

                            canMove = true;
                            switch (Random.Range(0, 3))
                            {
                                case 0:
                                    animator.currentAnimation = GiantAnimator.Animation.GiantSwing;
                                    EnableKinematics(true, Kinematics.Chest);
                                    EnableKinematics(true, Kinematics.rightArm);
                                    EnableKinematics(true, Kinematics.rightHand);
                                    break;
                                case 1:
                                    animator.currentAnimation = GiantAnimator.Animation.GiantSwipeUp;
                                    EnableKinematics(true, Kinematics.Chest);
                                    EnableKinematics(true, Kinematics.rightArm);
                                    EnableKinematics(true, Kinematics.rightHand);
                                    break;
                                case 2:
                                    animator.currentAnimation = GiantAnimator.Animation.Laser;
                                    EnableKinematics(true, Kinematics.Chest);
                                    EnableKinematics(true, Kinematics.rightArm);
                                    EnableKinematics(true, Kinematics.rightHand);
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

                            canMove = true;
                            switch (Random.Range(0, 2))
                            {
                                case 0:
                                    animator.currentAnimation = GiantAnimator.Animation.GiantSwing;
                                    EnableKinematics(true, Kinematics.Chest);
                                    EnableKinematics(true, Kinematics.rightArm);
                                    EnableKinematics(true, Kinematics.rightHand);
                                    break;
                                case 1:
                                    animator.currentAnimation = GiantAnimator.Animation.GiantSwipeUp;
                                    EnableKinematics(true, Kinematics.Chest);
                                    EnableKinematics(true, Kinematics.rightArm);
                                    EnableKinematics(true, Kinematics.rightHand);
                                    break;
                            }
                            // Update the state;
                            currentEnemyState = EnemyState.attacking;
                        }
                        break;
                    case PlayerPosition.farinfront:
                        animator.currentAnimation = GiantAnimator.Animation.Laser;
                        EnableKinematics(true, Kinematics.Chest);
                        EnableKinematics(true, Kinematics.leftArm);
                        EnableKinematics(true, Kinematics.leftHand);
                        // Update the state;
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

			// Jonathan's Code was here

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
        else if(Vector3.Distance(player.transform.position, this.transform.position) < 2000.0f)
        {
			Vector3 from = player.transform.position;
            from.y = 0;
            Vector3 to = this.transform.position;
            to.y = 0;
            Vector3 heading = (from - to).normalized;
            // left
            float angle = -Vector3.SignedAngle(heading, -this.transform.right, Vector3.up);


			// Call to activate the missile launchers
			if (angle >= 0 && angle <= 180) {
				activeLaunchers.Clear();
				for (int i = 0; i < missileLaunchers[1].transform.childCount; i++) {
					activeLaunchers.Add(missileLaunchers[1].transform.GetChild(i).gameObject);
				}
			} else if (angle < 0 && angle >= 180) {
				activeLaunchers.Clear();
				for (int i = 0; i < missileLaunchers[1].transform.childCount; i++) {
					activeLaunchers.Add(missileLaunchers[0].transform.GetChild(i).gameObject);
				}
			}





			if (angle >= 60 && angle <= 120)
            {
                position = PlayerPosition.farinfront;
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
    /// Performs the last stand phase.
    /// </summary>
    private void LastStand()
    {

        switch (lastStandState)
        {
            case LastStandState.Idle:
                this.lastStandState = LastStandState.Default;
                break;

            case LastStandState.Default:
                LastStandCheckConditions();
                // Fire Turrets

                // Fire Missiles
                break;

            case LastStandState.Pulse:
                Pulse();
                break;

            case LastStandState.FinalLaser:
                Debug.Log("Final Lasering");
                FinalLaser();
                break;
        }

        // Increment the last stand duration.
        LastStandDuration -= Time.deltaTime;
    }

    /// <summary>
    /// Emits a pulse around the boss.
    /// </summary>
    private void Pulse()
    {
        if (EmitterStartTime > EmitterWindUp)
        {
            // Create the emitter object
            Instantiate(PulseEmitter, ChestPosition.position, ChestPosition.rotation);
            this.lastStandState = LastStandState.Default;
        }

        // Increment the timer.
        EmitterStartTime += Time.deltaTime;
    }

    /// <summary>
    /// Performs the final state.
    /// </summary>
    private void FinalLaser()
    {
        
        // If wind up is finished.
        if(FinalLaserTime > FinalLaserWindUpTime)
        {
            // Enabled the laser and it's script.
            ChestLaser.SetActive(true);
            ChestLaser.GetComponent<Laser>().enabled = true;

            if (FinalLaserTime > FinalLaserWindUpTime + FinalLaserShootTime)
            {
                ChestLaser.SetActive(false);
                ChestLaser.GetComponent<Laser>().enabled = false;
                gameManager.PlayerLose();
            }
        }

        // Increment the final laser timer.
        FinalLaserTime += Time.deltaTime;
    }

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
        if (canMove)
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
            this.body.position += (this.transform.forward * movementSpeed);

            // Enable animation.
            this.thisAnimator.SetBool("IsMoving", true);
        }
        else if(currentEnemyState == EnemyState.attacking)
        {
            this.thisAnimator.enabled = false;
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
                // Enabled the objects
                foreach (GameObject laser in LLaserObjects)
                {
                    laser.SetActive(true);
                    if (!laser.GetComponent<Laser>().enabled)
                    {
                        laser.GetComponent<Laser>().enabled = true;
                    }
                }
                break;
            case GiantAnimator.Hand.right:
                // Enabled the objects
                foreach (GameObject laser in RLaserObjects)
                {
                    laser.SetActive(true);
                    if (!laser.GetComponent<Laser>().enabled)
                    {
                        laser.GetComponent<Laser>().enabled = true;
                    }
                }
                break;
        }
    }

    /// <summary>
    /// Disables the laser.
    /// </summary>
    private void DisableLaser()
    {
        // Disable the left
        foreach (GameObject laser in LLaserObjects)
        {
            laser.SetActive(false);
            laser.GetComponent<Laser>().enabled = false;
        }
        // Disable the right
        foreach (GameObject laser in RLaserObjects)
        {
            laser.SetActive(false);
            laser.GetComponent<Laser>().enabled = false;
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
    
    /// <summary>
    /// Turns the kinematics on or off
    /// </summary>
    private void EnableKinematics(bool condition, Kinematics kinematics)
    {
        switch(kinematics)
        {
            case Kinematics.leftHand:
                foreach (GameObject kinematic in animator.LeftHandKinematics)
                {
                    kinematic.GetComponent<InverseKinematics>().enabled = condition;
                    //kinematic.GetComponent<InverseKinematics>().Reset();
                }
                break;
            case Kinematics.leftArm:
                foreach (GameObject kinematic in animator.LeftArmKinematics)
                {
                    kinematic.GetComponent<InverseKinematics>().enabled = condition;
                    //kinematic.GetComponent<InverseKinematics>().Reset();
                }
                break;
            case Kinematics.leftLeg:
                foreach (GameObject kinematic in animator.LeftLegKinematics)
                {
                    kinematic.GetComponent<InverseKinematics>().enabled = condition;
                    //kinematic.GetComponent<InverseKinematics>().Reset();
                }
                break;
            case Kinematics.rightArm:
                foreach (GameObject kinematic in animator.RightArmKinematics)
                {
                    kinematic.GetComponent<InverseKinematics>().enabled = condition;
                    //kinematic.GetComponent<InverseKinematics>().Reset();
                }
                break;
            case Kinematics.rightHand:
                foreach (GameObject kinematic in animator.RightHandKinematics)
                {
                    kinematic.GetComponent<InverseKinematics>().enabled = condition;
                    //kinematic.GetComponent<InverseKinematics>().Reset();
                }
                break;
            case Kinematics.rightLeg:
                foreach (GameObject kinematic in animator.RightLegKinematics)
                {
                    kinematic.GetComponent<InverseKinematics>().enabled = condition;
                    //kinematic.GetComponent<InverseKinematics>().Reset();
                }
                break;
            case Kinematics.Chest:
                foreach (GameObject kinematic in animator.ChestKinematics)
                {
                    kinematic.GetComponent<InverseKinematics>().enabled = condition;
                    //kinematic.GetComponent<InverseKinematics>().Reset();
                }
                break;
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
