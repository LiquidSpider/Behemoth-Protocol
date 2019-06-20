using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class giantBehaviour : MonoBehaviour
{
	//Initialisation variables

	public GameObject myself; //Easy reference to own GameObject
	public GameObject[] pathway = new GameObject[6]; //Path the Giant will take
	public float pathLength = 0.0f; //Distance of path
	float moveDistance = 0.0f; //Distance to travel in 1sec
	GameObject currentTarget;
	int targetIndex = 0;

	//Variables containing the body parts and if they are damaged or not
	public GameObject LeftArm;
	bool leftArmSafe = true;
	public GameObject RightArm;
	bool rightArmSafe = true;
	public GameObject Body;
	public GameObject Legs;
	bool legsSafe = true;
	float legDamagedTime = 0;


    // Start is called before the first frame update
    void Start() {
        getPathLength();
        getMoveDistance();
        currentTarget = pathway[targetIndex];

    }

    // Update is called once per frame
    void Update() {
    	//Giant will move along path to target, unless legs are damaged
    	if(legsSafe) {
	        moveToTarget();
	    } else if(rightArmSafe || leftArmSafe) {
	    	fixLegs();
	    } else {
	    	//In here will be the 'last ditch effort' or whatever
	    }
    }

    // getPathLength retreives the total distance to Giant has to move, in order to calculate how much it needs to move each frame
    void getPathLength() {
    	float pathDisX = 0;
    	float pathDisZ = 0;
    	for(int i = 1; i < pathway.Length; i++) {
    		pathDisX = pathway[i].transform.position.x - pathway[i-1].transform.position.x;
    		pathDisZ = pathway[i].transform.position.z - pathway[i-1].transform.position.z;

    		pathLength += Mathf.Sqrt(pathDisX * pathDisX + pathDisZ * pathDisZ);
    	}
    }

    // getMoveDistance finds the distance the Giant should travel each frame
    void getMoveDistance() {
    	// Total travel time should be 12min * 60
    	int travTime = 10 * 60; //actually going for 10 mins
    	moveDistance = pathLength / travTime;
    }

    // moveToTarget moves the Giant along its path to the dam wall
    void moveToTarget() {
    	myself.transform.LookAt(currentTarget.transform);

    	//Change target if giant has reached its current target
    	if(Vector3.Distance(currentTarget.transform.position, myself.transform.position)  <= 0.5f) {
    		targetIndex++;
    		currentTarget = pathway[targetIndex];
    	} else {
    		//Otherwise, head towards next target
    		myself.transform.Translate(Vector3.forward * moveDistance * Time.deltaTime);
    	}
    }

    // leftArmGone adjusts variables when the giant loses its arm
    public void leftArmGone() {
    	leftArmSafe = false;
    	LeftArm.SetActive(false);
    }

    // rightArmGone adjusts variables when the giant loses its arm
    public void rightArmGone() {
    	rightArmSafe = false;
    	RightArm.SetActive(false);
    }

    // legsGame adjusts variables when the giant loses its legs
    public void legsGone() {
    	legsSafe = false;
    	legDamagedTime = 0;
    }

    // fixLegs runs a timer that will return the giant to walking when the timer is up
    void fixLegs() {
    	//Increment Timer
    	legDamagedTime += Time.deltaTime;
    	//If both arms are still operational
    	if(rightArmSafe && leftArmSafe) {
    		//Fix in 15 seconds
	    	if(legDamagedTime > 15.0f) {
    			legsSafe = true;
		 	   	Legs.GetComponent<legsBehaviour>().legsFixed();
    		}
    	//If only 1 arm is operational - fix in 30 secs
    	} else if(rightArmSafe && !leftArmSafe || leftArmSafe && !rightArmSafe) {
    		if(legDamagedTime > 30.0f) {
    			legsSafe = true;
		    	Legs.GetComponent<legsBehaviour>().legsFixed();
    		}
    	//if no arms are operational, don't do anything here
    	} else {
    		return;
    	}
    }
}
