using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMotion : MonoBehaviour
{
    //  Vars
    //[Tooltip("Horizontal sensitivity")]
    //public float speedH = 2.0f;                                         // Sensitivity horizontal
    //[Tooltip("Vertical sensitivity")]
    //public float speedV = 2.0f;                                         // Sensitivity vertical
    //[Tooltip("How far you can look down")]
    //public float maxY = 89.9f;                                          // Clamps player's vertical angle betwwen this
    //[Tooltip("How high you can look up")]
    //public float minY = -89.9f;                                         // And this
    [Tooltip("How fast camera moves to player")]
    public float mSpeed = 1f;                                           // How fast camera moves to target
    //[Tooltip("How fast the camera auto rotates to player")]
    //public float rSpeed = 1f;                                           // How fast camera rotates to target
    [Tooltip("How long before the camera resumes automation after camera is manually moved")]
    //public float mLookTime = 3f;                                        // Seconds before auto camera rotation kicks back in

    // Private vars
    private float rotX = 0.0f;                                          // Current rotation, don't touch this
    private float rotY = 0.0f;                                          // Ditto
    //public bool mLook = false;
    //public float mLookTimer = 0f;
    private GameObject target;                                           // Camera following target

    void Start()
    {
        Target();
    }

    void Update()
    {
        FollowTarget();
    }

    void FollowTarget()
    {
        float step = mSpeed * Vector3.Distance(transform.position, target.transform.position) * Time.deltaTime; // calculate distance to move
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, step);
        transform.rotation = target.transform.rotation;
    }
    void Target()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<PlayerController>().gameObject;

    }
    void Target(GameManager nTarget)
    {
        target = nTarget.transform.gameObject;
    }
}
