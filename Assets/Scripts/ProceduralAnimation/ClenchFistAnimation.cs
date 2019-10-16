using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Stores the clench fist animation.
/// </summary>
public class ClenchFistAnimation
{
    /// <summary>
    /// Determines if the fist is clenched.
    /// </summary>
    public bool isClenched;

    /// <summary>
    /// The steps to clench the fist.
    /// </summary>
    private List<Step> clentchFistAnimationLeft;
    private List<Step> clentchFistAnimationRight;
    private List<Step> unClentchFistAnimationLeft;
    private List<Step> unClentchFistAnimationRight;

    public enum Hand
    {
        left,
        right,
        both
    }
    public Hand MoveHands;

    public bool clenchAnimation;

    /// <summary>
    /// The joints of the fingers:
    /// LHPinkyTip      0
    /// LHPinkyJoint    1
    /// LHRingTip       2
    /// LHRingJoint     3
    /// LHMiddleTip     4
    /// LHMiddleJoint   5
    /// LHIndexTip      6
    /// LHIndexJoint    7
    /// LHThumbTip      8
    /// LHThumbJoint    9
    /// RHPinkyTip      10
    /// RHPinkyJoint    11
    /// RHRingTip       12
    /// RHRingJoint     13
    /// RHMiddleTip     14
    /// RHMiddleJoint   15
    /// RHIndexTip      16
    /// RHIndexJoint    17
    /// RHThumbTip      18
    /// RHThumbJoint    19
    /// </summary>
    List<LLJoint> FingerJoints;

    public float AnimationSpeed;
    public float Speed
    {
        get
        {
            return AnimationSpeed;
        }
        set
        {
            AnimationSpeed = value;
            Setup();
        }
    }

    /// <summary>
    /// Initialise a new clenchFistAnimation.
    /// </summary>
    /// <param name="hand"></param>
    public ClenchFistAnimation(List<LLJoint> FingerJoints)
    {
        // Default variable values.
        isClenched = false;
        clenchAnimation = true;
        MoveHands = Hand.both;
        this.FingerJoints = FingerJoints;
    }

    /// <summary>
    /// Sets up the animation.
    /// </summary>
    public void Setup()
    {
        clentchFistAnimationLeft = new List<Step>()
        {
            // Pinky Target
            new Step(FingerJoints[0], new List<StepPosition>()
            {
                new StepPosition(new Vector3(0.6f, 0.0f, -0.3f), 0.2f * AnimationSpeed),
                new StepPosition(new Vector3(0.5f, 0.0f, -0.5f), 0.2f * AnimationSpeed),
                new StepPosition(new Vector3(0.4f, 0.0f, -0.5f), 0.2f * AnimationSpeed),
                new StepPosition(new Vector3(-0.1f, 0.0f, -0.4f), 0.2f * AnimationSpeed),
                new StepPosition(new Vector3(-0.1f, 0.0f, -0.4f), 0.2f * AnimationSpeed)
            }, Step.DirectionType.position, Step.StepType.addition),
            new Step(FingerJoints[0], new List<StepPosition>()
            {
                new StepPosition(new Vector3(0.0f, 45.0f, 0.0f), 1.0f),
                new StepPosition(new Vector3(0.0f, 45.0f, 0.0f), 1.0f)
            }, Step.DirectionType.rotation, Step.StepType.addition),
            // Pinky Joint
            new Step(FingerJoints[1], new List<StepPosition>()
            {
                new StepPosition(new Vector3(-0.2f, 0.0f, 0.0f), 0.2f * AnimationSpeed)
            }, Step.DirectionType.position, Step.StepType.addition),
            // Ring Target
            new Step(FingerJoints[2], new List<StepPosition>()
            {
                new StepPosition(new Vector3(0.6f, 0.0f, -0.3f), 0.2f * AnimationSpeed),
                new StepPosition(new Vector3(0.5f, 0.0f, -0.6f), 0.2f * AnimationSpeed),
                new StepPosition(new Vector3(0.4f, 0.0f, -0.6f), 0.2f * AnimationSpeed),
                new StepPosition(new Vector3(-0.2f, 0.0f, -0.4f), 0.2f * AnimationSpeed),
                new StepPosition(new Vector3(-0.2f, 0.0f, -0.4f), 0.2f * AnimationSpeed)
            }, Step.DirectionType.position, Step.StepType.addition),
            new Step(FingerJoints[2], new List<StepPosition>()
            {
                new StepPosition(new Vector3(0.0f, 45.0f, 0.0f), 1.0f),
                new StepPosition(new Vector3(0.0f, 45.0f, 0.0f), 1.0f)
            }, Step.DirectionType.rotation, Step.StepType.addition),
            // Ring Joint
            new Step(FingerJoints[3], new List<StepPosition>()
            {
                new StepPosition(new Vector3(-0.2f, 0.0f, 0.0f), 0.2f * AnimationSpeed)
            }, Step.DirectionType.position, Step.StepType.addition),
            // Middle Target
            new Step(FingerJoints[4], new List<StepPosition>()
            {
                new StepPosition(new Vector3(0.6f, 0.0f, -0.3f), 0.2f * AnimationSpeed),
                new StepPosition(new Vector3(0.5f, 0.0f, -0.6f), 0.2f * AnimationSpeed),
                new StepPosition(new Vector3(0.6f, 0.0f, -0.6f), 0.2f * AnimationSpeed),
                new StepPosition(new Vector3(-0.4f, 0.0f, -0.5f), 0.2f * AnimationSpeed),
                new StepPosition(new Vector3(-0.2f, 0.0f, -0.5f), 0.2f * AnimationSpeed)
            }, Step.DirectionType.position, Step.StepType.addition),
            new Step(FingerJoints[4], new List<StepPosition>()
            {
                new StepPosition(new Vector3(0.0f, 45.0f, 0.0f), 1.0f),
                new StepPosition(new Vector3(0.0f, 45.0f, 0.0f), 1.0f)
            }, Step.DirectionType.rotation, Step.StepType.addition),
            // Middle Joint
            new Step(FingerJoints[5], new List<StepPosition>()
            {
                new StepPosition(new Vector3(-0.2f, 0.0f, 0.0f), 0.2f * AnimationSpeed)
            }, Step.DirectionType.position, Step.StepType.addition),
            // Index Target
            new Step(FingerJoints[6], new List<StepPosition>()
            {
                new StepPosition(new Vector3(0.8f, 0.0f, -0.3f), 0.2f * AnimationSpeed),
                new StepPosition(new Vector3(0.5f, 0.0f, -0.6f), 0.2f * AnimationSpeed),
                new StepPosition(new Vector3(0.6f, 0.0f, -0.6f), 0.2f * AnimationSpeed),
                new StepPosition(new Vector3(-0.4f, 0.0f, -0.5f), 0.2f * AnimationSpeed),
                new StepPosition(new Vector3(-0.2f, 0.0f, -0.5f), 0.2f * AnimationSpeed)
            }, Step.DirectionType.position, Step.StepType.addition),
            new Step(FingerJoints[6], new List<StepPosition>()
            {
                new StepPosition(new Vector3(0.0f, 45.0f, 0.0f), 1.0f),
                new StepPosition(new Vector3(0.0f, 45.0f, 0.0f), 1.0f)
            }, Step.DirectionType.rotation, Step.StepType.addition),
            // Index Joint
            new Step(FingerJoints[7], new List<StepPosition>()
            {
                new StepPosition(new Vector3(-0.2f, 0.0f, 0.0f), 0.2f * AnimationSpeed)
            }, Step.DirectionType.position, Step.StepType.addition),
            // Thumb Target
            new Step(FingerJoints[8], new List<StepPosition>()
            {
                new StepPosition(new Vector3(1.0f, 0.0f, -0.5f), 0.2f * AnimationSpeed),
            }, Step.DirectionType.position, Step.StepType.addition),
            new Step(FingerJoints[8], new List<StepPosition>()
            {
                new StepPosition(new Vector3(90.0f, 45.0f, 0.0f), 1.0f)
            }, Step.DirectionType.rotation, Step.StepType.addition),
            // Thumb Joint
            new Step(FingerJoints[9], new List<StepPosition>()
            {
                new StepPosition(new Vector3(0.0f, 1.0f, 0.0f), 0.2f * AnimationSpeed),
            }, Step.DirectionType.position, Step.StepType.addition),
        };

        clentchFistAnimationRight = new List<Step>()
        {
            // Pinky Target
            new Step(FingerJoints[10], new List<StepPosition>()
            {
                new StepPosition(new Vector3(-0.6f, 0.0f, -0.3f), 0.2f * AnimationSpeed),
                new StepPosition(new Vector3(-0.5f, 0.0f, -0.3f), 0.2f * AnimationSpeed),
                new StepPosition(new Vector3(-0.4f, 0.0f, -0.3f), 0.2f * AnimationSpeed),
                new StepPosition(new Vector3(0.1f, 0.0f, -0.2f), 0.2f * AnimationSpeed),
                new StepPosition(new Vector3(0.1f, 0.0f, -0.2f), 0.2f * AnimationSpeed)
            }, Step.DirectionType.position, Step.StepType.addition),
            new Step(FingerJoints[10], new List<StepPosition>()
            {
                new StepPosition(new Vector3(0.0f, -45.0f, 0.0f), 1.0f),
                new StepPosition(new Vector3(0.0f, -45.0f, 0.0f), 1.0f)
            }, Step.DirectionType.rotation, Step.StepType.addition),
            // Pinky Joint
            new Step(FingerJoints[11], new List<StepPosition>()
            {
                new StepPosition(new Vector3(0.2f, 0.0f, 0.0f), 0.2f * AnimationSpeed)
            }, Step.DirectionType.position, Step.StepType.addition),
            // Ring Target
            new Step(FingerJoints[12], new List<StepPosition>()
            {
                new StepPosition(new Vector3(-0.6f, 0.0f, -0.3f), 0.2f * AnimationSpeed),
                new StepPosition(new Vector3(-0.5f, 0.0f, -0.6f), 0.2f * AnimationSpeed),
                new StepPosition(new Vector3(-0.4f, 0.0f, -0.6f), 0.2f * AnimationSpeed),
                new StepPosition(new Vector3(0.2f, 0.0f, -0.4f), 0.2f * AnimationSpeed),
                new StepPosition(new Vector3(0.2f, 0.0f, -0.4f), 0.2f * AnimationSpeed)
            }, Step.DirectionType.position, Step.StepType.addition),
            new Step(FingerJoints[12], new List<StepPosition>()
            {
                new StepPosition(new Vector3(0.0f, -45.0f, 0.0f), 1.0f),
                new StepPosition(new Vector3(0.0f, -45.0f, 0.0f), 1.0f)
            }, Step.DirectionType.rotation, Step.StepType.addition),
            // Ring Joint
            new Step(FingerJoints[13], new List<StepPosition>()
            {
                new StepPosition(new Vector3(0.2f, 0.0f, 0.0f), 0.2f * AnimationSpeed)
            }, Step.DirectionType.position, Step.StepType.addition),
            // Middle Target
            new Step(FingerJoints[14], new List<StepPosition>()
            {
                new StepPosition(new Vector3(-0.6f, 0.0f, -0.3f), 0.2f * AnimationSpeed),
                new StepPosition(new Vector3(-0.5f, 0.0f, -0.6f), 0.2f * AnimationSpeed),
                new StepPosition(new Vector3(-0.6f, 0.0f, -0.6f), 0.2f * AnimationSpeed),
                new StepPosition(new Vector3(0.4f, 0.0f, -0.5f), 0.2f * AnimationSpeed),
                new StepPosition(new Vector3(0.2f, 0.0f, -0.5f), 0.2f * AnimationSpeed)
            }, Step.DirectionType.position, Step.StepType.addition),
            new Step(FingerJoints[14], new List<StepPosition>()
            {
                new StepPosition(new Vector3(0.0f, -45.0f, 0.0f), 1.0f),
                new StepPosition(new Vector3(0.0f, -45.0f, 0.0f), 1.0f)
            }, Step.DirectionType.rotation, Step.StepType.addition),
            // Middle Joint
            new Step(FingerJoints[15], new List<StepPosition>()
            {
                new StepPosition(new Vector3(0.2f, 0.0f, 0.0f), 0.2f * AnimationSpeed)
            }, Step.DirectionType.position, Step.StepType.addition),
            // Index Target
            new Step(FingerJoints[16], new List<StepPosition>()
            {
                new StepPosition(new Vector3(-0.8f, 0.0f, -0.3f), 0.2f * AnimationSpeed),
                new StepPosition(new Vector3(-0.5f, 0.0f, -0.6f), 0.2f * AnimationSpeed),
                new StepPosition(new Vector3(-0.6f, 0.0f, -0.6f), 0.2f * AnimationSpeed),
                new StepPosition(new Vector3(0.4f, 0.0f, -0.5f), 0.2f * AnimationSpeed),
                new StepPosition(new Vector3(0.2f, 0.0f, -0.5f), 0.2f * AnimationSpeed)
            }, Step.DirectionType.position, Step.StepType.addition),
            new Step(FingerJoints[16], new List<StepPosition>()
            {
                new StepPosition(new Vector3(0.0f, -45.0f, 0.0f), 1.0f),
                new StepPosition(new Vector3(0.0f, -45.0f, 0.0f), 1.0f)
            }, Step.DirectionType.rotation, Step.StepType.addition),
            // Index Joint
            new Step(FingerJoints[17], new List<StepPosition>()
            {
                new StepPosition(new Vector3(0.2f, 0.0f, 0.0f), 0.2f * AnimationSpeed)
            }, Step.DirectionType.position, Step.StepType.addition),
            // Thumb Target
            new Step(FingerJoints[18], new List<StepPosition>()
            {
                new StepPosition(new Vector3(-1.0f, 0.0f, -0.5f), 0.2f * AnimationSpeed),
            }, Step.DirectionType.position, Step.StepType.addition),
            new Step(FingerJoints[18], new List<StepPosition>()
            {
                new StepPosition(new Vector3(90.0f+180, 45.0f+180, 0.0f), 1.0f)
            }, Step.DirectionType.rotation, Step.StepType.addition),
            // Thumb Joint
            new Step(FingerJoints[19], new List<StepPosition>()
            {
                new StepPosition(new Vector3(0.0f, 1.0f, 0.0f), 0.2f * AnimationSpeed),
            }, Step.DirectionType.position, Step.StepType.addition),
        };

        unClentchFistAnimationLeft = new List<Step>()
        {
            new Step(FingerJoints[0], new List<StepPosition>()
            {
                new StepPosition(new Vector3(0.05f, -0.025f, 0.0f), 0.2f * AnimationSpeed),
                new StepPosition(new Vector3(0.1f, -0.05f, 0.0f), 0.2f * AnimationSpeed),
                new StepPosition(new Vector3(0.1f, -0.075f, 0.0f), 0.2f * AnimationSpeed),
                new StepPosition(new Vector3(0.05f, -0.1f, 0.0f), 0.2f * AnimationSpeed)
            }, Step.DirectionType.position, Step.StepType.addition)
        };

        unClentchFistAnimationRight = new List<Step>()
        {
            new Step(FingerJoints[0], new List<StepPosition>()
            {
                new StepPosition(new Vector3(0.05f, -0.025f, 0.0f), 0.2f * AnimationSpeed),
                new StepPosition(new Vector3(0.1f, -0.05f, 0.0f), 0.2f * AnimationSpeed),
                new StepPosition(new Vector3(0.1f, -0.075f, 0.0f), 0.2f * AnimationSpeed),
                new StepPosition(new Vector3(0.05f, -0.1f, 0.0f), 0.2f * AnimationSpeed)
            }, Step.DirectionType.position, Step.StepType.addition)
        };
    }

    /// <summary>
    /// Performs the animation.
    /// </summary>
    public void PerformAnimation()
    {
        // If we're clench and we're not clenched then clench fist.
        if (clenchAnimation && !isClenched)
        {
            switch (MoveHands)
            {
                case Hand.left:
                    if (PerformAnimationStep(clentchFistAnimationLeft))
                    {
                        isClenched = true;

                        // Reset all the animations
                        foreach (Step step in unClentchFistAnimationLeft)
                        {
                            step.Reset();
                        }
                    }
                    break;
                case Hand.right:
                    if (PerformAnimationStep(clentchFistAnimationRight))
                    {
                        isClenched = true;

                        // Reset all the animations
                        foreach (Step step in unClentchFistAnimationRight)
                        {
                            step.Reset();
                        }
                    }
                    break;
                case Hand.both:
                    if (PerformAnimationStep(clentchFistAnimationLeft) & PerformAnimationStep(clentchFistAnimationRight))
                    {
                        isClenched = true;

                        // Reset all the animations
                        foreach (Step step in unClentchFistAnimationLeft)
                        {
                            step.Reset();
                        }
                        // Reset all the animations
                        foreach (Step step in unClentchFistAnimationRight)
                        {
                            step.Reset();
                        }
                    }
                    break;
            }
        }
        // If we're unclench and we're currently clenched then unclench.
        else if (!clenchAnimation && isClenched)
        {
            switch (MoveHands)
            {
                case Hand.left:
                    if (PerformAnimationStep(unClentchFistAnimationLeft))
                    {
                        isClenched = false;

                        // Reset all the animations
                        foreach (Step step in unClentchFistAnimationLeft)
                        {
                            step.Reset();
                        }
                    }
                    break;
                case Hand.right:
                    if (PerformAnimationStep(unClentchFistAnimationRight))
                    {
                        isClenched = false;

                        // Reset all the animations
                        foreach (Step step in unClentchFistAnimationRight)
                        {
                            step.Reset();
                        }
                    }
                    break;
                case Hand.both:
                    if (PerformAnimationStep(unClentchFistAnimationLeft) & PerformAnimationStep(unClentchFistAnimationRight))
                    {
                        isClenched = false;

                        // Reset all the animations
                        foreach (Step step in unClentchFistAnimationLeft)
                        {
                            step.Reset();
                        }
                        // Reset all the animations
                        foreach (Step step in unClentchFistAnimationRight)
                        {
                            step.Reset();
                        }
                    }
                    break;
            }
        }
    }

    /// <summary>
    /// Performs a given animation step.
    /// </summary>
    /// <param name="steps"></param>
    public bool PerformAnimationStep(List<Step> steps)
    {
        // Perform the animation step
        foreach (Step step in steps)
        {
            if (!step.isComplete)
            {
                step.PerformStep();
            }
        }

        // Check if finished.
        if (steps.TrueForAll(IsComplete))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// predicate returns true if step is complete.
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    private static bool IsComplete(Step s)
    {
        return s.isComplete;
    }
}
