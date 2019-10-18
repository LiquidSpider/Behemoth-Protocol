using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserHeightCheck : MonoBehaviour
{

    public GameObject laser;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 laserPosition = laser.transform.position;

        Debug.Log(Mathf.Abs(this.transform.position.y - laserPosition.y));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
