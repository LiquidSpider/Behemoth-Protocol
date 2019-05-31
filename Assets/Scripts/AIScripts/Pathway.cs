using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathway : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDrawGizmosSelected()
    {
        Color colour = Color.red;
        Gizmos.color = colour;
        //Gizmos.DrawLine(this.transform.position, (this.transform.up * 10.0f).normalized + this.transform.position);
        Gizmos.DrawRay(this.transform.position, this.transform.up * 10.0f);

        //RaycastHit hit;
        //Physics.Raycast(transform.position, -transform.up, out hit, 1000.0f);
        //Debug.Log(hit.distance);

    }
}
