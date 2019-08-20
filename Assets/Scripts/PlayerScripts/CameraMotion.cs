using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMotion : MonoBehaviour
{
    //  Vars
    public float minSpeed = 0.2f;                                           // How fast camera moves to target
    public float maxSpeed = 1;                                              // The tightest the camera can get
    public float recenter = 0.3f;                                           // Lerp factor for readjusting from recoil
    public float recoilSpeed = 0.3f;                                        // How fast the camera kicks up
    

    // Private vars
    private GameObject target;                                              // Camera following target
    private float tSpeed;                                                   // How tight the camera wants to be
    private float aSpeed;                                                   // How tight the camera actually is
    private Vector3 tRotation;                                              // Target rotation
    private Vector3 rVelocity = Vector3.zero;                               // Recoil velocity
    private float fVel = 0f;

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
    void Target(GameObject nTarget)
    {
        target = nTarget.transform.gameObject;
    }
    void ManageRecoil()
    {
        tRotation = target.transform.rotation.eulerAngles;
        // Recoil part

        tRotation = Vector3.SmoothDamp(tRotation, target.transform.rotation.eulerAngles, ref rVelocity, recenter);
        transform.rotation = Quaternion.Euler(tRotation);
    }
    public void Recoil(float powerx, float powery)
    {

        Vector3 nTarget = transform.eulerAngles;
        nTarget.x -= powery;
        nTarget.y += Random.Range(-powerx, powerx);
        Vector3 vector = Vector3.SmoothDamp(transform.eulerAngles, nTarget, ref rVelocity, recoilSpeed);
        transform.rotation = Quaternion.Euler(vector);
    }
}
