using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentJoint : MonoBehaviour
{

    /// <summary>
    /// The parented joint for this joint. Used for maximum distance and rotation.
    /// </summary>
    public GameObject Joint;

    /// <summary>
    /// If yes, used for calculating max from default location.
    /// </summary>
    public bool GetDefault;

    /// <summary>
    /// The maximum distance the target can get from the parent.
    /// </summary>
    [System.NonSerialized]
    public float maxDistance;

    /// <summary>
    /// The min distance the target can get from the parent.
    /// </summary>
    public float minDistance;

}
