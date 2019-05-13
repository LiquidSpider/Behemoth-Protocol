using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //  Components
    private Rigidbody rb;                                               // Player body
    public GameObject body;                                             // Player gameobject body
    private Animator animator;                                          // Body animator
    private TrailRenderer trail;                                        // Trail renderer in player body
    //  Camera Vars
    private GameObject cameraObj;                                       // The camera object
    private Camera camera;                                              // The camera component
    public float speedH = 2.0f;                                         // Sensitivity horizontal
    public float speedV = 2.0f;                                         // Sensitivity vertical
    public float maxY = 89.9f;                                          // Clamps player's vertical angle betwwen this
    public float minY = -89.9f;                                         // And this
    public float cruiseLMod = 0.5f;                                     // Multiplier for look speed when cruising
    public bool isZoom = false;                                         // Is the player zooming
    [SerializeField]
    private Vector3 cZoom;                                              // Where the camera is when zoomed
    public Vector3 cUnZoom = new Vector3(-1, 1, -3);                    // Where it is when it's not
    private Vector3 target;                                             // Where the camera wants to be
    private float rotX = 0.0f;                                          // Current rotation, don't touch this
    private float rotY = 0.0f;                                          // Ditto
    private float uFOV;                                                 // Unzoomed FOV
    public float zFOV = 60;                                             // Zoomed FOV
    private float tFOV;                                                 // Target FOV
    public float zSpeed = 1f;                                           // How long it takes to zoom
    private float zCLerp = 0f;                                          // Current lerp time
    public float zCLerpSpeed = 1f;                                      // Lerp speed, lower = better, should never be 0
    
    //  Movement Vars
    public float accSpeed = 200.0f;                                     // How much force is applied each tick
    public float dTime = 1f;                                            // How long the player is "dodging" for
    private bool isDodge = false;                                       // Is the player dodging
    public float dForce = 10000.0f;                                     // How much force is applied on dodge
    private bool isCruising = false;                                    // Is the player afterburning
    public float cruiseMod = 0.10f;                                     // Multiplier for all movement except forward when cruising
    public float cruiseFwd = 5f;                                        // Multiplier for forward movement when cruising
    private float cHoldTime = 0f;                                       // How long player has held dodge key for
    public float cHoldThreshold = 0.5f;                                 // How long the player has to hold dodge to cruise

    //  Weapon vars
    public GameObject arm;                                              // Object that holds the gun
    public List<GameObject> weapons = new List<GameObject>();           // Current inventory
    public int cWweap = 0;                                              // Current weapon selected

    void Start() { // Mainly component getting
        cameraObj = GameObject.FindGameObjectWithTag("MainCamera");
        camera = cameraObj.GetComponent<Camera>();
        cZoom = cameraObj.transform.localPosition;
        uFOV = cameraObj.GetComponent<Camera>().fieldOfView;
        rb = GetComponent<Rigidbody>();
        trail = body.GetComponent<TrailRenderer>();
        animator = body.GetComponent<Animator>();
        Cursor.lockState = CursorLockMode.Locked;
    }


    void Update() {
        CameraMove();
        PlayerControls();
        AimWeapon();
        //AvoidObstruction();
    }
    void CameraMove() { // Camera controls and manipulation goes here
        if (!isCruising) {
            rotX += speedH * Input.GetAxis("RightHorizontal");
            rotY -= speedV * Input.GetAxis("RightVertical");
            rotX += speedH * Input.GetAxis("Mouse X");
            rotY -= speedV * Input.GetAxis("Mouse Y");
            if (!isDodge) {
                trail.time = 0.5f;
                trail.startWidth = 0.5f;
            }
        } else {
            rotX += speedH * Input.GetAxis("RightHorizontal") * cruiseLMod;
            rotY -= speedV * Input.GetAxis("RightVertical") * cruiseLMod;
            rotX += speedH * Input.GetAxis("Mouse X") * cruiseLMod;
            rotY -= speedV * Input.GetAxis("Mouse Y") * cruiseLMod;
            if (!isDodge) {
                trail.time = 2;
                trail.startWidth = 1f;
            }

        }
        rotY = Mathf.Clamp(rotY, minY, maxY);
        transform.eulerAngles = new Vector3(rotY, rotX, 0.0f);
        zCLerp += Time.deltaTime;
        if (zCLerp > zCLerpSpeed) {
            zCLerp = zCLerpSpeed;
        }
        float t = zCLerp / zCLerpSpeed;
        t = Mathf.Sin(t * Mathf.PI * 0.5f);
        camera.transform.localPosition = Vector3.Lerp(camera.transform.localPosition, target, t);
        camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, tFOV, t);
    }

    void PlayerControls() { // Player controls and manipulation goes here
        if (Input.GetButtonDown("Pause")) { // Release key
            Cursor.lockState = CursorLockMode.None;
        }
        if (Input.GetButton("Zoom")) {
            zCLerp = 0f;
            target = cUnZoom;
            tFOV = zFOV;
            isZoom = true;
        } else {
            zCLerp = 0f;
            tFOV = uFOV;
            target = cZoom;
            isZoom = false;
        }
        if (!isDodge) {
            float mods = 1f;
            if (isCruising) {
                mods = cruiseMod;
                rb.AddForce(transform.forward * accSpeed * cruiseFwd);
            } else {
                mods = 1f;
                rb.AddForce(transform.forward * Input.GetAxis("LeftVertical") * accSpeed * mods);
            }
            rb.AddForce(transform.right * Input.GetAxis("LeftHorizontal") * accSpeed * mods);
            rb.AddForce(transform.up * Input.GetAxis("AscDesc") * accSpeed * mods);
        }
        // Inputs
        //if (Input.GetButtonDown("Dodge") && !isDodge) {
        //    StartCoroutine(Dodge());
        //}
        if (Input.GetButtonDown("Dodge")) cHoldTime = 0f;
        if (Input.GetButton("Dodge")) {
            if (cHoldTime > cHoldThreshold) {
                isCruising = true;
            } else cHoldTime += Time.deltaTime;
        }
        if (Input.GetButtonUp("Dodge") && cHoldTime < cHoldThreshold) {
            StartCoroutine(Dodge());
        } else if (Input.GetButtonUp("Dodge") && isCruising) isCruising = false;
        //if (Input.GetAxis("Cruise") > 0 && !isDodge) {
        //    trail.time = 2;
        //    trail.startWidth = 1f;
        //    isCruising = true;
        //} else {
        //    trail.time = 0.5f;
        //    trail.startWidth = 0.5f;
        //    isCruising = false;
        //}
        if (Input.GetButtonDown("Weapon1")) {
            SwapWeapon(0);
        }
        if (Input.GetButtonDown("Weapon2")) {
            SwapWeapon(1);
        }
        if (Input.GetButtonDown("Weapon3")) {
            SwapWeapon(2);
        }
        if (Input.GetButtonDown("Weapon4")) {
            SwapWeapon(3);
        }
    }

    void AimWeapon() {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        if (Physics.Raycast(ray, out RaycastHit aimPoint)) {
            arm.transform.LookAt(aimPoint.point);
        } else {
            arm.transform.localEulerAngles = new Vector3(0, 0, 0);
        }
    }

    void SwapWeapon(int wIndex) {
        if (wIndex < weapons.Count) {
            foreach (GameObject cWeapon in GameObject.FindGameObjectsWithTag("CurrentWeapon")) {
                Destroy(cWeapon);
            }
            GameObject nWeapon = Instantiate(weapons[wIndex], arm.transform, false);
            nWeapon.transform.localPosition = Vector3.zero;
            nWeapon.transform.localRotation = Quaternion.identity;
        }
    }

    //void AvoidObstruction() {
    //    if (Physics.Linecast(camera.transform.position, transform.position, out RaycastHit obstruction)) {

    //    }
    //}

    IEnumerator Dodge() {
        if (!isDodge) {
            isDodge = true;
            if (Input.GetAxis("LeftHorizontal") > 0) {
                animator.SetTrigger("DodgeRight");
            }
            if (Input.GetAxis("LeftHorizontal") < 0) {
                animator.SetTrigger("DodgeLeft");
            }
            rb.AddForce(transform.forward * Input.GetAxis("LeftVertical") * dForce * 100);
            rb.AddForce(transform.right * Input.GetAxis("LeftHorizontal") * dForce * 100);
            rb.AddForce(transform.up * Input.GetAxis("AscDesc") * dForce * 100);
            rb.AddTorque(transform.right * Input.GetAxis("LeftHorizontal") * dForce);
            yield return new WaitForSeconds(dTime);
            isDodge = false;
        }
    }
}
