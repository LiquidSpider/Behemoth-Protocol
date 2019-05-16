using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunTemplate : MonoBehaviour
{
    //  Gonna be a lot of variables here. Surely there's a way to clean it up.

    // AI Gun implementation
    public enum ControlledBy
    {
        player,
        AI
    }
    public ControlledBy controller;

    //  Gun Variables
    public int magSize = 0;                     // Set to 0 for infinite ammo
    public int pellets = 1;                     // Shots fired per "bullet"
    public float fireRate = 0.1f;               // Seconds between each shot
    public FireMode fMode = FireMode.Auto;      // Currently selected firemode
    public int burstNum = 3;                    // How many shots per burst
    public float damage = 0;                    // Currently does nothing
    public float impactForce = 20f;             // Force applied to shot object
    public GameObject barrel;                   // Bullet exit point
    public GameObject tracerSrc;                // LineRenderer prefab to use
    public int rAmmo = 0;                       // Ammo in reserve, not used when magSize = 0
    public float fireForce = 2f;                // Force applied to weapon when shot
    public float weight = 1f;                   // Force returning weapon to original spot
    public float minAcc = 0f;                   // Accuracy when gun has not fired for a set time
    public float maxAcc = 30f;                  // Maximum inaccuracy after constant firing
    public float incAcc = 3f;                   // How much inaccuracy increases each shot
    public float handling = 1f;                 // How fast accuracy returns to minAcc between shots
    public AudioClip sFire;                     // Sound played when gun fires
    public bool soundPitchRandom;               // Should the sound have randomized pitch? (Good for automatic weapons)

    //  Bullet Variables                        All variables must start with prefix 'p'
    public GameObject projectile;               // Bullet prefab, probably won't need to change   
    public float pSpeed = 200f;                 // How fast the projectile moves
    public float pDamage = 0f;                  // Does nothing right now, should be directly inherited from gun's GunTemplate
    public float pWeight = 0.01f;               // Weight of the projectile for impact force
    public bool pGrav = false;                  // Does this projectile use gravity? Take weight into consideration
    public bool pExplosive = false;             // Does this projectile do splash damage and apply splash force?
    public float pExpSize = 0f;                 // How large the explosion is
    public float pExpDamage = 0f;               // How much damage the explosion itself does. Will implement splash damage later
    public float pLightStr = 20f;               // How bright the light component is
    public float pPrjSize = 0.1f;               // How large the collider is. Should be visually represented

    //  No need to touch these
    private int cAmmo = 0;                      // Ammo currently loaded, not used when magSize = 0
    public float cAcc = 0f;                     // Current accuracy
    private float fireTime = 0f;                // Time since last shot
    public enum FireMode { Semi, Burst, Auto }; // Fire modes, self explainatory
    public GameObject soundSrc;                 // The object that makes the sound
    private bool isBursting = false;            // Is the gun currently going through a burst of fire?
    private bool hasFired = false;              // For semi-auto, as GetAxis is not a bool
    private Animation animation;

    void Start() {
        cAcc = minAcc;                          // So guns don't start as accurate as the previous gun
        fireTime = fireRate + 1;                // So guns can fire immediately on swapping
        animation = GetComponent<Animation>();  
    }
    // Update is called once per frame
    void Update() {
        
        // Depending on who is controlling the weapon stops the gun from shooting on the attack button
        switch(controller)
        {
            case ControlledBy.player:
                PlayerControl(fMode);
                break;
            case ControlledBy.AI:
                break;
        }
    }

    void PlayerControl(FireMode fMode) {
        fireTime += Time.deltaTime;
        if (cAcc >= minAcc) {
            float handlebuffer = cAcc / minAcc * 5;
            cAcc -= (handling + handlebuffer) * Time.deltaTime;
        }

		if (!GameObject.FindGameObjectWithTag("Player").transform.GetChild(1).GetComponent<PlayerController>().isCruising) {
			switch (fMode) {
				case FireMode.Semi:
					if (Input.GetButtonDown("Attack") || Input.GetAxis("Attack") != 0) {
						if (fireTime > fireRate && !hasFired) {
							hasFired = true;
							Fire();
						}
					}
					if (Input.GetAxis("Attack") < 1) {
						hasFired = false;
					}
					break;
				case FireMode.Burst:
					if (Input.GetButtonDown("Attack") || Input.GetAxis("Attack") != 0) {
						StartCoroutine(BurstFire());
					}
					break;
				case FireMode.Auto:
					if (Input.GetButton("Attack") || Input.GetAxis("Attack") != 0) {
						if (fireTime > fireRate) {
							Fire();
						}
					}
					break;
			}
		}

    }

    public void Fire() {
        fireTime = 0;
        for (int i = 0; i < pellets; i++) {

            GameObject bullet = Instantiate(projectile, barrel.transform.position, barrel.transform.rotation);
            bullet.transform.Rotate(Vector3.up * (Random.Range(-cAcc, cAcc) / 10)); // Dividing by 10 so larger accuracy values can be input for balancing sake
            bullet.transform.Rotate(Vector3.left * (Random.Range(-cAcc, cAcc)) / 10);
            BulletScript bStats = bullet.GetComponent<BulletScript>();
            bStats.creatorsColliders = GetParentColliders();
            bStats.speed = pSpeed;
            bStats.damage = damage;
            bStats.grav = pGrav;
            bStats.weight = pWeight;
            bStats.lightStr = pLightStr;
			bullet.transform.parent = GameObject.FindGameObjectWithTag("MissileParent").transform;
		}
        animation.Play("Fire", PlayMode.StopAll);
        MakeSound(sFire, sFire.length, soundPitchRandom);
        if (cAcc <= maxAcc) {
            cAcc += incAcc;
        }
    }

    void MakeSound(AudioClip sound, float sLength, bool pitchRandom) {
        AudioSource source = soundSrc.GetComponent<AudioSource>();
        source.clip = sFire;
        source.volume = 0.5f;
        if (pitchRandom) source.pitch = Random.Range(0.75f, 1.25f);
        GameObject oSound = Instantiate(soundSrc, barrel.transform.position, barrel.transform.rotation);
        oSound.GetComponent<TimedDestroy>().maxTime = sLength;
		oSound.transform.parent = GameObject.FindGameObjectWithTag("MissileParent").transform;
    }

    IEnumerator BurstFire() {
        if (!isBursting) {
            isBursting = true;
            for (int i = 0; i < burstNum; i++) {
                Fire();
                yield return new WaitForSeconds(fireRate);
            }
            isBursting = false;
        }
    }

    /// <summary>
    /// Get all the colliders in this objects parents.
    /// </summary>
    /// <returns>An array of all the parents colliders.</returns>
    private Collider[] GetParentColliders()
    {

        // Get the root parent
        Transform parent = this.gameObject.transform.root;
        // Get all the colliders in the object
        Collider[] colliders = parent.GetComponentsInChildren<Collider>();

        return colliders;

    }
}
