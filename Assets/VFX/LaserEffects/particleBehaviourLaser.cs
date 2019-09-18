using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class particleBehaviourLaser : MonoBehaviour
{

	public float velocity;
    public float rotateAmount;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(WaitAndDestroy(3.0f));

        velocity = Random.Range(12.0f, 13.0f);
        rotateAmount = Random.Range(4.0f, 8.0f);

        if(rotateAmount >= 0 && rotateAmount < 4) {
            rotateAmount += 4;
        } else if(rotateAmount < 0 && rotateAmount > -4) {
            rotateAmount -= 4;
        }
        
        transform.Rotate(0.0f, Random.Range(10.0f, 20.0f), 0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0.0f, rotateAmount, 0.0f);
        transform.position = new Vector3(transform.position.x + (velocity * Time.deltaTime), transform.position.y, transform.position.z);
    }

    public void setup(float x, float y, float size, float speed) {
    	transform.position = new Vector3(transform.position.x, transform.position.y + y, transform.position.z + x);
    	//velocity = speed;

    	transform.localScale = new Vector3(size, size, size);

		gameObject.transform.parent = GameObject.FindGameObjectWithTag("MissileParent").transform.GetChild(0);
	}

    IEnumerator WaitAndDestroy(float waitTime) {
    	yield return new WaitForSeconds(waitTime);
    	Destroy(gameObject);
    }
}
