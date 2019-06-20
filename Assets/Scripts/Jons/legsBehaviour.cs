using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class legsBehaviour : MonoBehaviour
{

	public GameObject giantBehave;
	public float health = 6000;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("l")) {
        	damaged();
        }
    }

    // damaged reports to the overall AI that the legs are gone
    void damaged() {
    	giantBehave.GetComponent<giantBehaviour>().legsGone();
    }

    // legsFixed will reset the leg health, allowing the giant to keep moving
    public void legsFixed() {
    	health = 4000;
    }
}
