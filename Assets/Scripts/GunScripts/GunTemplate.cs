using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class GunTemplate : MonoBehaviour {
    //  Gonna be a lot of variables here. Surely there's a way to clean it up.

    // AI Gun implementation
    public enum ControlledBy {
        player,
        AI
    }
    public ControlledBy controller;

    //  Gun Variables
    public AudioClip[] fireSounds = new AudioClip[4];         // All the sounds that can be fired
    public float fireRate = 0.1f;               // Seconds between each shot
    public float damage = 0;                    // Currently does nothing
    public GameObject barrel;                   // Bullet exit point
    public GameObject tracerSrc;                // LineRenderer prefab to use
    public float minAcc = 0f;                   // Accuracy when gun has not fired for a set time
    public float maxAcc = 30f;                  // Maximum inaccuracy after constant firing
    public float incAcc = 3f;                   // How much inaccuracy increases each shot
    public float handling = 1f;                 // How fast accuracy returns to minAcc between shots

    //  Bullet Variables                        All variables must start with prefix 'p'
    public GameObject projectile;               // Bullet prefab, probably won't need to change   
    public float pSpeed = 200f;                 // How fast the projectile moves
    public bool pGrav = false;                  // Does this projectile use gravity? Take weight into consideration
    public float pLightStr = 20f;               // How bright the light component is

    //  No need to touch these
    public float cAcc = 0f;                     // Current accuracy
    private float fireTime = 0f;                // Time since last shot
    private Animation animation;                // Fire animation
    private MakeSound makeSound;

    void Start() {
        cAcc = minAcc;                          // So guns don't start as accurate as the previous gun
        fireTime = fireRate + 1;                // So guns can fire immediately on swapping
        animation = GetComponent<Animation>();
        makeSound = GetComponent<MakeSound>();
    }
    // Update is called once per frame
    void Update() {

        // Depending on who is controlling the weapon stops the gun from shooting on the attack button
        switch (controller)
        {
            case ControlledBy.player:
                PlayerControl();
                break;
            case ControlledBy.AI:
                break;
        }
    }

    void PlayerControl() {
        fireTime += Time.deltaTime;
        if (cAcc >= minAcc) {
            float handlebuffer = cAcc / minAcc * 5;
            cAcc -= (handling + handlebuffer) * Time.deltaTime;
        }
        if (!GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<PlayerController>().isCruising) {
			if (Input.GetButton("Attack") || Input.GetAxis("Attack") != 0) {
				if (transform.root.root.GetComponent<PlayerHealth>().battery >= 150) {
					if (fireTime > fireRate) {

						if (GameObject.FindGameObjectWithTag("UI").transform.GetChild(2).GetComponent<WeaponSelect>().weaponNumber == 1) {

							//GameObject.FindGameObjectWithTag("UI").transform.GetChild(0).GetComponent<Image>().color = Color.red;

							Fire();
							transform.root.GetComponent<PlayerHealth>().UseBattery(50 * fireRate);
						}

						//if (GameObject.FindGameObjectWithTag("LeftSelect").GetComponent<WeaponSelect>().weaponNumber == 1) {

						//	GameObject.FindGameObjectWithTag("UI").transform.GetChild(0).GetComponent<Image>().color = Color.red;

						//	Fire();
						//	transform.root.GetComponent<PlayerHealth>().UseBattery(50 * fireRate);
						//}
					}
                }
            }
        }
    }

    public void Fire() {
		fireTime = 0;
        GameObject bullet = Instantiate(projectile, barrel.transform.position, barrel.transform.rotation);
        bullet.transform.Rotate(Vector3.up * (Random.Range(-cAcc, cAcc) / 10)); // Dividing by 10 so larger accuracy values can be input for balancing sake
        bullet.transform.Rotate(Vector3.left * (Random.Range(-cAcc, cAcc)) / 10);
        bullet.layer = 9;
        BulletScript bStats = bullet.GetComponent<BulletScript>();
        bStats.speed = pSpeed;
        bStats.damage = damage;
        bStats.grav = pGrav;
        bStats.lightStr = pLightStr;
        bullet.transform.parent = GameObject.FindGameObjectWithTag("MissileParent").transform;
        animation.Play("Fire", PlayMode.StopAll);
        cAcc += incAcc;
        CameraMotion recoil = FindObjectOfType<CameraMotion>();
        if (recoil != null)
        {
            recoil.Recoil(bullet.transform.rotation.eulerAngles);
        }
        FireSound();
    }

    void FireSound()
    {
        int sIndex = Random.Range(1, 5);
        switch (sIndex)
        {
            case 1:
                makeSound.Play(fireSounds[0]);
                break;
            case 2:
                makeSound.Play(fireSounds[1]);
                break;
            case 3:
                makeSound.Play(fireSounds[2]);
                break;
            case 4:
                makeSound.Play(fireSounds[3]);
                break;
        }

    }

    /// <summary>
    /// Get all the colliders in this objects parents.
    /// </summary>
    /// <returns>An array of all the parents colliders.</returns>
    private Collider[] GetParentColliders()
    {

        // Get the root parent
        Transform parent = gameObject.transform.root;
        // Get all the colliders in the object
        Collider[] colliders = parent.GetComponentsInChildren<Collider>();

        return colliders;

    }
}
