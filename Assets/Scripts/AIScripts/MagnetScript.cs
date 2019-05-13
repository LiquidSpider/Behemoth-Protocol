using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetScript : MonoBehaviour
{

    // The object magnetised
    public GameObject magnetObject;

    // The forces used to handle the magnetisation
    public float magnetForce;
    private Vector3 direction;

    // this objects rigidbody
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

        // Check if gravity is applied to this object
        if(Rigidbody)
        {
            if(Rigidbody.useGravity)
            {
                Debug.Log("Note " + this.gameObject.name + " uses gravity. It may slowely fall during magnetisation.");
            }

        }

    }

    // fixed update call
    private void FixedUpdate()
    {

        // Call magnet method
        PullObject();

    }

    // Update is called once per frame
    void Update()
    {

        // Update the direction of the pull
        direction = -this.gameObject.transform.up;

#if DEBUG

        // Draw the direction of the magnets pull
        Debug.DrawRay(this.gameObject.transform.position, -this.gameObject.transform.up, Color.red);

#endif

    }

    void PullObject()
    {

        // Create a ray to detect magnet below the object
        Ray ray = new Ray(this.gameObject.transform.position, direction);

        // All the objects hit by the ray
        RaycastHit[] hits = Physics.RaycastAll(ray, float.PositiveInfinity);

        List<RaycastHit> magnetHits = new List<RaycastHit>();

        // Find the rays that hit the object
        for (int i = 0; i < hits.Length; i++)
        {

            if (hits[i].collider.gameObject.name == magnetObject.name)
            {

                // Add object to list
                magnetHits.Add(hits[i]);

            }

        }

        // If there was a magnet hti
        if (magnetHits.Count > 0)
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

            // TODO
            // Rotate these points around the objects position relative to the objects current rotation
            Vector3 position1 = new Vector3(4, 0, 4);
            Vector3 position2 = new Vector3(-4, 0, 4);
            Vector3 position3 = new Vector3(-4, 0, -4);
            Vector3 position4 = new Vector3(4, 0, -4);

            position1 = this.transform.rotation * position1 + this.transform.position;
            position2 = this.transform.rotation * position2 + this.transform.position;
            position3 = this.transform.rotation * position3 + this.transform.position;
            position4 = this.transform.rotation * position4 + this.transform.position;

            Debug.DrawRay(position1, direction, Color.green);
            Debug.DrawRay(position2, direction, Color.red);
            Debug.DrawRay(position3, direction, Color.yellow);
            Debug.DrawRay(position4, direction, Color.blue);

            // Add 4 forces at different positions
            Rigidbody.AddForceAtPosition(direction * (magnetForce) * Time.deltaTime, position1);
            Rigidbody.AddForceAtPosition(direction * (magnetForce) * Time.deltaTime, position2);
            Rigidbody.AddForceAtPosition(direction * (magnetForce) * Time.deltaTime, position3);
            Rigidbody.AddForceAtPosition(direction * (magnetForce) * Time.deltaTime, position4);

            //// Add the force to the body
            //Rigidbody.velocity += direction * magnetForce * Time.deltaTime;

        }

    }

}
