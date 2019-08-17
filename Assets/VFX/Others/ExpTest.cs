using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("4"))
        {
            Exploder.explode(gameObject.transform);
        } else if(Input.GetKeyDown("1"))
        {
            Exploder.damagedSmoke(gameObject, gameObject.transform);
        } else if(Input.GetKeyDown("2"))
        {
            Vector3 rotat = new Vector3(0.0f, 90.0f, 0.0f);
            Exploder.damagedSmokeWRotation(gameObject, rotat);
        } else if(Input.GetKeyDown("3"))
        {
            Vector3 rotat = new Vector3(0.0f, 180.0f, 0.0f);
            Exploder.damagedSmokeWRotation(gameObject, rotat);
        }
    }
}
