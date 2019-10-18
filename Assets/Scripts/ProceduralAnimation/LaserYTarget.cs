using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserYTarget : MonoBehaviour
{

    public bool IsLeft;
    public GameObject Hand;
    public GameObject Player;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        //Move the object update and down the forward vector of the hand matching the player.
        Vector3 planeOrigin = Hand.transform.position;
        Vector3 planeNormal;

        if (IsLeft)
            planeNormal = -Hand.transform.right;
        else
            planeNormal = Hand.transform.right;

        Vector3 point = Player.transform.position;

        Vector3 v = point - planeOrigin;
        float d = Vector3.Dot(v, planeNormal);
        Vector3 position = point - (d * planeNormal);
        
        // Clamp the positions > origin
        if (position.z < planeOrigin.z)
        {
            position.z = planeOrigin.z;
        }

        this.transform.position = position;

        if (IsLeft)
        {
            DrawPlane(this.transform.position, planeNormal, Color.green);
            DrawPlane(this.transform.position, Hand.transform.up, Color.red);
            DrawPlane(this.transform.position, Hand.transform.forward, Color.blue);
        }
        else
        {
            DrawPlane(this.transform.position, planeNormal, Color.green);
        }
    }

    void DrawPlane(Vector3 position, Vector3 normal, Color color)
    {

        Vector3 v3;

        if (normal.normalized != Vector3.forward)
            v3 = Vector3.Cross(normal, Vector3.forward).normalized * normal.magnitude;
        else
            v3 = Vector3.Cross(normal, Vector3.up).normalized * normal.magnitude; ;

        var corner0 = position + v3;
        var corner2 = position - v3;
        var q = Quaternion.AngleAxis(90.0f, normal);
        v3 = q * v3;
        var corner1 = position + v3;
        var corner3 = position - v3;

        Debug.DrawLine(corner0, corner2, color);
        Debug.DrawLine(corner1, corner3, color);
        Debug.DrawLine(corner0, corner1, color);
        Debug.DrawLine(corner1, corner2, color);
        Debug.DrawLine(corner2, corner3, color);
        Debug.DrawLine(corner3, corner0, color);
        Debug.DrawRay(position, normal, Color.red);
    }
}
