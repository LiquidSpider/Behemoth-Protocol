using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetScript : MonoBehaviour
{

    // The object magnetised
    public GameObject magnetObject;

    // The forces used to handle the magnetisation
    public float magnetForce;
    public float maxMagnetSpeed;

    // this objects rigib body
    private Rigidbody Rigidbody;

    // Start is called before the first frame update
    void Start()
    {

        // Check if a magnet object exists
        if (!magnetObject)
        {
            throw new System.Exception("Please assign an object to " + this.gameObject.name + " - Magnet Script");
        }

        // Check if rigidbody exists
        if(!this.gameObject.GetComponent<Rigidbody>())
        {
            throw new System.Exception(this.gameObject.name + " has no rigidbody attached. Please attach rigidbody.");
        }
        else
        {
            this.Rigidbody = this.gameObject.GetComponent<Rigidbody>();
        }

    }

    // fixed update call
    private void FixedUpdate()
    {

        // call magnet method
        PullObject();

    }

    // Update is called once per frame
    void Update()
    {

#if DEBUG

        // Draw the direction of the magnets pull
        Debug.DrawRay(this.gameObject.transform.position, -this.gameObject.transform.up, Color.red);

#endif

    }

    // Handles the pull of the object
    void PullObject()
    {

        // First Check if the object is close enough to reduce magnet
        Ray ray = new Ray(this.gameObject.transform.position, -this.gameObject.transform.up);

        // All the objects hit by the ray
        RaycastHit[] hits = Physics.RaycastAll(ray, float.PositiveInfinity);

        List<RaycastHit> magnetHits = new List<RaycastHit>();
        
        // Find the rays that hit the object
        for(int i = 0; i < hits.Length; i++)
        {

            if (hits[i].collider.gameObject.name == magnetObject.name)
            {

                // Add object to list
                magnetHits.Add(hits[i]);

            }

        }

        if(magnetHits.Count > 0)
        {

            RaycastHit magnet = new RaycastHit();

            // get the closest one
            for (int i = 0; i < magnetHits.Count; i++)
            {

                if (i == 0)
                    magnet = magnetHits[i];
                else
                {
                    if (magnet.distance > magnetHits[i].distance)
                        magnet = magnetHits[i];
                }

            }

            // Stop adding a massive force if the object it close enough
            if(magnet.distance > 0.5f)
            {

                // Check if the object is already being pulled towards at enough magnitude
                if (Vector3.Dot(Rigidbody.velocity, -this.gameObject.transform.up) < maxMagnetSpeed)
                {

                    // Add the force to the object
                    Rigidbody.AddForce(-this.gameObject.transform.up * magnetForce);

                }

            }
            else
            {

                // Check if the object is already being pulled towards at enough magnitude
                if (Vector3.Dot(Rigidbody.velocity, -this.gameObject.transform.up) < 5)
                {

                    // Add the force to the object
                    Rigidbody.AddForce(-this.gameObject.transform.up * 100);

                }

            }

        }

    }

}
