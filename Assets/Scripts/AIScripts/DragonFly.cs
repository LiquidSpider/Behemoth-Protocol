using System.Collections;
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

    // on destory variables
    private bool isQuitting = false;
    public GameObject rubbishPile;

    // shooting variables
    public GameObject shootPostion;
    public GameObject bullet;
    public float shootTime;
    private float shootTimer;

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

    // Sound variables
    public List<AudioClip> flyClips = new List<AudioClip>();
    public List<AudioClip> deathClips = new List<AudioClip>();
    public List<AudioClip> explosionClips = new List<AudioClip>();
    private AudioSource source;

    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        // On spawn get the audio source
        source = transform.GetComponentInChildren<AudioSource>();
        source.clip = flyClips[UnityEngine.Random.Range(0, flyClips.Count)];
        source.loop = true;
        source.Play();
        // On spawn setup the collider list
        this.colliders = this.GetComponentsInChildren<Collider>();
        // Get the player object
        player = GameObject.FindWithTag("Player").transform.GetChild(0).transform;
        // Get the game manager
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        // Get the objects rigidbody.
        if (body == null)
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

        switch (currentDragonFlyBehaviour)
        {
            case DragonFlyBehaviour.idle:
                Idle();
                break;
            case DragonFlyBehaviour.Attacking:
                Attacking();
                // check if we should kamikaze
                BoidsLeft();
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
        if (gameManager.dragonFlies.Count <= 2)
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
        body.velocity = Vector3.ClampMagnitude(body.velocity, maxVelocity);
    }

    /// <summary>
    /// The idle state of the dragonfly.
    /// </summary>
    private void Idle()
    {
        // Do nothing
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
                Shoot();
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
        if (applySeperation)
        {
            Seperation();
        }

        if (applyAlignment)
        {
            Alignment();
        }

        if (applyCohesion)
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

        foreach (GameObject boid in gameManager.dragonFlies)
        {
            if (boid != this)
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

        if (counter > 0)
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

        foreach (GameObject boid in gameManager.dragonFlies)
        {
            if (boid != this)
            {
                float distance = Vector3.Distance(this.transform.position, boid.transform.position);
                // Only calculate if withing range.
                if ((distance > 0) && (distance < cohesionDistance))
                {
                    //speed = boid.GetComponent<DragonFly>().body.velocity;
                    alignment = boid.transform.forward;
                    counter++;
                }
            }
        }

        if (counter > 0)
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

        foreach (GameObject boid in gameManager.dragonFlies)
        {
            if (boid != this)
            {
                float distance = Vector3.Distance(this.transform.position, boid.transform.position);
                if ((distance > 0) && (distance < cohesionDistance))
                {
                    cohesion += boid.transform.position;
                    counter++;
                }
            }
        }

        if (counter > 0)
        {
            cohesion = cohesion * (1.0f / (counter));
            // pull towards position
            cohesion = (cohesion - this.transform.position).normalized;
            cohesion *= cohesionForce;
            this.body.AddForce(cohesion);
        }
    }

    #endregion

    /// <summary>
    /// Moves the target towards the position using relative forces.
    /// </summary>
    /// <param name="position"></param>
    private void MoveTowardsTarget(Vector3 position)
    {
        // If kamikaze mode triple speed and ignore distance
        if (this.currentDragonFlyBehaviour == DragonFlyBehaviour.Kamikaze)
        {
            // rotate towards target.
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(position - transform.position), Mathf.Min(defaultRotationSpeed * 20 * Time.deltaTime, 1));
            // add forward momemtum.
            body.AddForce(this.transform.forward * defaultSpeed);
        }
        else
        {
            // rotate towards target.
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(position - transform.position), Mathf.Min(defaultRotationSpeed * Time.deltaTime, 1));
            // add forward momemtum.
            body.AddForce(this.transform.forward * defaultSpeed);
        }
    }

    /// <summary>
    /// Shoots towards the direction it's facing.
    /// </summary>
    private void Shoot()
    {
        if (shootTimer < Time.time)
        {
            // Shoot
            Instantiate(bullet, shootPostion.transform.position, this.transform.rotation);
            shootTimer = Time.time + shootTime;
        }
    }

    /// <summary>
    /// When collider enters another collider.
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        // if we're kamikazeing and the player is the collider.
        if (currentDragonFlyBehaviour == DragonFlyBehaviour.Kamikaze)
        {
            // Blowup
            if (collision.collider.gameObject.tag == "Player")
                Destroy(this.gameObject);
        }

        // damage section.
        if (collision.gameObject.transform.tag == "Explosion - Player")
        {
            this.GetComponent<EnemyHealth>().TakeDamage(200, collision.gameObject);
        }

        if (collision.gameObject.transform.tag == "Bullet - Player")
        {
            this.GetComponent<EnemyHealth>().TakeDamage(10);
        }

        if (collision.collider.gameObject.layer != 13)
            this.transform.GetChild(1).GetComponent<ShowHealth>().DamageTaken();

    }

    /// <summary>
    /// When the application is being closed.
    /// </summary>
    private void OnApplicationQuit()
    {
        isQuitting = true;
    }

    /// <summary>
    /// Create a new audio source on the current location playing the death sounds
    /// </summary>
    private void DeathSound()
    {
        // Vocal death sound
        var dSound = new GameObject();
        dSound.transform.position = transform.position;
        dSound.AddComponent<AudioSource>();
        dSound.AddComponent<TimedDestroy>();
        dSound.GetComponent<AudioSource>().clip = deathClips[UnityEngine.Random.Range(0, deathClips.Count)];
        dSound.GetComponent<AudioSource>().spatialBlend = 1;
        dSound.GetComponent<AudioSource>().dopplerLevel = 0;
        dSound.GetComponent<AudioSource>().rolloffMode = AudioRolloffMode.Logarithmic;
        dSound.GetComponent<AudioSource>().spatialBlend = 1;
        dSound.GetComponent<AudioSource>().minDistance = 50;
        dSound.GetComponent<AudioSource>().maxDistance = 400;
        dSound.GetComponent<AudioSource>().volume = 0.8f;
        dSound.GetComponent<AudioSource>().Play();
        dSound.GetComponent<TimedDestroy>().maxTime = dSound.GetComponent<AudioSource>().clip.length;

        // Small explosion
        var eSound = new GameObject();
        eSound.transform.position = transform.position;
        eSound.AddComponent<AudioSource>();
        eSound.AddComponent<TimedDestroy>();
        eSound.GetComponent<AudioSource>().clip = explosionClips[UnityEngine.Random.Range(0, explosionClips.Count)];
        eSound.GetComponent<AudioSource>().spatialBlend = 1;
        eSound.GetComponent<AudioSource>().dopplerLevel = 0;
        eSound.GetComponent<AudioSource>().rolloffMode = AudioRolloffMode.Logarithmic;
        eSound.GetComponent<AudioSource>().spatialBlend = 1;
        eSound.GetComponent<AudioSource>().minDistance = 70;
        eSound.GetComponent<AudioSource>().maxDistance = 600;
        eSound.GetComponent<AudioSource>().volume = 1f;
        eSound.GetComponent<AudioSource>().Play();
        eSound.GetComponent<TimedDestroy>().maxTime = eSound.GetComponent<AudioSource>().clip.length;
    }

    /// <summary>
    /// On Destroy(this) event.
    /// </summary>
    private void OnDestroy()
    {
        if (!isQuitting)
        {
            // create a rubbish pile.
            Instantiate(rubbishPile, this.transform.localPosition, this.transform.localRotation);
            DeathSound();

            // remove self from the game manager
            if (gameManager.dragonFlies.Contains(this.gameObject))
            {
                try
                {
                    gameManager.dragonFlies.Remove(this.gameObject);
                }
                catch (Exception e)
                {
                }
            }
        }
    }
}