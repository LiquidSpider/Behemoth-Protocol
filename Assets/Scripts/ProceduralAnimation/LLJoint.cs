using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Represents a joint.
/// </summary>
[Serializable]
public class LLJoint
{
    /// <summary>
    /// The object for this joint.
    /// </summary> 
    public GameObject jointObject;
    /// <summary>
    /// The starting position of the object.
    /// </summary>
    [NonSerialized]
    public Vector3 StartingPosition;
    /// <summary>
    /// The starting rotation of the object.
    /// </summary>
    [NonSerialized]
    public Vector3 StartingRotation;
    /// <summary>
    /// The local starting rotation of the object.
    /// </summary>
    public Vector3 startingLocalRotation;
    /// <summary>
    /// The local starting position of the object.
    /// </summary>
    public Vector3 startingLocalPosition;

    /// <summary>
    /// The joint objects Local Euler.
    /// </summary>
    public Vector3 CurrentLocationEuler
    {
        get
        {
            return this.jointObject.transform.localEulerAngles;
        }
        set
        {
            this.jointObject.transform.localEulerAngles = value;
        }
    }

    /// <summary>
    /// The joint objects locat rotation.
    /// </summary>
    public Quaternion CurrentLocationRotation
    {
        get
        {
            return this.jointObject.transform.localRotation;
        }
        set
        {
            this.jointObject.transform.localRotation = value;
        }
    }

    /// <summary>
    /// The joint objects local position.
    /// </summary>
    public Vector3 CurrentLocationPosition
    {
        get
        {
            return this.jointObject.transform.localPosition;
        }
        set
        {
            this.jointObject.transform.localPosition = value;
        }
    }

    /// <summary>
    /// The joint objects local position
    /// </summary>
    public Vector3 CurrentPosition
    {
        get
        {
            return this.jointObject.transform.position;
        }
        set
        {
            this.jointObject.transform.position = value;
        }
    }

    /// <summary>
    /// The joint object local rotation
    /// </summary>
    public Quaternion CurrentRotation
    {
        get
        {
            return this.jointObject.transform.rotation;
        }
        set
        {
            this.jointObject.transform.rotation = value;
        }
    }

    /// <summary>
    /// The parent joint.
    /// </summary>
    public ParentJoint parentJoint;

    /// <summary>
    /// Initialise a new joint.
    /// </summary>
    /// <param name="gameObject"></param>
    public LLJoint(GameObject gameObject)
    {
        this.jointObject = gameObject;
        this.StartingPosition = gameObject.transform.position;
        this.StartingRotation = gameObject.transform.eulerAngles;
        this.startingLocalPosition = gameObject.transform.localPosition;
        this.startingLocalRotation = gameObject.transform.localEulerAngles;
        parentJoint = null;
    }

    /// <summary>
    /// Initialise a new joint.
    /// </summary>
    /// <param name="gameObject"></param>
    /// <param name="parentJoint"></param>
    public LLJoint(GameObject gameObject, ParentJoint parentJoint)
    {
        this.jointObject = gameObject;
        this.StartingPosition = gameObject.transform.position;
        this.StartingRotation = gameObject.transform.eulerAngles;
        this.startingLocalPosition = gameObject.transform.localPosition;
        this.startingLocalRotation = gameObject.transform.localEulerAngles;
        this.parentJoint = parentJoint;

        // set the parent joint maxdistance
        if (parentJoint.GetDefault)
        {
            parentJoint.maxDistance = Vector3.Distance(parentJoint.Joint.transform.position, gameObject.transform.position);
        }
    }
}