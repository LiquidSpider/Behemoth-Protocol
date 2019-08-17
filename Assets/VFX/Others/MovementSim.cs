using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementSim : MonoBehaviour
{

	public Vector3 moveDirection = new Vector3(0.0f, 0.0f, 0.0f);
	public Vector3 lastLoc = new Vector3(0.0f, 0.0f, 0.0f);

    // Start is called before the first frame update
    void Start()
    {
    	moveDirection = gameObject.transform.parent.parent.position;
    	lastLoc = gameObject.transform.parent.parent.position;
    }

    // Update is called once per frame
    void Update()
    {
    	lastLoc.x = moveDirection.x;
    	lastLoc.y = moveDirection.y;
    	lastLoc.z = moveDirection.z;
    	moveDirection = gameObject.transform.parent.parent.position;

        var emitter = gameObject.transform.GetComponent<ParticleSystem>().emission;
        emitter.rateOverTime = 1 + 50 * (Mathf.Abs((moveDirection.x - lastLoc.x)/3) + Mathf.Abs((moveDirection.y - lastLoc.y)/3) + Mathf.Abs((moveDirection.z - lastLoc.z)/3));
    }
}
