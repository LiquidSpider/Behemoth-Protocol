using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rightArmBehaviour : MonoBehaviour
{

	public GameObject giantBehave;
	public float health = 7000;

	private List<GameObject> damageTakenFrom = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("i")) {
        	damaged();
        }
    }

    // damaged reports to the overall AI that the arm is gone
    void damaged() {
    	//giantBehave.GetComponent<giantBehaviour>().rightArmGone();
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
