using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LooseMovement : MonoBehaviour
{
    public GameObject target;
    private Rigidbody rb;
    //public float offset = 3f;
    //public float maxspeed = 200f;
    public float maxDist = 4f;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        MoveTowards(target);
    }

    void MoveTowards(GameObject targetpos) {
        transform.rotation = targetpos.transform.rotation;
        //if (Vector3.Distance(transform.position, targetpos.transform.position) > maxDist) {
        //    rb.velocity = targetpos.GetComponent<Rigidbody>().velocity;
        //}
        transform.position = Vector3.ClampMagnitude(targetpos.transform.position, maxDist);
    }
}
