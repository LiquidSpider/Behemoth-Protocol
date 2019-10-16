using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandTarget : MonoBehaviour
{

    //public enum Hand
    //{
    //    left,
    //    right
    //}

    //public Hand hand;

    public GameObject pivot;
    public GameObject chest;

    private Vector3 startingPosition;

    // Start is called before the first frame update
    void Start()
    {
        this.startingPosition = this.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        // Rotation the starting position around relative to the gaints rotation;
        this.transform.localPosition = RotatePointAroundPivot(startingPosition, transform.InverseTransformPoint(pivot.transform.position), chest.transform.rotation.eulerAngles);
    }

    public Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles)
    {
        return Quaternion.Euler(angles) * (point - pivot) + pivot;
    }

}
