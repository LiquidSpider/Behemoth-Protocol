using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMotion : MonoBehaviour
{
    //  Vars
    public float minSpeed = 0.2f;                                           // How fast camera moves to target
    public float maxSpeed = 1;                                              // The tightest the camera can get
    

    // Private vars
    private GameObject target;                                              // Camera following target
    private float tSpeed;                                                   // How tight the camera wants to be
    private float aSpeed;                                                   // How tight the camera actually is
    private Vector3 tRotation;                                              // Target rotation
    private Vector3 rVelocity = Vector3.zero;                               // Recoil velocity

    void Start()
    {
        Target();
    }

    void LateUpdate()
    {
        FollowTarget();
        ManageRecoil();
    }

    void FollowTarget()
    {
        tSpeed = minSpeed + (target.GetComponent<Rigidbody>().velocity.magnitude * 0.0008f);
        tSpeed = Mathf.Clamp(tSpeed, minSpeed, maxSpeed);
        Vector3 smoothPosition = Vector3.Lerp(transform.position, target.transform.position, tSpeed);
        transform.position = smoothPosition;
        
    }
    void Target()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<PlayerController>().gameObject;

    }
    void Target(GameManager nTarget)
    {
        target = nTarget.transform.gameObject;
    }
    void ManageRecoil()
    {
        tRotation = target.transform.rotation.eulerAngles;
        // Recoil part

        tRotation = Vector3.SmoothDamp(tRotation, target.transform.rotation.eulerAngles, ref rVelocity, 0.5f);
        transform.rotation = Quaternion.Euler(tRotation);
    }
    public void Recoil(Vector3 direction)
    {
        //Debug.Log("Recoil called: X" + direction.x + " Y" + direction.y);
        tRotation = direction;
    }
}
