using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class legsBehaviour : MonoBehaviour
{

	public GameObject giantBehave;
	public float health = 6000;

	private List<GameObject> damageTakenFrom = new List<GameObject>();

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
    	//giantBehave.GetComponent<giantBehaviour>().legsGone();
    }

    // legsFixed will reset the leg health, allowing the giant to keep moving
    public void legsFixed() {
    	health = 4000;
    }

    private void OnCollisionEnter(Collision other) {
		if (other.gameObject.transform.tag == "Explosion - Player") {

			TakeDamage(200, other.gameObject);
		}

		if (other.gameObject.transform.tag == "Bullet - Player") {

			TakeDamage(10, other.gameObject);
		}

	}


	public void TakeDamage(float damage, GameObject explosion) {
		if (!damageTakenFrom.Contains(explosion)) {
			damageTakenFrom.Add(explosion);
			health -= damage;

			if (health <= 0) {
				damaged();
			}
		}
	}
	
}
