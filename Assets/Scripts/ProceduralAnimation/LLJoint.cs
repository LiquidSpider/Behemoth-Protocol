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
    public GameObject gameObject;
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
            return this.gameObject.transform.localEulerAngles;
        }
        set
        {
            this.gameObject.transform.localEulerAngles = value;
        }
    }

    /// <summary>
    /// The joint objects locat rotation.
    /// </summary>
    public Quaternion CurrentLocationRotation
    {
        get
        {
            return this.gameObject.transform.localRotation;
        }
        set
        {
            this.gameObject.transform.localRotation = value;
        }
    }

    /// <summary>
    /// The joint objects local position.
    /// </summary>
    public Vector3 CurrentLocationPosition
    {
        get
        {
            return this.gameObject.transform.localPosition;
        }
        set
        {
            this.gameObject.transform.localPosition = value;
        }
    }

    /// <summary>
    /// The joint objects local position
    /// </summary>
    public Vector3 CurrentPosition
    {
        get
        {
            return this.gameObject.transform.position;
        }
        set
        {
            this.gameObject.transform.position = value;
        }
    }

    /// <summary>
    /// The joint object local rotation
    /// </summary>
    public Quaternion CurrentRotation
    {
        get
        {
            return this.gameObject.transform.rotation;
        }
        set
        {
            this.gameObject.transform.rotation = value;
        }
    }

    /// <summary>
    /// Initialise a new joint.
    /// </summary>
    /// <param name="gameObject"></param>
    public LLJoint(GameObject gameObject)
    {
        this.gameObject = gameObject;
        this.StartingPosition = gameObject.transform.position;
        this.StartingRotation = gameObject.transform.eulerAngles;
        this.startingLocalPosition = gameObject.transform.localPosition;
        this.startingLocalRotation = gameObject.transform.localEulerAngles;
    }
}