using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;               // Player body
    public GameObject body;             // Player gameobject body
    private Animator animator;          // Body animator
    //  Camera Vars
    public float speedH = 2.0f;         // Sensitivity horizontal
    public float speedV = 2.0f;         // Sensitivity vertical
    public float maxY = 89.9f;          // Clamps player's vertical angle betwwen this
    public float minY = -89.9f;         // And this

    private float rotX = 0.0f;          // Current rotation, don't touch this
    private float rotY = 0.0f;          // Ditto
    
    //  Movement Vars
    public float accSpeed = 200.0f;     // How much force is applied each tick
    public float dTime = 1f;            // How long the player is "dodging" for
    private bool isDodge = false;       // Is the player dodging
    public float dForce = 10000.0f;      // How much force is applied on dodge

    //  Weapon vars
    public GameObject arm;              // Object that holds the gun

    void Start() {
        rb = GetComponent<Rigidbody>();
        animator = body.GetComponent<Animator>();
        Cursor.lockState = CursorLockMode.Locked;
    }


    void Update() {
        CameraMove();
        PlayerMove();
        AimWeapon();
    }
    void CameraMove() { 
        rotX += speedH * Input.GetAxis("RightHorizontal");
        rotY -= speedV * Input.GetAxis("RightVertical");
        rotX += speedH * Input.GetAxis("Mouse X");
        rotY -= speedV * Input.GetAxis("Mouse Y");
        rotY = Mathf.Clamp(rotY, minY, maxY);
        transform.eulerAngles = new Vector3(rotY, rotX, 0.0f);
    }
    void PlayerMove() {
        if (Input.GetButtonDown("Pause")) {
            Cursor.lockState = CursorLockMode.None;
        }
        if (!isDodge) {
            rb.AddForce(transform.forward * Input.GetAxis("LeftVertical") * accSpeed);
            rb.AddForce(transform.right * Input.GetAxis("LeftHorizontal") * accSpeed);
            rb.AddForce(transform.up * Input.GetAxis("AscDesc") * accSpeed);
        }
        if (Input.GetButtonDown("Dodge") && !isDodge) {
            StartCoroutine(Dodge());
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
    IEnumerator Dodge() {
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
