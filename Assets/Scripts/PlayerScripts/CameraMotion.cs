using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMotion : MonoBehaviour
{
    //  Vars
    [Tooltip("How fast camera moves to player")]
    public float mSpeed = 1f;                                           // How fast camera moves to target

    // Private vars
    private float rotX = 0.0f;                                          // Current rotation, don't touch this
    private float rotY = 0.0f;                                          // Ditto
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
