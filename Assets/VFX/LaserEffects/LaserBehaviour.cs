using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBehaviour : MonoBehaviour
{

	public GameObject LaserModel;
	public GameObject myself;

	public float chargeTime = 3.0f;
	public float fireTime = 5.0f;
	public float waitTime = 1.0f;

	public bool charging = false;
	public bool firing = false;
	public bool waiting = true;

	public float timer = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        chargeTime = 3.0f;
        fireTime = 5.0f;
        waitTime = 1.0f;
        LaserModel.SetActive(false);
        //myself.GetComponent<ParticleSystem>().Stop();

    }

    // Update is called once per frame
    void Update()
    {	
    	timer += Time.deltaTime;
        if(waiting && timer > waitTime) {
        	waiting = false;
        	charging = true;
        	//myself.GetComponent<ParticleSystem>().Play();
            myself.GetComponent<particleControllerLaserCharge>().activate();
        	timer = 0.0f;
        }

        if(charging) {
        	//effect things on and off here
        }

        if(charging && timer > chargeTime) {
        	charging = false;
        	firing = true;
        	timer = 0.0f;
        	//effects on and off here
        	//myself.GetComponent<ParticleSystem>().Stop();
            myself.GetComponent<particleControllerLaserCharge>().deactivate();
            LaserModel.SetActive(true);
            LaserModel.GetComponent<particleControllerLaser>().activate();
        }

        if(firing) {
        	//effect things on and off here
        }

        if(firing && timer > fireTime) {
        	firing = false;
        	waiting = true;
        	timer = 0.0f;
        	//effect on and off here
        	LaserModel.SetActive(false);
        }
    }
}
