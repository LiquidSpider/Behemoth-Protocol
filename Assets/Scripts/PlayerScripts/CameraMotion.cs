using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMotion : MonoBehaviour
{
    //  Vars
    [Tooltip("How fast camera moves to player")]
    public float mSpeed = 0.2f;                                           // How fast camera moves to target

    // Private vars
    private GameObject target;                                           // Camera following target

    void Start()
    {
        Target();
    }

    void LateUpdate()
    {
        FollowTarget();
    }

    void FollowTarget()
    {
        float nSpeed = mSpeed;
        if (target.GetComponent<PlayerController>().isCruising) nSpeed = nSpeed * 1.5f;
        Vector3 smoothPosition = Vector3.Lerp(transform.position, target.transform.position, nSpeed);
        transform.position = smoothPosition;
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
