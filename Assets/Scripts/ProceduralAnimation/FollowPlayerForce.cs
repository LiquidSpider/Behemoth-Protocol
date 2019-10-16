using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayerForce : MonoBehaviour
{

    public GameObject player;
    public float speed;
    public float SlowDistance;

    private bool startFollowing = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if(!startFollowing && Time.time >= 2)
            startFollowing = true;

        if(startFollowing)
        {
            if(Vector3.Distance(this.transform.position, player.transform.position) < SlowDistance)
            {                
                this.GetComponent<Rigidbody>().AddForce((player.transform.position - this.transform.position) * speed * Time.deltaTime);
            }
            else
            {
               this.GetComponent<Rigidbody>().AddForce((player.transform.position - this.transform.position) * speed/100 * Time.deltaTime);
            }
        }
    }
}
