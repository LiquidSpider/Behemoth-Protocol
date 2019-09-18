using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class particleBehaviourCharge : MonoBehaviour
{

	public float velocity = -1;
	float distToMove = -1;

    // Start is called before the first frame update
    void Start()
    {   
        //Auto remove particles after a time
        StartCoroutine(WaitAndDestroy(3.0f));
    }

    // Update is called once per frame
    void Update()
    {
        //Move the particle forward -- probably a better way to do this
        transform.position = new Vector3(transform.position.x + (velocity * Time.deltaTime), transform.position.y, transform.position.z);
    }

    public void setup(float x, float y, float size, float speed) {
        //place object where it needs to be, give it speed and size it
    	transform.position = new Vector3(transform.position.x, transform.position.y + y, transform.position.z + x);
    	velocity = speed;

    	transform.localScale = new Vector3(size, size, size);

		gameObject.transform.parent = GameObject.FindGameObjectWithTag("MissileParent").transform.GetChild(0);
    }

    IEnumerator WaitAndDestroy(float waitTime) {
    	yield return new WaitForSeconds(waitTime);
    	Destroy(gameObject);
    }
}
