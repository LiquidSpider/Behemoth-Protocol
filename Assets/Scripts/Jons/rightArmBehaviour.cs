using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rightArmBehaviour : MonoBehaviour
{

	public GameObject giantBehave;
	public float health = 7000;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("p")) {
        	damaged();
        }
    }

    // damaged reports to the overall AI that the arm is gone
    void damaged() {
    	giantBehave.GetComponent<giantBehaviour>().rightArmGone();
    }
}
