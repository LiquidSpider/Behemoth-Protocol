using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents a step position with a travel time.
/// </summary>
public class StepPosition
{
    /// <summary>
    /// The position or rotation in local space.
    /// </summary>
    public Vector3 position;
    /// <summary>
    /// The time it takes to travel to this position or rotation.
    /// </summary>
    public float time;

    /// <summary>
    /// Create a new instance of a StepPosition.
    /// </summary>
    public StepPosition(Vector3 position, float time)
    {
        this.position = position;
        this.time = time;
    }
}
