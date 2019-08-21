using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An animation step.
/// </summary>
public class Step
{
    /// <summary>
    /// The index of the movement being animated from the list of positions.
    /// </summary>
    public int Counter;
    /// <summary>
    /// The list of positions collected within this step.
    /// </summary>
    public List<StepPosition> Positions;

    private const float DistanceClearance = 0.001f;

    /// <summary>
    /// The joint to perform the this step on.
    /// </summary>
    public LLJoint joint;
    
    /// <summary>
    /// If the step type is a follower, the object to follow.
    /// </summary>
    public GameObject follower;

    /// <summary>
    /// The duration to follow the follower.
    /// </summary>
    public float followDuration;

    /// <summary>
    /// The speed to follow the follower.
    /// </summary>
    public float followSpeed;

    private Vector3 followerPosition
    {
        get 
        {
            Transform transform = joint.gameObject.transform.parent.transform;
            return transform.InverseTransformPoint(follower.transform.position);
        }
    }

    public enum FollowType
    {
        xyz,
        xy,
        yz,
        zx,
        x,
        y,
        z
    }

    public FollowType followType;

    public enum StepType
    {
        position,
        addition,
        follower
    }
    /// <summary>
    /// The type of step this step is.
    /// </summary>
    public StepType stepType;

    public enum DirectionType
    {
        position,
        rotation
    }
    /// <summary>
    /// The movement direction of this step.
    /// </summary>
    public DirectionType directionType;

    /// <summary>
    /// Determines if this step has been completed.
    /// </summary>
    public bool isComplete;

    /// <summary>
    /// The next position for addition.
    /// </summary>
    public Vector3 MoveToPosition;

    private bool moveToHasBeenSet = false;
    private Vector3 nextMoveToPosition
    {
        get
        {
            return MoveToPosition;
        }
        set
        {
            MoveToPosition = value;
            moveToHasBeenSet = true;
        }
    }
    /// <summary>
    /// Calculates the length of the current step journey.
    /// </summary>
    private float JourneyLength
    {
        get
        {
            if (directionType == DirectionType.position)
                return Vector3.Distance(joint.CurrentLocationPosition, Positions[Counter].position);
            else
                return Quaternion.Angle(joint.CurrentLocationRotation, Quaternion.Euler(Positions[Counter].position));
        }
    }

    /// <summary>
    /// The starting time of the animation.
    /// </summary>
    private float time;

    /// <summary>
    /// Initialise a new position type step.
    /// </summary>
    /// <param name="Counter"></param>
    /// <param name="Positions"></param>
    public Step(LLJoint joint, List<StepPosition> Positions, DirectionType directionType, StepType stepType)
    {
        this.joint = joint;
        this.Counter = 0;
        this.Positions = Positions;
        this.stepType = stepType;
        this.directionType = directionType;
        this.time = 0;
    }

    /// <summary>
    /// Initialise a new follower type step.
    /// </summary>
    /// <param name="follower"></param>
    public Step(LLJoint joint, GameObject follower, DirectionType directionType, FollowType followType, float followSpeed, float followDuration)
    {
        this.joint = joint;
        this.follower = follower;
        this.stepType = StepType.follower;
        this.directionType = directionType;
        this.time = 0;
        this.followType = followType;
        this.followSpeed = followSpeed;
        this.followDuration = followDuration;
    }

    /// <summary>
    /// Performs the animation.
    /// </summary>
    public void PerformStep()
    {
        // Depending on the step type
        switch(stepType)
        {
            // If positional step
            case StepType.position:
                if (Counter < Positions.Count)
                {
                    switch (directionType)
                    {
                        case DirectionType.position:
                            MoveTowards();
                            break;
                        case DirectionType.rotation:
                            RotateTowards();
                            break;
                    }
                }
                else
                {
                    isComplete = true;
                }
                break;
            // If additional step
            case StepType.addition:
                if(Counter < Positions.Count)
                {
                    switch(directionType)
                    {
                        case DirectionType.position:
                            AddMovementTowards();
                            break;
                        case DirectionType.rotation:
                            AddRotationTowards();
                            break;
                    }
                }
                else
                {
                    isComplete = true;
                }
                break;
            // If follower step
            case StepType.follower:
                switch(directionType)
                {
                    case DirectionType.position:
                        FollowMovement();
                        break;
                    case DirectionType.rotation:
                        FollowRotation();
                        break;
                }
                break;
        }
    }

    /// <summary>
    /// Moves the object towards the current position.
    /// </summary>
    private void MoveTowards()
    {
        // Check the distance between the two objects.
        if (Vector3.Distance(joint.CurrentLocationPosition, Positions[Counter].position) >= DistanceClearance)
        {
            float speed = GetSpeed(Positions[Counter].time);
            // Move object towards position.
            joint.CurrentLocationPosition = Vector3.Lerp(joint.CurrentLocationPosition, Positions[Counter].position, speed);
        }
        else
        {
            // Increment the step if distance is close enough.
            Counter++;
            time = 0;
        }
    }

    /// <summary>
    /// Moves the object towards the current rotation.
    /// </summary>
    private void RotateTowards()
    {
        // Check the distance between the two rotations.
        if (Vector3.Distance(joint.CurrentLocationEuler, Positions[Counter].position) >= DistanceClearance)
        {
            float speed = GetSpeed(Positions[Counter].time);
            Quaternion rotation = Quaternion.Euler(Positions[Counter].position);
            // Rotate towards position.
            joint.CurrentLocationRotation = Quaternion.Lerp(joint.CurrentLocationRotation, rotation, speed);
        }
        else
        {
            // Increment the step if distance is close enough.
            Counter++;
            time = 0;
        }
    }

    /// <summary>
    /// Move Position towards target being (currentPositionAtEndOfStep + Position).
    /// </summary>
    private void AddMovementTowards()
    {
        // Set moveToPosition
        if(!moveToHasBeenSet)
        {
            nextMoveToPosition = joint.CurrentLocationPosition + Positions[Counter].position;
        }

        // Check the distance between the two objects.
        if (Vector3.Distance(joint.CurrentLocationPosition, MoveToPosition) >= DistanceClearance)
        {
            float speed = GetSpeed(Positions[Counter].time);
            // Move object towards position.
            joint.CurrentLocationPosition = Vector3.Lerp(joint.CurrentLocationPosition, MoveToPosition, speed);
        }
        else
        {
            // Increment the step if distance is close enough.
            Counter++;
            time = 0;
            moveToHasBeenSet = false;
        }
    }

    /// <summary>
    /// Move Rotation towards target being (currentRotationAtEndOfStep + Rotation).
    /// </summary>
    private void AddRotationTowards()
    {
        // Set moveToPosition
        if (!moveToHasBeenSet)
        {
            nextMoveToPosition = joint.CurrentLocationEuler + Positions[Counter].position;
        }

        // Check the distance between the two rotations.
        if (Vector3.Distance(joint.CurrentLocationEuler, MoveToPosition) >= DistanceClearance)
        {
            float speed = GetSpeed(Positions[Counter].time);
            Quaternion rotation = Quaternion.Euler(MoveToPosition);
            // Rotate towards position.
            joint.CurrentLocationRotation = Quaternion.Lerp(joint.CurrentLocationRotation, rotation, speed);
        }
        else
        {
            // Increment the step if distance is close enough.
            Counter++;
            time = 0;
            moveToHasBeenSet = false;
        }
    }

    /// <summary>
    /// Follow the targeted players movement.
    /// </summary>
    private void FollowMovement()
    {

        Vector3 position = Vector3.zero; 

        switch (followType)
        {
            case FollowType.xyz:
                position = follower.transform.position;
                break;
            case FollowType.zx:
                Vector3 zx = follower.transform.position;
                position = new Vector3(zx.x, joint.CurrentPosition.y, zx.z);
                break;
            case FollowType.yz:
                Vector3 yz = follower.transform.position;
                position = new Vector3(joint.CurrentPosition.x, yz.y, yz.z);
                break;
            case FollowType.xy:
                Vector3 xy = follower.transform.position;
                position = new Vector3(xy.x, xy.y, joint.CurrentPosition.z);
                break;
            case FollowType.x:
                Vector3 x = follower.transform.position;
                position = new Vector3(x.x, joint.CurrentPosition.y, joint.CurrentPosition.z);
                break;
            case FollowType.y:
                Vector3 y = follower.transform.position;
                position = new Vector3(joint.CurrentPosition.x, y.y, joint.CurrentPosition.z);
                break;
            case FollowType.z:
                Vector3 z = follower.transform.position;
                position = new Vector3(joint.CurrentPosition.x, joint.CurrentPosition.y, z.z);
                break;
        }

        // Check the distance between the two objects.
        if (followDuration >= 0)
        {
            float speed = GetSpeed(followSpeed);
            // Move object towards position.
            joint.CurrentPosition = Vector3.Lerp(joint.CurrentPosition, position, speed);
        }
        else
        {
            // Increment the step if distance is close enough.
            time = 0;
            followDuration = 0;
            isComplete = true;
        }

        followDuration -= Time.deltaTime;
    }

    /// <summary>
    /// Follow the targeted players rotation.
    /// </summary>
    private void FollowRotation()
    {
        Quaternion rotation = Quaternion.LookRotation(follower.transform.position - joint.CurrentPosition);

        switch (followType)
        {
            case FollowType.xyz:
                break;
            case FollowType.zx:
                break;
            case FollowType.yz:
                break;
            case FollowType.xy:
                break;
            case FollowType.x:
                break;
            case FollowType.y:
                break;
            case FollowType.z:
                break;
        }

        // Check the distance between the two objects.
        if (followDuration >= 0)
        {
            float speed = GetSpeed(followSpeed);
            // Move object towards position.
            joint.CurrentRotation = Quaternion.RotateTowards(joint.CurrentRotation, rotation, speed);
        }
        else
        {
            // Increment the step if distance is close enough.
            time = 0;
            followDuration = 0;
            isComplete = true;
        }

        followDuration -= Time.deltaTime;
    }

    /// <summary>
    /// Gets the speed of the animation.
    /// </summary>
    /// <returns></returns>
    private float GetSpeed(float duration)
    {
        time += Time.deltaTime / duration;
        return time;
    }

    /// <summary>
    /// Resets the step to it's initialstate after the animation has been completed.
    /// </summary>
    public void Reset()
    {
        this.isComplete = false;
        this.Counter = 0;
        this.time = 0;
    }
}