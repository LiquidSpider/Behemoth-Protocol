using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class particleControllerLaserCharge : MonoBehaviour
{

	public GameObject particleBase;

	public float xMax = -1;
	public float yMax = -1;

	public float zVelMin = -1;
	public float zVelMax = -1;
	public float sizeMax = -1;
	public float sizeMin = -1;

	float xSpawn;
	float ySpawn;
	float zSpawn;
	float xVel;
	float yVel;
	float zVel;
	float size;

	public float emitRate = 5;

	public bool active = false;
	public float timer = 0.0f;

    // Start is called before the first frame update
    void Start() {
    	activate();
        
    }

    // Update is called once per frame
    void Update() {
        if(active) {
        	timer += Time.deltaTime;
        	if(timer >= 1 / emitRate) {
        		timer = 0.0f;
        		findLocation();
        		size = Random.Range(sizeMin, sizeMax);
        		GameObject tempParticle = Instantiate(particleBase, transform.position, transform.rotation);
                //tempParticle.transform.parent = gameObject.transform;
                size = size * 10.0f;
        		tempParticle.GetComponent<particleBehaviourCharge>().setup(xSpawn, ySpawn, size, zVel);
        	}
        }
    }

    public void activate() {
    	active = true;
    }

    public void deactivate() {
    	active = false;
    }

    void findLocation() {
    	bool goodLoc = false;
    	float xRatio;
    	float yRatio;

    	while(!goodLoc) {
	    	xSpawn = Random.Range(-xMax, xMax);
    		ySpawn = Random.Range(-yMax, yMax);

    		zVel = Random.Range(zVelMin, zVelMax);

	    	xRatio = xSpawn/xMax;
	    	yRatio = ySpawn/yMax;

	    	if(xRatio*xRatio + yRatio*yRatio <= 1) {
	    		goodLoc = true;
	    	} else {
	    		goodLoc = false;
	    	}

    	}

    }
}
