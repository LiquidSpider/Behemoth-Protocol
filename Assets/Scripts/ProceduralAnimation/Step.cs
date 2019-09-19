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

    public float SetupDurection;

    /// <summary>
    /// The speed to follow the follower.
    /// </summary>
    public float followSpeed;

    private Vector3 followerPosition
    {
        get
        {
            Transform transform = joint.jointObject.transform.parent.transform;
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
        rotation,
        matchRotation
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
        this.SetupDurection = followDuration;
    }

    /// <summary>
    /// Performs the animation.
    /// </summary>
    public void PerformStep()
    {
        // Depending on the step type
        switch (stepType)
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
                if (Counter < Positions.Count)
                {
                    switch (directionType)
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
                switch (directionType)
                {
                    case DirectionType.position:
                        FollowMovement();
                        break;
                    case DirectionType.rotation:
                        FollowRotation();
                        break;
                    case DirectionType.matchRotation:
                        MatchRotation();
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
            joint.CurrentLocationPosition = Vector3.MoveTowards(joint.CurrentLocationPosition, Positions[Counter].position, speed);
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
            joint.CurrentLocationRotation = Quaternion.RotateTowards(joint.CurrentLocationRotation, rotation, speed);
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
        if (!moveToHasBeenSet)
        {
            nextMoveToPosition = joint.CurrentLocationPosition + Positions[Counter].position;
        }

        // Check the distance between the two objects.
        if (Vector3.Distance(joint.CurrentLocationPosition, MoveToPosition) >= DistanceClearance)
        {
            float speed = GetSpeed(Positions[Counter].time);
            // Move object towards position.
            joint.CurrentLocationPosition = Vector3.MoveTowards(joint.CurrentLocationPosition, MoveToPosition, speed);
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
            joint.CurrentLocationRotation = Quaternion.RotateTowards(joint.CurrentLocationRotation, rotation, speed);
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

        Vector3 position = follower.transform.position;

        switch (followType)
        {
            case FollowType.xyz:
                break;
            case FollowType.zx:
                position = new Vector3(position.x, joint.CurrentPosition.y, position.z);
                break;
            case FollowType.yz:
                position = new Vector3(joint.CurrentPosition.x, position.y, position.z);
                break;
            case FollowType.xy:
                position = new Vector3(position.x, position.y, joint.CurrentPosition.z);
                break;
            case FollowType.x:
                position = new Vector3(position.x, joint.CurrentPosition.y, joint.CurrentPosition.z);
                break;
            case FollowType.y:
                position = new Vector3(joint.CurrentPosition.x, position.y, joint.CurrentPosition.z);
                break;
            case FollowType.z:
                position = new Vector3(joint.CurrentPosition.x, joint.CurrentPosition.y, position.z);
                break;
        }

        // Check the distance between the two objects.
        if (followDuration >= 0)
        {
            // If the joint has a parent.
            if (joint.parentJoint)
            {

                // Get the new position
                Vector3 movingPosition = Vector3.MoveTowards(joint.CurrentPosition, position, followSpeed);
                Vector3 parentPosition = joint.parentJoint.Joint.transform.position;
                // The distance between the new position and the parent joint.
                float distance = Vector3.Distance(movingPosition, parentPosition);

                // if we're greater than the distance then clamp it by the distance * normal between centre
                if (distance > joint.parentJoint.maxDistance)
                {
                    Vector3 fromOrigin = (movingPosition - parentPosition).normalized * joint.parentJoint.maxDistance;
                    movingPosition = parentPosition + fromOrigin;
                }

                joint.CurrentPosition = movingPosition;
            }
            else
            {
                // Move object towards position.
                //joint.CurrentLocationPosition = Vector3.MoveTowards(joint.CurrentLocationPosition, position, followSpeed);
            }
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
    private void MatchRotation()
    {
        Vector3 euler = follower.transform.eulerAngles;
        Vector3 currentEuler = joint.CurrentRotation.eulerAngles;

        switch (followType)
        {
            case FollowType.xyz:
                break;
            case FollowType.zx:
                euler = new Vector3(euler.x, currentEuler.y, euler.z);
                break;
            case FollowType.yz:
                euler = new Vector3(currentEuler.x, euler.y, euler.z);
                break;
            case FollowType.xy:
                euler = new Vector3(euler.x, euler.y, currentEuler.z);
                break;
            case FollowType.x:
                euler = new Vector3(euler.x, currentEuler.y, currentEuler.z);
                break;
            case FollowType.y:
                euler = new Vector3(currentEuler.x, euler.y, currentEuler.z);
                break;
            case FollowType.z:
                euler = new Vector3(currentEuler.x, currentEuler.y, euler.z);
                break;
        }

        Quaternion rotation = Quaternion.Euler(euler);

        // Check the distance between the two objects.
        if (followDuration >= 0)
        {
            //float speed = GetSpeed(followSpeed);
            // Move object towards position.
            joint.CurrentRotation = Quaternion.RotateTowards(joint.CurrentRotation, rotation, followSpeed);
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
        Vector3 euler = Quaternion.LookRotation(follower.transform.position - joint.CurrentPosition).eulerAngles;
        Vector3 currentEuler = joint.CurrentRotation.eulerAngles;

        switch (followType)
        {
            case FollowType.xyz:
                break;
            case FollowType.zx:
                euler = new Vector3(euler.x, currentEuler.y, euler.z);
                break;
            case FollowType.yz:
                euler = new Vector3(currentEuler.x, euler.y, euler.z);
                break;
            case FollowType.xy:
                euler = new Vector3(euler.x, euler.y, currentEuler.z);
                break;
            case FollowType.x:
                euler = new Vector3(euler.x, currentEuler.y, currentEuler.z);
                break;
            case FollowType.y:
                euler = new Vector3(currentEuler.x, euler.y, currentEuler.z);
                break;
            case FollowType.z:
                euler = new Vector3(currentEuler.x, currentEuler.y, euler.z);
                break;
        }

        Quaternion rotation = Quaternion.Euler(euler);

        // Check the distance between the two objects.
        if (followDuration >= 0)
        {
            //float speed = GetSpeed(followSpeed);
            // Move object towards position.
            joint.CurrentRotation = Quaternion.RotateTowards(joint.CurrentRotation, rotation, followSpeed);
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

    public Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles)
    {
        return Quaternion.Euler(angles) * (point - pivot) + pivot;
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
        this.followDuration = SetupDurection;
    }

}