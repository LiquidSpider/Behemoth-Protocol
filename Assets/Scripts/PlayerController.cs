using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb; // Player body
    //  Camera Vars
    public float speedH = 2.0f; // Sensitivity
    public float speedV = 2.0f;
    private float rotX = 0.0f; // Current rotation, don't touch this
    private float rotY = 0.0f;
    
    //  Movement Vars
    public float accSpeed = 200.0f;

    //  Weapon vars
    public GameObject arm; // Object that holds the gun

    void Start() {
        rb = GetComponent<Rigidbody>();
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
        transform.eulerAngles = new Vector3(rotY, rotX, 0.0f);
    }
    void PlayerMove() {
        if (Input.GetButtonDown("Pause")) {
            Cursor.lockState = CursorLockMode.None;
        }
        rb.AddForce(transform.forward * Input.GetAxis("LeftVertical") * accSpeed);
        rb.AddForce(transform.right * Input.GetAxis("LeftHorizontal") * accSpeed);
        rb.AddForce(transform.up * Input.GetAxis("AscDesc") * accSpeed);
    }
    void AimWeapon() {
        RaycastHit aimPoint;
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        if (Physics.Raycast(ray, out aimPoint)) {
            arm.transform.LookAt(aimPoint.point);
        } else {
            arm.transform.localEulerAngles = new Vector3(0, 0, 0);
        }
    }
}
