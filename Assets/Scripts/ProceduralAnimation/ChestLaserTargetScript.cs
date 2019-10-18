using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestLaserTargetScript : MonoBehaviour
{

    public GameObject ChestLaserPivot;
    public GameObject Player;

    public float maxAngle;
    private float maxRadians;
    public float minAngle;
    private float minRadians;

    public float speed;

    private float maxHeight;
    private float minHeight;

    // Start is called before the first frame update
    void Start()
    {
        if(speed == 0)
            speed = 1000;
    }

    // Update is called once per frame
    void Update()
    {
        maxRadians = maxAngle * Mathf.PI / 180;
        minRadians = minAngle * Mathf.PI / 180;

        CalculateMaxAndMinHeight();
        FollowPlayer();
    }

    private void CalculateMaxAndMinHeight()
    {    
        Vector3 pivotPosition = ChestLaserPivot.transform.position;

        float currentHeight = this.transform.position.y;
        float pivotCurrentHeight = pivotPosition.y; 

        Vector3 currentPosition = new Vector3(this.transform.position.x, 0, this.transform.position.z);
        Vector3 currentPivotPosition = new Vector3(ChestLaserPivot.transform.position.x, 0, ChestLaserPivot.transform.position.z);
        float distanceFromPivot = Vector3.Distance(currentPosition, currentPivotPosition);

        // Calculate the max and min y heights
        maxHeight = pivotCurrentHeight + (Mathf.Tan(maxRadians) * distanceFromPivot);
        minHeight = pivotCurrentHeight - (Mathf.Tan(minRadians) * distanceFromPivot);

        //Debug.Log(string.Format("distance: {0} currentHeight: {1} maxHeight: {2} minHeight: {3}", distanceFromPivot, currentHeight, maxHeight, minHeight));
    }

    private void FollowPlayer()
    {
        Vector3 position = Player.transform.position;
        //position.y += 83.72638f;
        Vector3 newPosition = Vector3.MoveTowards(this.transform.position, Player.transform.position, Time.deltaTime * speed);
        newPosition.y = Mathf.Clamp(newPosition.y, minHeight, maxHeight);
        this.transform.position = newPosition;
    }
}
