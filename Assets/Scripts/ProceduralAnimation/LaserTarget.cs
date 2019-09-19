using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTarget : MonoBehaviour
{

    public GameObject parent;
    public GameObject player;
    public float maxYRotation;
    public float minYRotation;

    public enum HandTarget
    {
        left,
        right
    }

    public HandTarget handTarget;

    private float angle;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        RotateParent();

        // match the players Y to this Y. and clamp the y
        float yPosition = player.transform.position.y + 20.0f;
        if(yPosition < 615.0f)
        {
            yPosition = 615.0f;
        }
        this.transform.position = new Vector3(this.transform.position.x, yPosition, this.transform.position.z);

        LookAtPlayer();

    }

    private void RotateParent()
    {
        // Get the angle between the parent object and the player on the Y axis.
        Vector3 playerPosition = new Vector3(player.transform.position.x, 0, player.transform.position.z);
        Vector3 parentPosition = new Vector3(parent.transform.position.x, 0, parent.transform.position.z);
        Vector3 Direction = playerPosition - parentPosition;
        Quaternion rotation = Quaternion.LookRotation(Direction);

        // convert the angle to -180 - 180 notation.
        angle = rotation.eulerAngles.y % 360;
        angle = angle > 180 ? angle - 360 : angle;

        // Clamp the angle between the max and min
        angle = Mathf.Clamp(angle, minYRotation, maxYRotation);

        // set the angle
        parent.transform.eulerAngles = new Vector3(0, angle, 0);
    }

    private void LookAtPlayer()
    {

        Vector3 playerPosition = new Vector3(player.transform.position.x, 0, player.transform.position.z);
        Vector3 thisPosition = new Vector3(this.transform.position.x, 0, this.transform.position.z);
        Vector3 Direction = playerPosition - thisPosition;
        Quaternion rotation = Quaternion.LookRotation(Direction);

        float angle2 = rotation.eulerAngles.y % 360;
        angle2 = angle2 > 180 ? angle2 - 360 : angle2;

        // Clamp the rotation if we're close to the max or min of the parent rotation
        if (angle < minYRotation + 20)
        {
            switch (handTarget)
            {
                case HandTarget.left:
                    angle2 = Mathf.Clamp(angle2, -130, -50);
                    break;
                case HandTarget.right:
                    angle2 = Mathf.Clamp(angle2, -90, -10);
                    break;
            }
        }
        else if (angle > maxYRotation - 20)
        {
            angle2 = Mathf.Clamp(angle2, 0, 80);
        }

        this.transform.eulerAngles = new Vector3(0, angle2, 0);

    }

}
