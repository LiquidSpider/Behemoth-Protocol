using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonFlySpawner : MonoBehaviour
{

    public float cooldownLength;
    public float numberOfDragonFlys;
    public float spawnRate;

    // timers
    private float cooldownTimer;
    private float spawnRateTimer;

    // dragon fly variables
    public GameObject dragonFly;
    private GameObject dragonFlyParent;
    public GameManager gameManager;

    private GameObject[] spawners;
    private int counter = 0;

    public bool _spawn;
    public bool spawn
    {
        get
        {
            return _spawn;
        }
        set
        {
            if (value == true && _spawn != value)
            {
                cooldownTimer = Time.time + cooldownLength;
            }
            _spawn = value;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // Get the player
        this.dragonFlyParent = GameObject.FindGameObjectWithTag("DragonFlyParent");
        // store the gameManager.
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();

        if (!dragonFlyParent)
        {
            Debug.Log("object with DragonFlyParent tag not found. Disabling DragonFlySpawner script.");
            this.gameObject.SetActive(false);
        }
        if (!gameManager)
        {
            Debug.Log("object with gameManager tag not found. Disabling DragonFlySpawner script.");
            this.gameObject.SetActive(false);
        }
    }

    private void Awake()
    {
        // Get the spawners that are children of this object.
        List<GameObject> spawnerList = new List<GameObject>();
        Transform[] children = this.transform.GetComponentsInChildren<Transform>();
        foreach (Transform child in children)
        {
            if (child.gameObject.name.ToLower().Contains("spawner"))
            {
                if (child.gameObject != this.gameObject)
                    spawnerList.Add(child.gameObject);
            }
        }
        spawners = spawnerList.ToArray();
        spawnerList.Clear(); spawnerList = null;
    }

    // Update is called once per frame
    void Update()
    {

        // Check the spawn condition
        SpawnCondition();

        if (spawn)
        {
            // wait the cooldown timer
            if (cooldownTimer <= Time.time)
            {
                if (spawnRateTimer <= Time.time)
                {
                    // spawn the dragon flies on the rate.
                    SpawnDragonFly();
                    // set cooldown.
                    spawnRateTimer = Time.time + spawnRate;
                }
            }

            // Check to stop spawning
            if (gameManager.dragonFlies.Count >= numberOfDragonFlys)
            {
                // activate the dragon flies.
                foreach (GameObject dragonFly in gameManager.dragonFlies)
                {
                    dragonFly.GetComponent<DragonFly>().flyTowardsTarget = true;
                    dragonFly.GetComponent<DragonFly>().currentDragonFlyBehaviour = DragonFly.DragonFlyBehaviour.Attacking;
                }
                spawn = false;
            }
        }
    }

    /// <summary>
    /// Determines if we should start spawning dragonflies.
    /// </summary>
    /// <returns></returns>
    private void SpawnCondition()
    {
        // Check if there are any dragon flies left.
        if (gameManager.dragonFlies.Count == 0)
        {
            spawn = true;
        }
    }

    /// <summary>
    /// Spawns a dragon fly at a spawner incrementing through the number of spawners.
    /// </summary>
    void SpawnDragonFly()
    {
        if (counter < spawners.Length)
        {
            // spawn the dragonfly.
            GameObject newDragonFly = Instantiate(dragonFly, spawners[counter].transform.position, spawners[counter].transform.rotation);
            newDragonFly.transform.SetParent(dragonFlyParent.transform);
            counter++;
        }
        else
        {
            // reset the counter.
            counter = 0;
        }
    }
}
