using System;
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

    [Tooltip("Element 0 = horizontal recoil. Element 1 = vertical recoi.")]
    public float[] recoilxy = new float[2] { 3, 10 }; //
    private float[] storedRecoil = new float[2];

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
    private Camera camera;                      // The camera component



    void OnValidate()
    {
        // Ensures recoilxy maintains 2 elements
        if (recoilxy.Length != 2)
        {
            Debug.LogWarning("RecoilXY requires 2 fields exactly.");
            Array.Resize(ref recoilxy, 2);
        }
    }

    void Start() {
        cAcc = minAcc;                          // So guns don't start as accurate as the previous gun
        fireTime = fireRate + 1;                // So guns can fire immediately on swapping
        animation = GetComponent<Animation>();
        makeSound = GetComponent<MakeSound>();
        camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        storedRecoil[0] = recoilxy[0];
        storedRecoil[1] = recoilxy[1];
    }
    // Update is called once per frame
    void Update() {
        //AimWeapon();
        // Depending on who is controlling the weapon stops the gun from shooting on the attack button
        switch (controller)
        {
            case ControlledBy.player:
                PlayerControl();
                break;
            case ControlledBy.AI:
                break;
        }

        switch (FindObjectOfType<PlayerController>().isZoom)
        {
            case true:
                recoilxy[0] = storedRecoil[0] / 2;
                recoilxy[1] = storedRecoil[1] / 2;
                break;
            case false:
                recoilxy[0] = storedRecoil[0];
                recoilxy[1] = storedRecoil[1];
                break;
        }
        
    }

    void AimWeapon()
    {

		// Altered this to remove the jitteryness of the gatling gun, it works, probs could do it better
		RaycastHit hit;

		int layerMask = LayerMask.GetMask("Default", "Player"); //1 << 10;
		layerMask = ~layerMask;

		if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, Mathf.Infinity, layerMask)) {  //Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)), out hit)) { //Camera.main.transform.position, Camera.main.transform.forward, out hit)) {
			transform.GetChild(0).LookAt(hit.point);

			// Without this the gun is always pointed to the right slightly, not sure why, it is consistant though, so manually rotating it should fix it for now
			Vector3 rot = transform.GetChild(0).localEulerAngles;
			rot.y -= 8f;
			rot.x -= 2.5f;
			transform.GetChild(0).localEulerAngles = rot;
		}


		//Ray ray = camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
		//int layerMask = 1 << 10;
		//layerMask = ~layerMask;
		//if (Physics.Raycast(ray, out RaycastHit aimPoint, Mathf.Infinity, layerMask)) {
		//	transform.LookAt(aimPoint.point);
		//} else {
		//	transform.localEulerAngles = new Vector3(0, 0, 0);
		//}
	}

    void PlayerControl() {
        fireTime += Time.deltaTime;
        if (cAcc >= minAcc) {
            float handlebuffer = cAcc / minAcc * 5;
            cAcc -= (handling + handlebuffer) * Time.deltaTime;
        }
        if (!GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<PlayerController>().isCruising) {
			if (Input.GetButton("Attack") || Input.GetAxis("Attack") != 0) {
				if (transform.root.root.GetComponent<PlayerHealth>().battery >= 5) {
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
        bullet.transform.Rotate(Vector3.up * (UnityEngine.Random.Range(-cAcc, cAcc) / 10)); // Dividing by 10 so larger accuracy values can be input for balancing sake
        bullet.transform.Rotate(Vector3.left * (UnityEngine.Random.Range(-cAcc, cAcc)) / 10);
        bullet.layer = 9;
        BulletScript bStats = bullet.GetComponent<BulletScript>();
        bStats.speed = pSpeed;
        bStats.damage = damage;
        bStats.grav = pGrav;
        bullet.transform.parent = GameObject.FindGameObjectWithTag("MissileParent").transform;
        animation.Play("Fire", PlayMode.StopAll);
        cAcc += incAcc;
        CameraMotion recoil = FindObjectOfType<CameraMotion>();
        if (recoil != null)
        {
            recoil.Recoil(recoilxy[0], recoilxy[1]);
        }
        FireSound();
    }

    void FireSound()
    {
        int sIndex = UnityEngine.Random.Range(1, 5);
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
