using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldRotate : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.Rotate(0.0f, 0.0f, 1.0f);

        if(gameObject.transform.localScale.x < 1) {
        	gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x + 0.05f, gameObject.transform.localScale.y + 0.05f, gameObject.transform.localScale.z + 0.05f);
        }
    }
}
