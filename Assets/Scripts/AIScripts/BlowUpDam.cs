using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlowUpDam : MonoBehaviour
{

    public GameObject robot;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionExit(Collision collision)
    {

        if(collision.transform.name == "Root_M")
            robot.GetComponent<giantBehaviour>().BlowUpDam();
    }
}
