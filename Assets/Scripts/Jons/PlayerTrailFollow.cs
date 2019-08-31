using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTrailFollow : MonoBehaviour
{

	public GameObject playerForAnimator;
	private Animator playerAnimator;
	private ParticleSystem ps;

    // Start is called before the first frame update
    void Awake() {
        playerAnimator = playerForAnimator.GetComponent<Animator>();
        ps = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update() {

    	if (playerAnimator == null) {
    		playerAnimator = playerForAnimator.GetComponent<Animator>();
    	}

    	if (ps == null) {
    		ps = GetComponent<ParticleSystem>();
    	}

    	if (!ps.isPlaying) {
    		ps.Play();
    	}

        //Change the x/y/z of this objects transform based on what animation the player is in
        if (playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle")) {
        	gameObject.transform.localPosition = new Vector3(-0.0f, 1.392f, -0.162f);
        } else if (playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("boost")) {
        	gameObject.transform.localPosition = new Vector3(0.0f, 1.051f, 0.358f);
        } else if (playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Shield")) {
        	gameObject.transform.localPosition = new Vector3(0.0f, 1.357f, -0.265f);
        } else if (Input.GetKey("w")) { //Change this A LOT
        	gameObject.transform.localPosition = new Vector3(-0.0f, 1.445f, 0.068f);
        }
    }
}
