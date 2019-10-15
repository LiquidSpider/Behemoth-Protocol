using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class particleControllerLaserCharge : MonoBehaviour
{

	public GameObject particleBase;

	private float xMax = -10;
	private float yMax = -10;

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

	public float emitRate = 25;

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
				size = size * 7.5f;
				tempParticle.GetComponent<particleBehaviourCharge>().setup(xSpawn, ySpawn, size, zVel);
				//tempParticle.transform.parent = gameObject.transform;
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

	    	if(xRatio*xRatio + yRatio*yRatio <= 10) {
	    		goodLoc = true;
	    	} else {
	    		goodLoc = false;
	    	}

    	}

    }
}
