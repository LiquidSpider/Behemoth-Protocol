﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.Data;
public class GiantAnimator : MonoBehaviour
{
    //public GameObject[] Kinematics;
    public GameObject ChestPiece;
    public GameObject[] LeftHandKinematics;
    public GameObject[] LeftArmKinematics;
    public GameObject[] RightHandKinematics;
    public GameObject[] RightArmKinematics;
    public GameObject[] LeftLegKinematics;
    public GameObject[] RightLegKinematics;
    public GameObject[] ChestKinematics;

    private giantBehaviour giantBehaviour;

    private GameManager gameManager;

    private GiantSoundManager gsm;

    private GameObject Player;
    public bool isComplete;

    public enum Animation
    {
        idle,
        GiantDamPunch,
        GiantClap,
        //GiantSwipeUp,
        GiantSwing,
        Laser,
        FinalLaser,
        DamLaser
    }

    [Header("Animation States")]
    public Animation currentAnimation;

    #region KinematicVariables
    // Joints
    private LLJoint LeftHand;
    private LLJoint LeftElbow;
    private LLJoint RightHand;
    private LLJoint RightElbow;
    private LLJoint LeftLeg;
    private LLJoint LeftKnee;
    private LLJoint RightLeg;
    private LLJoint RightKnee;
    private LLJoint Chest;
    private LLJoint Waist;
    private LLJoint Spine;

    // Finger joints
    private LLJoint LHPinkyTip;
    private LLJoint LHPinkyJoint;
    private LLJoint LHRingTip;
    private LLJoint LHRingJoint;
    private LLJoint LHMiddleTip;
    private LLJoint LHMiddleJoint;
    private LLJoint LHIndexTip;
    private LLJoint LHIndexJoint;
    private LLJoint LHThumbTip;
    private LLJoint LHThumbJoint;
    private LLJoint RHPinkyTip;
    private LLJoint RHPinkyJoint;
    private LLJoint RHRingTip;
    private LLJoint RHRingJoint;
    private LLJoint RHMiddleTip;
    private LLJoint RHMiddleJoint;
    private LLJoint RHIndexTip;
    private LLJoint RHIndexJoint;
    private LLJoint RHThumbTip;
    private LLJoint RHThumbJoint;
    #endregion

    [Header("Animation Speed Variables")]
    public float DestructionAnimationSpeed;
    public float DestructionAnimationRotationSpeed;
    public float LaserFirstStepSpeed;
    public float LaserFirstStepRotationSpeed;
    public float LaserHandFollowSpeed;
    public float LaserFingerFollowSpeed;
    public float LaserSecondStepSpeed;
    public float LaserSecondStepRotationSpeed;
    public float ClapAnimationSpeed;
    public float ClapAnimationRotationSpeed;
    public float PunchAnimationSpeed;
    public float PunchAnimationRotationSpeed;
    public float PunchReleaseAnimationSpeed;
    public float ChestLaserFollowSpeed;

    #region PunchVariables
    public GameObject PunchTargetL;
    public GameObject PunchTargetR;

    public enum PunchAnimationState
    {
        idle,
        DrawBack,
        Release,
        Recover
    }

    [Space(10)]
    public PunchAnimationState currentPunchState;

    private List<Step> damPunchAnimationPart0;
    private List<Step> damPunchAnimationPart1;
    #endregion

    private ClenchFistAnimation fistAnimation;

    #region ClapVariables

    public enum ClapAnimationState
    {
        idle,
        Bend,
        DrawBack,
        Release,
        Recover
    }

    [Space(10)]
    public ClapAnimationState currentClapAnimationState;

    private List<Step> clapAnimationPart0;
    private List<Step> clapAnimationPart1;
    private List<Step> clapAnimationPart2;
    private List<Step> clapAnimationPart3;

    public GameObject LeftHandClapTarget;
    public GameObject RightHandClapTarget;
    public GameObject clapTarget;
    #endregion

    #region SwipeUpVariables
    //private float SwipeUpAnimSpeed = 50.0f;

    //public enum Hand
    //{
    //    left,
    //    right
    //}
    //public Hand hand;

    //public enum SwipeUpAnimationState
    //{
    //    idle,
    //    swipe,
    //    recover,
    //}
    //[Space(10)]
    //public SwipeUpAnimationState CurrentSwipeUpState;

    //private List<Step> swipeUpAnimationLPart0;
    //private List<Step> swipeUpAnimationLPart1;
    //private List<Step> swipeUpAnimationRPart0;
    //private List<Step> swipeUpAnimationRPart1;
    #endregion

    #region SwingVariables

    public enum SwingAnimationState
    {
        idle,
        DrawBack,
        Release,
        Recover
    }
    [Space(10)]
    public SwingAnimationState CurrentSwingState;

    private List<Step> swingAnimationLPart0;
    private List<Step> swingAnimationLPart1;
    private List<Step> swingAnimationLPart2;
    private List<Step> swingAnimationRPart0;
    private List<Step> swingAnimationRPart1;
    private List<Step> swingAnimationRPart2;
    #endregion

    #region LaserVariables

    public enum LaserAnimationState
    {
        idle,
        aim,
        shoot,
        recover
    }
    public LaserAnimationState currentLaserState;

    private List<Step> laserAnimationLPart0;
    private List<Step> laserAnimationLPart1;
    private List<Step> laserAnimationLPart2;
    private List<Step> laserAnimationRPart0;
    private List<Step> laserAnimationRPart1;
    private List<Step> laserAnimationRPart2;

    public GameObject LeftLaserTarget;
    public GameObject LeftFingerLaserTarget;
    public GameObject RightLaserTarget;
    public GameObject RightFingerLaserTarget;
    public GameObject[] LFingerTargets;
    public GameObject[] RFingerTargets;

    public enum Hand
    {
        left,
        right
    }
    public Hand hand;
    #endregion

    #region FinalLaserVariables
    public enum FinalLaserAnimationState
    {
        idle,
        shoot,
        recover,
    }

    public FinalLaserAnimationState currentFinalLaserState;

    public GameObject ChestLaserTarget;
    private List<Step> finalLaserAnimationPart0;
    private List<Step> finalLaserAnimationPart1;
    #endregion

    #region DamLaser
    public enum DamLaserAnimationState
    {
        idle,
        aim,
        shoot,
        recover,
    }

    public DamLaserAnimationState currentDamLaserState;

    public GameObject DamLaserTarget;
    private List<Step> DamLaserAnimationPart0;
    private List<Step> DamLaserAnimationPart1;
    private List<Step> DamLaserAnimationPart2;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        isComplete = false;

        // Get the player
        this.Player = GameObject.FindGameObjectWithTag("Player");

        // Setup
        SetupKinematics();
        SetupFist();
        SetupLaserAnimation();
        SetupDamPunchAnimation();
        SetupClapAnimation();
        SetupSwingAnimation();
        SetupFinalLaserAnimation();
        SetupDamLaserAnimation();
        //SetupSwipeAnimation();

        // Get the giant behaviour
        this.giantBehaviour = this.GetComponent<giantBehaviour>();

        // Debug
        if (!Player)
        {
            Debug.Log("No Object with 'Player' tag found. Giant Disabled");
            this.gameObject.SetActive(false);
        }

        gsm = GetComponent<GiantSoundManager>();

    }

    /// <summary>
    /// Setup the object joints.
    /// </summary>
    private void SetupKinematics()
    {
        GameObject[] joints = GameObject.FindGameObjectsWithTag("Joints");
        LeftHand = new LLJoint(joints.Where(x => x.name == "LHTarget").First(), joints.Where(x => x.name == "LHTarget").First().GetComponent<ParentJoint>());
        LeftElbow = new LLJoint(joints.Where(x => x.name == "LHElbow").First());
        RightHand = new LLJoint(joints.Where(x => x.name == "RHTarget").First(), joints.Where(x => x.name == "RHTarget").First().GetComponent<ParentJoint>());
        RightElbow = new LLJoint(joints.Where(x => x.name == "RHElbow").First());
        LeftLeg = new LLJoint(joints.Where(x => x.name == "LLTarget").First(), joints.Where(x => x.name == "LLTarget").First().GetComponent<ParentJoint>());
        LeftKnee = new LLJoint(joints.Where(x => x.name == "LLKnee").First());
        RightLeg = new LLJoint(joints.Where(x => x.name == "RLTarget").First(), joints.Where(x => x.name == "RLTarget").First().GetComponent<ParentJoint>());
        RightKnee = new LLJoint(joints.Where(x => x.name == "RLKnee").First());
        Chest = new LLJoint(joints.Where(x => x.name == "ChestTarget").First(), joints.Where(x => x.name == "ChestTarget").First().GetComponent<ParentJoint>());
        Waist = new LLJoint(joints.Where(x => x.name == "Waist").First());
        Spine = new LLJoint(joints.Where(x => x.name == "Root_M").First());

        // Fingers
        LHPinkyTip = new LLJoint(joints.Where(x => x.name == "LHPinkyTarget").First(), joints.Where(x => x.name == "LHPinkyTarget").First().GetComponent<ParentJoint>());
        LHPinkyJoint = new LLJoint(joints.Where(x => x.name == "LHPinkyJoint").First());
        LHRingTip = new LLJoint(joints.Where(x => x.name == "LHRingTarget").First(), joints.Where(x => x.name == "LHRingTarget").First().GetComponent<ParentJoint>());
        LHRingJoint = new LLJoint(joints.Where(x => x.name == "LHRingJoint").First());
        LHMiddleTip = new LLJoint(joints.Where(x => x.name == "LHMiddleTarget").First(), joints.Where(x => x.name == "LHMiddleTarget").First().GetComponent<ParentJoint>());
        LHMiddleJoint = new LLJoint(joints.Where(x => x.name == "LHMiddleJoint").First());
        LHIndexTip = new LLJoint(joints.Where(x => x.name == "LHIndexTarget").First(), joints.Where(x => x.name == "LHIndexTarget").First().GetComponent<ParentJoint>());
        LHIndexJoint = new LLJoint(joints.Where(x => x.name == "LHIndexJoint").First());
        LHThumbTip = new LLJoint(joints.Where(x => x.name == "LHThumbTarget").First(), joints.Where(x => x.name == "LHThumbTarget").First().GetComponent<ParentJoint>());
        LHThumbJoint = new LLJoint(joints.Where(x => x.name == "LHThumbJoint").First());

        RHPinkyTip = new LLJoint(joints.Where(x => x.name == "RHPinkyTarget").First(), joints.Where(x => x.name == "RHPinkyTarget").First().GetComponent<ParentJoint>());
        RHPinkyJoint = new LLJoint(joints.Where(x => x.name == "RHPinkyJoint").First());
        RHRingTip = new LLJoint(joints.Where(x => x.name == "RHRingTarget").First(), joints.Where(x => x.name == "RHRingTarget").First().GetComponent<ParentJoint>());
        RHRingJoint = new LLJoint(joints.Where(x => x.name == "RHRingJoint").First());
        RHMiddleTip = new LLJoint(joints.Where(x => x.name == "RHMiddleTarget").First(), joints.Where(x => x.name == "RHMiddleTarget").First().GetComponent<ParentJoint>());
        RHMiddleJoint = new LLJoint(joints.Where(x => x.name == "RHMiddleJoint").First());
        RHIndexTip = new LLJoint(joints.Where(x => x.name == "RHIndexTarget").First(), joints.Where(x => x.name == "RHIndexTarget").First().GetComponent<ParentJoint>());
        RHIndexJoint = new LLJoint(joints.Where(x => x.name == "RHIndexJoint").First());
        RHThumbTip = new LLJoint(joints.Where(x => x.name == "RHThumbTarget").First(), joints.Where(x => x.name == "RHThumbTarget").First().GetComponent<ParentJoint>());
        RHThumbJoint = new LLJoint(joints.Where(x => x.name == "RHThumbJoint").First());
    }

    /// <summary>
    /// Sets up the laser animation step.
    /// </summary>
    private void SetupLaserAnimation()
    {
        laserAnimationLPart0 = new List<Step>()
        {
            new Step(LeftHand, new List<StepPosition>()
            {
                new StepPosition(new Vector3(0, 0.2f, 0.2f), 350.0f * LaserFirstStepSpeed)
            }, Step.DirectionType.position, Step.StepType.addition),
            new Step(LeftHand, new List<StepPosition>()
            {
                new StepPosition(new Vector3(330, 30, 285), 0.1f * LaserFirstStepRotationSpeed)
            }, Step.DirectionType.rotation, Step.StepType.position),
        };

        laserAnimationLPart1 = new List<Step>()
        {
            new Step(LeftHand, LeftLaserTarget, Step.DirectionType.position, Step.FollowType.xyz, 3.0f * LaserHandFollowSpeed, 6.0f),
            new Step(LeftHand, LeftLaserTarget, Step.DirectionType.matchRotation, Step.FollowType.y, 0.3f * LaserHandFollowSpeed, 6.0f),
            new Step(LHPinkyTip, LeftFingerLaserTarget, Step.DirectionType.rotation, Step.FollowType.xyz, 2.0f * LaserHandFollowSpeed, 6.0f),
            new Step(LHRingTip, LeftFingerLaserTarget, Step.DirectionType.rotation, Step.FollowType.xyz, 2.0f * LaserHandFollowSpeed, 6.0f),
            new Step(LHMiddleTip, LeftFingerLaserTarget, Step.DirectionType.rotation, Step.FollowType.xyz, 2.0f * LaserHandFollowSpeed, 6.0f),
            new Step(LHIndexTip, LeftFingerLaserTarget, Step.DirectionType.rotation, Step.FollowType.xyz, 2.0f * LaserHandFollowSpeed, 6.0f)
        };

        laserAnimationLPart2 = new List<Step>()
        {
            new Step(LeftHand, new List<StepPosition>()
            {
                new StepPosition(LeftHand.startingLocalRotation, 0.1f * LaserSecondStepRotationSpeed)
            }, Step.DirectionType.rotation, Step.StepType.position),
            new Step(LeftHand, new List<StepPosition>()
            {
                new StepPosition(LeftHand.startingLocalPosition, 350.0f * LaserSecondStepSpeed)
            }, Step.DirectionType.position, Step.StepType.position),
            new Step(LHPinkyTip, new List<StepPosition>()
            {
                new StepPosition(LHPinkyTip.startingLocalRotation, 0.01f * LaserSecondStepRotationSpeed)
            }, Step.DirectionType.rotation, Step.StepType.position),
            new Step(LHRingTip, new List<StepPosition>()
            {
                new StepPosition(LHRingTip.startingLocalRotation, 0.01f * LaserSecondStepRotationSpeed)
            }, Step.DirectionType.rotation, Step.StepType.position),
            new Step(LHMiddleTip, new List<StepPosition>()
            {
                new StepPosition(LHMiddleTip.startingLocalRotation, 0.01f * LaserSecondStepRotationSpeed)
            }, Step.DirectionType.rotation, Step.StepType.position),
            new Step(LHIndexTip, new List<StepPosition>()
            {
                new StepPosition(LHIndexTip.startingLocalRotation, 0.01f * LaserSecondStepRotationSpeed)
            }, Step.DirectionType.rotation, Step.StepType.position),
        };

        laserAnimationRPart0 = new List<Step>()
        {
            new Step(RightHand, new List<StepPosition>()
            {
                new StepPosition(new Vector3(0, 0.2f, 0.2f), 350.0f * LaserFirstStepSpeed)
            }, Step.DirectionType.position, Step.StepType.addition),
            new Step(RightHand, new List<StepPosition>()
            {
                new StepPosition(new Vector3(330, 30, 75), 0.1f * LaserFirstStepRotationSpeed)
            }, Step.DirectionType.rotation, Step.StepType.position),
        };

        laserAnimationRPart1 = new List<Step>()
        {
            new Step(RightHand, RightLaserTarget, Step.DirectionType.position, Step.FollowType.xyz, 3.0f * LaserHandFollowSpeed, 6.0f),
            new Step(RightHand, RightLaserTarget, Step.DirectionType.matchRotation, Step.FollowType.y, 0.3f * LaserHandFollowSpeed, 6.0f),
            new Step(RHPinkyTip, RightFingerLaserTarget, Step.DirectionType.rotation, Step.FollowType.xyz, 2.0f * LaserHandFollowSpeed, 6.0f),
            new Step(RHRingTip, RightFingerLaserTarget, Step.DirectionType.rotation, Step.FollowType.xyz, 2.0f * LaserHandFollowSpeed, 6.0f),
            new Step(RHMiddleTip, RightFingerLaserTarget, Step.DirectionType.rotation, Step.FollowType.xyz, 2.0f * LaserHandFollowSpeed, 6.0f),
            new Step(RHIndexTip, RightFingerLaserTarget, Step.DirectionType.rotation, Step.FollowType.xyz, 2.0f * LaserHandFollowSpeed, 6.0f),
        };

        laserAnimationRPart2 = new List<Step>()
        {
            new Step(RightHand, new List<StepPosition>()
            {
                new StepPosition(RightHand.startingLocalRotation, 0.1f * LaserSecondStepRotationSpeed)
            }, Step.DirectionType.rotation, Step.StepType.position),
            new Step(RightHand, new List<StepPosition>()
            {
                new StepPosition(RightHand.startingLocalPosition, 350.0f * LaserSecondStepSpeed)
            }, Step.DirectionType.position, Step.StepType.position),
            new Step(RHPinkyTip, new List<StepPosition>()
            {
                new StepPosition(RHPinkyTip.startingLocalRotation, 0.01f * LaserSecondStepRotationSpeed)
            }, Step.DirectionType.rotation, Step.StepType.position),
            new Step(RHRingTip, new List<StepPosition>()
            {
                new StepPosition(RHRingTip.startingLocalRotation, 0.01f * LaserSecondStepRotationSpeed)
            }, Step.DirectionType.rotation, Step.StepType.position),
            new Step(RHMiddleTip, new List<StepPosition>()
            {
                new StepPosition(RHMiddleTip.startingLocalRotation, 0.01f * LaserSecondStepRotationSpeed)
            }, Step.DirectionType.rotation, Step.StepType.position),
            new Step(RHIndexTip, new List<StepPosition>()
            {
                new StepPosition(RHIndexTip.startingLocalRotation, 0.01f * LaserSecondStepRotationSpeed)
            }, Step.DirectionType.rotation, Step.StepType.position),
        };
    }

    private void SetupDamLaserAnimation()
    {
        DamLaserAnimationPart0 = new List<Step>()
        {
            new Step(Chest, DamLaserTarget, Step.DirectionType.rotation, Step.FollowType.xyz, 3.0f, 1.0f),
        };

        DamLaserAnimationPart1 = new List<Step>()
        {
            new Step(Chest, DamLaserTarget, Step.DirectionType.rotation, Step.FollowType.xyz, 3.0f, 5.0f),
        };

        DamLaserAnimationPart2 = new List<Step>()
        {
            new Step(Chest, new List<StepPosition>()
            {
                new StepPosition(Chest.startingLocalRotation, 1.0f)
            }, Step.DirectionType.rotation, Step.StepType.position),
        };
    }

    ///// <summary>
    ///// Sets up the swipe animation steps.
    ///// </summary>
    //private void SetupSwipeAnimation()
    //{
    //    // Setup Left step 1
    //    swipeUpAnimationLPart0 = new List<Step>()
    //    {
    //        new Step(LeftHand, Player, Step.DirectionType.position, Step.FollowType.xyz, 10.0f, 0.5f),
    //        new Step(LeftElbow, new List<StepPosition>()
    //        {
    //            new StepPosition(new Vector3(0.0f, 0.2f, -0.4f), 0.4f * SwipeUpAnimSpeed)
    //        }, Step.DirectionType.position, Step.StepType.addition)
    //    };

    //    // Setup Left step 2
    //    swipeUpAnimationLPart1 = new List<Step>()
    //    {
    //        new Step(LeftHand, new List<StepPosition>()
    //        {
    //            new StepPosition(LeftHand.startingLocalPosition, 7.0f * SwipeUpAnimSpeed)
    //        }, Step.DirectionType.position, Step.StepType.position),
    //        new Step(LeftElbow, new List<StepPosition>()
    //        {
    //            new StepPosition(LeftElbow.startingLocalPosition, 7.0f * SwipeUpAnimSpeed)
    //        }, Step.DirectionType.position, Step.StepType.position),
    //    };

    //    // Setup Right step 1
    //    swipeUpAnimationRPart0 = new List<Step>()
    //    {
    //        new Step(RightHand, Player, Step.DirectionType.position, Step.FollowType.xyz, 10.0f, 0.5f),
    //        new Step(RightElbow, new List<StepPosition>()
    //        {
    //            new StepPosition(new Vector3(0.0f, 0.2f, -0.4f), 0.4f * SwipeUpAnimSpeed)
    //        }, Step.DirectionType.position, Step.StepType.addition)
    //    };

    //    // Setup Right step 2
    //    swipeUpAnimationRPart1 = new List<Step>()
    //    {
    //        new Step(RightHand, new List<StepPosition>()
    //        {
    //            new StepPosition(RightHand.startingLocalPosition, 7.0f * SwipeUpAnimSpeed)
    //        }, Step.DirectionType.position, Step.StepType.position),
    //        new Step(RightElbow, new List<StepPosition>()
    //        {
    //            new StepPosition(RightElbow.startingLocalPosition, 7.0f * SwipeUpAnimSpeed)
    //        }, Step.DirectionType.position, Step.StepType.position)
    //    };
    //}

    /// <summary>
    /// Sets up the swing animation steps.
    /// </summary>
    private void SetupSwingAnimation()
    {
        // Setup Left step 1
        swingAnimationLPart0 = new List<Step>()
        {
            new Step(LeftHand, new List<StepPosition>()
            {
                new StepPosition(new Vector3(-0.05f, 0.05f, -0.05f), 20f * PunchAnimationSpeed),
                new StepPosition(new Vector3(-0.05f, 0.05f, -0.1f), 20f * PunchAnimationSpeed),
                new StepPosition(new Vector3(-0.05f, 0.05f, -0.1f), 20f * PunchAnimationSpeed),
                new StepPosition(new Vector3(-0.05f, 0.05f, -0.1f), 20f * PunchAnimationSpeed),
                new StepPosition(new Vector3(0.05f, 0.05f, -0.05f), 20f * PunchAnimationSpeed),
                new StepPosition(new Vector3(0.05f, 0.05f, -0.05f), 20f * PunchAnimationSpeed),
            }, Step.DirectionType.position, Step.StepType.addition),
            new Step(LeftElbow, new List<StepPosition>()
            {
                new StepPosition(new Vector3(0.0f, 0.0f, -0.4f), 20f * PunchAnimationSpeed),
                new StepPosition(new Vector3(0.4f, 0.0f, -0.4f), 20f * PunchAnimationSpeed)
            }, Step.DirectionType.position, Step.StepType.addition),
            // new Step(Chest, new List<StepPosition>()
            // {
            //     new StepPosition(new Vector3(0.0f, -0.3f, 0.0f), 20f * PunchAnimationSpeed)
            // }, Step.DirectionType.position, Step.StepType.addition),
            // new Step(Spine, new List<StepPosition>()
            // {
            //     new StepPosition(new Vector3(0.0f, -0.3f, 0.0f), 20f * PunchAnimationSpeed)
            // }, Step.DirectionType.position, Step.StepType.addition),
            // new Step(RightHand, new List<StepPosition>()
            // {
            //     new StepPosition(new Vector3(0.0f, -0.3f, 0.0f), 20f * PunchAnimationSpeed)
            // }, Step.DirectionType.position, Step.StepType.addition),
            // new Step(RightElbow, new List<StepPosition>()
            // {
            //     new StepPosition(new Vector3(0.0f, -0.3f, 0.0f), 20f * PunchAnimationSpeed)
            // }, Step.DirectionType.position, Step.StepType.addition),
        };

        // Setup Left step 2
        swingAnimationLPart1 = new List<Step>()
        {
            new Step(LeftHand, PunchTargetL, Step.DirectionType.position, Step.FollowType.xyz, 10.0f * PunchReleaseAnimationSpeed, 1.0f),
            new Step(LeftHand, PunchTargetL, Step.DirectionType.rotation, Step.FollowType.xyz, 10.0f * PunchReleaseAnimationSpeed, 1.0f),
        };

        // Setup Left step 3
        swingAnimationLPart2 = new List<Step>()
        {
            new Step(LeftHand, new List<StepPosition>()
            {
                new StepPosition(LeftHand.startingLocalPosition, 350.0f * PunchAnimationSpeed)
            }, Step.DirectionType.position, Step.StepType.position),
            new Step(LeftHand, new List<StepPosition>()
            {
                new StepPosition(LeftHand.startingLocalRotation, 0.01f * PunchAnimationRotationSpeed)
            }, Step.DirectionType.rotation, Step.StepType.position),
            new Step(LeftElbow, new List<StepPosition>()
            {
                new StepPosition(LeftElbow.startingLocalPosition, 350.0f * PunchAnimationSpeed)
            }, Step.DirectionType.position, Step.StepType.position),
            // new Step(Chest, new List<StepPosition>()
            // {
            //     new StepPosition(Chest.startingLocalPosition, 350.0f * PunchAnimationSpeed)
            // }, Step.DirectionType.position, Step.StepType.position),
            // new Step(Spine, new List<StepPosition>()
            // {
            //     new StepPosition(Spine.startingLocalPosition, 350.0f * PunchAnimationSpeed)
            // }, Step.DirectionType.position, Step.StepType.position),
            // new Step(RightHand, new List<StepPosition>()
            // {
            //     new StepPosition(RightHand.startingLocalPosition, 350.0f * PunchAnimationSpeed)
            // }, Step.DirectionType.position, Step.StepType.position),
            // new Step(RightElbow, new List<StepPosition>()
            // {
            //     new StepPosition(RightElbow.startingLocalPosition, 350.0f * PunchAnimationSpeed)
            // }, Step.DirectionType.position, Step.StepType.position),
        };

        // Setup Right step 1
        swingAnimationRPart0 = new List<Step>()
        {
            new Step(RightHand, new List<StepPosition>()
            {
                new StepPosition(new Vector3(0.05f, 0.05f, -0.05f), 20f * PunchAnimationSpeed),
                new StepPosition(new Vector3(0.05f, 0.05f, -0.1f), 20f * PunchAnimationSpeed),
                new StepPosition(new Vector3(0.05f, 0.05f, -0.1f), 20f * PunchAnimationSpeed),
                new StepPosition(new Vector3(0.05f, 0.05f, -0.1f), 20f * PunchAnimationSpeed),
                new StepPosition(new Vector3(-0.05f, 0.05f, -0.05f), 20f * PunchAnimationSpeed),
                new StepPosition(new Vector3(-0.05f, 0.05f, -0.05f), 20f * PunchAnimationSpeed),
            }, Step.DirectionType.position, Step.StepType.addition),
            new Step(RightElbow, new List<StepPosition>()
            {
                new StepPosition(new Vector3(0.0f, 0.0f, -0.4f), 20f * PunchAnimationSpeed),
                new StepPosition(new Vector3(0.4f, 0.0f, -0.4f), 20f * PunchAnimationSpeed)
            }, Step.DirectionType.position, Step.StepType.addition),
            // new Step(Chest, new List<StepPosition>()
            // {
            //     new StepPosition(new Vector3(0.0f, -0.3f, 0.0f), 20f * PunchAnimationSpeed)
            // }, Step.DirectionType.position, Step.StepType.addition),
            // new Step(Spine, new List<StepPosition>()
            // {
            //     new StepPosition(new Vector3(0.0f, -0.3f, 0.0f), 20f * PunchAnimationSpeed)
            // }, Step.DirectionType.position, Step.StepType.addition),
            // new Step(LeftHand, new List<StepPosition>()
            // {
            //     new StepPosition(new Vector3(0.0f, -0.3f, 0.0f), 20f * PunchAnimationSpeed)
            // }, Step.DirectionType.position, Step.StepType.addition),
            // new Step(LeftElbow, new List<StepPosition>()
            // {
            //     new StepPosition(new Vector3(0.0f, -0.3f, 0.0f), 20f * PunchAnimationSpeed)
            // }, Step.DirectionType.position, Step.StepType.addition),
        };

        // Setup Right step 2
        swingAnimationRPart1 = new List<Step>()
        {
            new Step(RightHand, PunchTargetR, Step.DirectionType.position, Step.FollowType.xyz, 10.0f * PunchReleaseAnimationSpeed, 1.0f),
            new Step(RightHand, PunchTargetR, Step.DirectionType.rotation, Step.FollowType.xyz, 10.0f * PunchReleaseAnimationSpeed, 1.0f),
        };

        // Setup Right step 3
        swingAnimationRPart2 = new List<Step>()
        {
            new Step(RightHand, new List<StepPosition>()
            {
                new StepPosition(RightHand.startingLocalPosition, 350.0f * PunchAnimationSpeed)
            }, Step.DirectionType.position, Step.StepType.position),
            new Step(RightHand, new List<StepPosition>()
            {
                new StepPosition(RightHand.startingLocalRotation, 0.01f * PunchAnimationRotationSpeed)
            }, Step.DirectionType.rotation, Step.StepType.position),
            new Step(RightElbow, new List<StepPosition>()
            {
                new StepPosition(RightElbow.startingLocalPosition, 350.0f * PunchAnimationSpeed)
            }, Step.DirectionType.position, Step.StepType.position),
            // new Step(Chest, new List<StepPosition>()
            // {
            //     new StepPosition(Chest.startingLocalPosition, 350.0f * PunchAnimationSpeed)
            // }, Step.DirectionType.position, Step.StepType.position),
            // new Step(Spine, new List<StepPosition>()
            // {
            //     new StepPosition(Spine.startingLocalPosition, 350.0f * PunchAnimationSpeed)
            // }, Step.DirectionType.position, Step.StepType.position),
            // new Step(LeftHand, new List<StepPosition>()
            // {
            //     new StepPosition(LeftHand.startingLocalPosition, 350.0f * PunchAnimationSpeed)
            // }, Step.DirectionType.position, Step.StepType.position),
            // new Step(LeftElbow, new List<StepPosition>()
            // {
            //     new StepPosition(LeftElbow.startingLocalPosition, 350.0f * PunchAnimationSpeed)
            // }, Step.DirectionType.position, Step.StepType.position),
        };
    }

    /// <summary>
    /// Sets up the first animator with the joint variables.
    /// </summary>
    private void SetupFist()
    {
        fistAnimation = new ClenchFistAnimation(new List<LLJoint>()
        {
            LHPinkyTip,
            LHPinkyJoint,
            LHRingTip,
            LHRingJoint,
            LHMiddleTip,
            LHMiddleJoint,
            LHIndexTip,
            LHIndexJoint,
            LHThumbTip,
            LHThumbJoint,
            RHPinkyTip,
            RHPinkyJoint,
            RHRingTip,
            RHRingJoint,
            RHMiddleTip,
            RHMiddleJoint,
            RHIndexTip,
            RHIndexJoint,
            RHThumbTip,
            RHThumbJoint
        });

        // Debug
        fistAnimation.Speed = 10.0f;
        fistAnimation.MoveHands = ClenchFistAnimation.Hand.both;
    }

    /// <summary>
    /// Performs initial setup for clap animation.
    /// </summary>
    private void SetupClapAnimation()
    {
        // setup step 1
        clapAnimationPart0 = new List<Step>()
        {
            //new Step(Waist, new List<StepPosition>()
            //{
            //    new StepPosition(new Vector3(0.0f, -0.6f, 0.0f), 2.0f * ClapAnimSpeed)
            //}, Step.DirectionType.position, Step.StepType.addition),
            //new Step(Spine, new List<StepPosition>()
            //{
            //    new StepPosition(new Vector3(0.0f, -0.4f, -0.2f), 2.0f * ClapAnimSpeed)
            //}, Step.DirectionType.position, Step.StepType.addition),
            //new Step(Chest, new List<StepPosition>()
            //{
            //    new StepPosition(new Vector3(0.0f, -0.6f, 0.15f), 7.0f * ClapAnimSpeed)
            //}, Step.DirectionType.position, Step.StepType.addition),
            //new Step(Chest, new List<StepPosition>()
            //{
            //    new StepPosition(new Vector3(30.0f, 0.0f, 0.0f), 1.0f)
            //}, Step.DirectionType.rotation, Step.StepType.addition),
            //new Step(LeftElbow, new List<StepPosition>()
            //{
            //    new StepPosition(new Vector3(0.0f, 0.2f, 0.2f), 7.0f * ClapAnimSpeed)
            //}, Step.DirectionType.position, Step.StepType.addition),
            new Step(LeftHand, new List<StepPosition>()
            {
                new StepPosition(new Vector3(0.0f, 0.2f, 0.2f), 350.0f * ClapAnimationSpeed)
            }, Step.DirectionType.position, Step.StepType.addition),
            new Step(LeftHand, new List<StepPosition>()
            {
                new StepPosition(new Vector3(270.0f, 0.0f, 0.0f), 1.0f * ClapAnimationRotationSpeed)
            }, Step.DirectionType.rotation, Step.StepType.addition),
            //new Step(RightElbow, new List<StepPosition>()
            //{
            //    new StepPosition(new Vector3(0.0f, 0.0f, 0.2f), 7.0f * ClapAnimSpeed)
            //}, Step.DirectionType.position, Step.StepType.addition),
            new Step(RightHand, new List<StepPosition>()
            {
                new StepPosition(new Vector3(0.0f, 0.2f, 0.2f), 350.0f * ClapAnimationSpeed)
            }, Step.DirectionType.position, Step.StepType.addition),
            new Step(RightHand, new List<StepPosition>()
            {
                new StepPosition(new Vector3(270.0f, -0.0f, 0.0f), 1.0f * ClapAnimationRotationSpeed)
            }, Step.DirectionType.rotation, Step.StepType.addition),
            //new Step(LeftKnee, new List<StepPosition>()
            //{
            //    new StepPosition(new Vector3(0.0f, 0.0f, 0.2f), 7.0f * ClapAnimSpeed)
            //}, Step.DirectionType.position, Step.StepType.addition),
            //new Step(RightKnee, new List<StepPosition>()
            //{
            //    new StepPosition(new Vector3(0.0f, 0.0f, 0.2f), 7.0f * ClapAnimSpeed)
            //}, Step.DirectionType.position, Step.StepType.addition)
        };

        // setup step 2
        clapAnimationPart1 = new List<Step>()
        {
            new Step(LeftHand, LeftHandClapTarget, Step.DirectionType.position, Step.FollowType.xyz, 5.0f, 1.2f),
            new Step(RightHand, RightHandClapTarget, Step.DirectionType.position, Step.FollowType.xyz, 5.0f, 1.2f),
            new Step(Chest, Player, Step.DirectionType.rotation, Step.FollowType.y, 5.0f * ClapAnimationRotationSpeed, 1.2f)
        };

        // setup step 3
        clapAnimationPart2 = new List<Step>()
        {
            new Step(LeftHand, clapTarget, Step.DirectionType.position, Step.FollowType.xyz, 10.0f, 0.25f),
            new Step(RightHand, clapTarget, Step.DirectionType.position, Step.FollowType.xyz, 10.0f,  0.25f),
            new Step(Chest, Player, Step.DirectionType.rotation, Step.FollowType.y, 5.0f * ClapAnimationRotationSpeed, 0.25f)
        };

        // setup step 4
        clapAnimationPart3 = new List<Step>()
        {
            new Step(LeftElbow, new List<StepPosition>()
            {
                new StepPosition(LeftElbow.startingLocalPosition, 350.0f * ClapAnimationSpeed)
            }, Step.DirectionType.position, Step.StepType.position),
            new Step(LeftHand, new List<StepPosition>()
            {
                new StepPosition(LeftHand.startingLocalPosition, 350.0f * ClapAnimationSpeed)
            }, Step.DirectionType.position, Step.StepType.position),
            new Step(LeftHand, new List<StepPosition>()
            {
                new StepPosition(LeftHand.startingLocalRotation, 1.0f * ClapAnimationRotationSpeed)
            }, Step.DirectionType.rotation, Step.StepType.position),
            new Step(RightElbow, new List<StepPosition>()
            {
                new StepPosition(RightElbow.startingLocalPosition, 350.0f * ClapAnimationSpeed)
            }, Step.DirectionType.position, Step.StepType.position),
            new Step(RightHand, new List<StepPosition>()
            {
                new StepPosition(RightHand.startingLocalPosition, 350.0f * ClapAnimationSpeed)
            }, Step.DirectionType.position, Step.StepType.position),
            new Step(RightHand, new List<StepPosition>()
            {
                new StepPosition(RightHand.startingLocalRotation, 1.0f * ClapAnimationRotationSpeed)
            }, Step.DirectionType.rotation, Step.StepType.position),
            new Step(Spine, new List<StepPosition>()
            {
                new StepPosition(Spine.startingLocalPosition, 350.0f * ClapAnimationSpeed)
            }, Step.DirectionType.position, Step.StepType.position),
            new Step(Chest, new List<StepPosition>()
            {
                new StepPosition(Chest.startingLocalPosition, 2.0f * ClapAnimationSpeed)
            }, Step.DirectionType.position, Step.StepType.position),
            new Step(Chest, new List<StepPosition>()
            {
                new StepPosition(Chest.startingLocalRotation, 1.0f * ClapAnimationRotationSpeed)
            }, Step.DirectionType.rotation, Step.StepType.position),
            new Step(Waist, new List<StepPosition>()
            {
                new StepPosition(Waist.startingLocalPosition, 350.0f * ClapAnimationSpeed)
            }, Step.DirectionType.position, Step.StepType.position),
            new Step(LeftKnee, new List<StepPosition>()
            {
                new StepPosition(LeftKnee.startingLocalPosition, 350.0f * ClapAnimationSpeed)
            }, Step.DirectionType.position, Step.StepType.position),
            new Step(RightKnee, new List<StepPosition>()
            {
                new StepPosition(RightKnee.startingLocalPosition, 350.0f * ClapAnimationSpeed)
            }, Step.DirectionType.position, Step.StepType.position),
        };
    }

    /// <summary>
    /// Performs initial setup for dam punch animation.
    /// </summary>
    private void SetupDamPunchAnimation()
    {
        // setup step 1
        damPunchAnimationPart0 = new List<Step>()
        {
            // LeftHand
            new Step(LeftElbow, new List<StepPosition>()
            {
                new StepPosition(new Vector3(-0.2f, 0.8f, 0.1f+0.2f), 350.0f * DestructionAnimationSpeed)
            }, Step.DirectionType.position, Step.StepType.position),
            new Step(LeftHand, new List<StepPosition>()
            {
                new StepPosition(new Vector3(-0.2f, 0.8f, 0.4f), 400.0f * DestructionAnimationSpeed)
            }, Step.DirectionType.position, Step.StepType.position),
            new Step(LeftHand, new List<StepPosition>()
            {
                new StepPosition(new Vector3(0.0f, 30.0f, 90.0f), 1.0f * DestructionAnimationRotationSpeed)
            }, Step.DirectionType.rotation, Step.StepType.position),            
            // RightHand
            new Step(RightElbow, new List<StepPosition>()
            {
                new StepPosition(new Vector3(0.7f, 0.7f, 0.3f), 500.0f * DestructionAnimationSpeed)
            }, Step.DirectionType.position, Step.StepType.position),
            new Step(RightHand, new List<StepPosition>()
            {
                new StepPosition(new Vector3(0.1f, 0.5f, 0.4f), 400.0f * DestructionAnimationSpeed)
            }, Step.DirectionType.position, Step.StepType.position),
            new Step(RightHand, new List<StepPosition>()
            {
                new StepPosition(new Vector3(0.0f, 280.0f, 40.0f), 1.0f * DestructionAnimationSpeed)
            }, Step.DirectionType.rotation, Step.StepType.position),
            // LeftLeg
            new Step(LeftKnee, new List<StepPosition>()
            {
                new StepPosition(new Vector3(-0.1f, 0.5f, 0.5f), 500.0f * DestructionAnimationSpeed)
            }, Step.DirectionType.position, Step.StepType.position),
            // RightLeg  
            new Step(RightKnee, new List<StepPosition>()
            {
                new StepPosition(new Vector3(0.1f, 0.5f, 1.0f - 0.2f), 300.0f * DestructionAnimationSpeed)
            }, Step.DirectionType.position, Step.StepType.position),
            new Step(RightLeg, new List<StepPosition>()
            {
                new StepPosition(new Vector3(0.1f, 0.2f, 0.2f- 0.2f), 100.0f * DestructionAnimationSpeed),
                new StepPosition(new Vector3(0.1f, 0.15f, 0.6f- 0.2f), 50.0f * DestructionAnimationSpeed),
                new StepPosition(new Vector3(0.1f, 0.1f, 0.8f- 0.2f), 50.0f * DestructionAnimationSpeed)
            }, Step.DirectionType.position, Step.StepType.position),
            new Step(RightLeg, new List<StepPosition>()
            {
                new StepPosition(new Vector3(-0.0f, 0.0f, 90.0f), 1.0f * DestructionAnimationRotationSpeed)
            }, Step.DirectionType.rotation, Step.StepType.position),
            // Spine
            new Step(Spine, new List<StepPosition>()
            {
                new StepPosition(new Vector3(0.0f, 0.5f, 0.4f), 500.0f * DestructionAnimationSpeed)
            }, Step.DirectionType.position, Step.StepType.position),
            new Step(Waist, new List<StepPosition>()
            {
                new StepPosition(new Vector3(0.0f, 1.0f, 0.5f), 500.0f * DestructionAnimationSpeed)
            }, Step.DirectionType.position, Step.StepType.position),
            // Chest
            new Step(Chest, new List<StepPosition>()
            {
                new StepPosition(new Vector3(0.0f, 1.3f, 0.4f), 500.0f * DestructionAnimationSpeed)
            }, Step.DirectionType.position, Step.StepType.position),
            new Step(Chest, new List<StepPosition>()
            {
                new StepPosition(new Vector3(0.0f, 330.0f, 0.0f), 2.0f * DestructionAnimationRotationSpeed)
            }, Step.DirectionType.rotation, Step.StepType.position)
        };

        // setup step 2
        damPunchAnimationPart1 = new List<Step>()
        {
            // LeftHand
            new Step(LeftElbow, new List<StepPosition>()
            {
                new StepPosition(new Vector3(-0.6f, 1.1f, -0.1f), 1.0f * DestructionAnimationSpeed)
            }, Step.DirectionType.position, Step.StepType.position),
            new Step(LeftHand, new List<StepPosition>()
            {
                new StepPosition(new Vector3(-0.4f, 0.8f, 0.5f), 500.0f * DestructionAnimationSpeed)
            }, Step.DirectionType.position, Step.StepType.position),
            new Step(LeftHand, new List<StepPosition>()
            {
                new StepPosition(new Vector3(0.0f, 315.0f, 270.0f), 1.0f * DestructionAnimationRotationSpeed)
            }, Step.DirectionType.rotation, Step.StepType.position),
            // RightHand
            new Step(RightElbow, new List<StepPosition>()
            {
                new StepPosition(new Vector3(0.2f, 0.7f, 0.0f), 500.0f * DestructionAnimationSpeed)
            }, Step.DirectionType.position, Step.StepType.position),
            new Step(RightHand, new List<StepPosition>()
            {
                new StepPosition(new Vector3(0.3f, 0.7f, 0.3f), 500.0f * DestructionAnimationSpeed)
            }, Step.DirectionType.position, Step.StepType.position),
            new Step(RightHand, new List<StepPosition>()
            {
                new StepPosition(new Vector3(0.0f, 50.0f, 270.0f), 1.0f * DestructionAnimationRotationSpeed)
            }, Step.DirectionType.rotation, Step.StepType.position),
            // Chest
            new Step(Chest, new List<StepPosition>()
            {
                new StepPosition(new Vector3(0.0f, 30.0f, 0.0f), 2.0f * DestructionAnimationRotationSpeed)
            }, Step.DirectionType.rotation, Step.StepType.position)
        };

    }

    private void SetupFinalLaserAnimation()
    {
        finalLaserAnimationPart0 = new List<Step>()
        {
            new Step(Chest, ChestLaserTarget, Step.DirectionType.rotation, Step.FollowType.xyz, 5.0f * ChestLaserFollowSpeed, 5.0f, new Vector3(0, -83.72638f, 0)),
            new Step(Chest, ChestPiece, Step.DirectionType.position, Step.FollowType.xyz, 1000.0f, 1.0f)
        };

        finalLaserAnimationPart1 = new List<Step>()
        {
            new Step(Chest, new List<StepPosition>()
            {
                new StepPosition(Chest.startingLocalRotation, 1.0f)
            }, Step.DirectionType.rotation, Step.StepType.position),
        };
    }

    /// <summary>
    /// Called after initialised.
    /// </summary>
    private void Awake()
    {
        gameManager = GameObject.FindObjectOfType<GameManager>();
        //EnableKinematics();
    }

    /// <summary>
    /// Called every frame.
    /// </summary>
    private void Update()
    {

    }

    /// <summary>
    /// Updated after update set.
    /// </summary>
    private void LateUpdate()
    {
        // pause
        if (gameManager.gamePaused)
        {

        }
        else
        {
            if (!isComplete)
            {
                switch (currentAnimation)
                {
                    case Animation.idle:
                        break;
                    case Animation.GiantClap:
                        GiantClap();
                        break;
                    case Animation.GiantDamPunch:
                        GiantDamPunch();
                        break;
                    //case Animation.GiantSwipeUp:
                    //    GiantSwipeUp();
                    //    break;
                    case Animation.GiantSwing:
                        GiantSwing();
                        break;
                    case Animation.Laser:
                        Laser();
                        break;
                    case Animation.FinalLaser:
                        FinalLaser();
                        break;
                    case Animation.DamLaser:
                        DamLaser();
                        break;
                }
            }
        }
    }

    private void DamLaser()
    {
        switch (currentDamLaserState)
        {
            case DamLaserAnimationState.idle:
                currentDamLaserState = DamLaserAnimationState.aim;
                break;
            case DamLaserAnimationState.aim:
                if (PerformAnimationStep(DamLaserAnimationPart0))
                {
                    currentDamLaserState = DamLaserAnimationState.shoot;
                }
                break;
            case DamLaserAnimationState.shoot:
                if (PerformAnimationStep(DamLaserAnimationPart1))
                {
                    currentDamLaserState = DamLaserAnimationState.recover;
                }
                break;
            case DamLaserAnimationState.recover:
                if (PerformAnimationStep(DamLaserAnimationPart2))
                {
                    currentDamLaserState = DamLaserAnimationState.idle;
                    isComplete = true;
                    DisableKinematics();
                }
                break;
        }
    }

    private void FinalLaser()
    {
        switch (currentFinalLaserState)
        {
            case FinalLaserAnimationState.idle:
                currentFinalLaserState = FinalLaserAnimationState.shoot;
                break;
            case FinalLaserAnimationState.shoot:
                if (PerformAnimationStep(finalLaserAnimationPart0))
                {
                    currentFinalLaserState = FinalLaserAnimationState.recover;
                }
                break;
            case FinalLaserAnimationState.recover:
                if (PerformAnimationStep(finalLaserAnimationPart1))
                {
                    currentFinalLaserState = FinalLaserAnimationState.idle;
                    isComplete = true;
                    DisableKinematics();
                }
                break;
        }
    }

    /// <summary>
    /// Peforms the animation for the laser attack.
    /// </summary>
    private void Laser()
    {
        switch (currentLaserState)
        {
            case LaserAnimationState.idle:
                currentLaserState = LaserAnimationState.aim;
                break;
            case LaserAnimationState.aim:
                switch (hand)
                {

                    case Hand.left:
                        if (PerformAnimationStep(laserAnimationLPart0))
                        {
                            // Increment the animation step.
                            currentLaserState = LaserAnimationState.shoot;
                            giantBehaviour.LaserTimer = 0;
                            gsm.SetStage(0);
                        }
                        break;
                    case Hand.right:
                        if (PerformAnimationStep(laserAnimationRPart0))
                        {
                            // Increment the animationstep.
                            currentLaserState = LaserAnimationState.shoot;
                            giantBehaviour.LaserTimer = 0;
                            gsm.SetStage(0);
                        }
                        break;
                }
                break;
            case LaserAnimationState.shoot:
                switch (hand)
                {
                    case Hand.left:
                        if (PerformAnimationStep(laserAnimationLPart1))
                        {
                            // Increment the Animation step.
                            gsm.SetStage(2);
                            currentLaserState = LaserAnimationState.recover;
                        }
                        break;
                    case Hand.right:
                        if (PerformAnimationStep(laserAnimationRPart1))
                        {
                            // Increment the Animation step.
                            gsm.SetStage(2);
                            currentLaserState = LaserAnimationState.recover;
                        }
                        break;
                }
                break;
            case LaserAnimationState.recover:
                switch (hand)
                {
                    case Hand.left:
                        if (PerformAnimationStep(laserAnimationLPart2))
                        {
                            // Increment the Animation step.    

                            currentLaserState = LaserAnimationState.idle;
                            isComplete = true;
                            DisableKinematics();
                        }
                        break;
                    case Hand.right:
                        if (PerformAnimationStep(laserAnimationRPart2))
                        {
                            // Increment the Animation step.

                            currentLaserState = LaserAnimationState.idle;
                            isComplete = true;
                            DisableKinematics();
                        }
                        break;
                }
                break;
        }
    }

    ///// <summary>
    ///// Performs the animation for the swipe attack.
    ///// </summary>
    //private void GiantSwipeUp()
    //{
    //    switch (CurrentSwipeUpState)
    //    {
    //        case SwipeUpAnimationState.idle:
    //            CurrentSwipeUpState = SwipeUpAnimationState.swipe;
    //            break;
    //        case SwipeUpAnimationState.swipe:
    //            switch (hand)
    //            {
    //                case Hand.left:
    //                    if (PerformAnimationStep(swipeUpAnimationLPart0))
    //                    {
    //                        // Increment the Animation step.    
    //                        CurrentSwipeUpState = SwipeUpAnimationState.recover;
    //                    }
    //                    break;
    //                case Hand.right:
    //                    if (PerformAnimationStep(swipeUpAnimationRPart0))
    //                    {
    //                        // Increment the Animation step.    
    //                        CurrentSwipeUpState = SwipeUpAnimationState.recover;
    //                    }
    //                    break;
    //            }
    //            break;
    //        case SwipeUpAnimationState.recover:
    //            // reset
    //            switch (hand)
    //            {
    //                case Hand.left:
    //                    if (PerformAnimationStep(swipeUpAnimationLPart1))
    //                    {
    //                        // Increment the Animation step.    
    //                        CurrentSwipeUpState = SwipeUpAnimationState.idle;
    //                        isComplete = true;
    //                        DisableKinematics();
    //                    }
    //                    break;
    //                case Hand.right:
    //                    if (PerformAnimationStep(swipeUpAnimationRPart1))
    //                    {
    //                        // Increment the Animation step.    
    //                        CurrentSwipeUpState = SwipeUpAnimationState.idle;
    //                        isComplete = true;
    //                        DisableKinematics();
    //                    }
    //                    break;
    //            }
    //            break;
    //    }
    //}

    /// <summary>
    /// Performs the animation for the swing attack.
    /// </summary>
    private void GiantSwing()
    {
        switch (CurrentSwingState)
        {
            case SwingAnimationState.idle:
                CurrentSwingState = SwingAnimationState.DrawBack;
                break;
            case SwingAnimationState.DrawBack:
                switch (hand)
                {
                    case Hand.left:
                        if (PerformAnimationStep(swingAnimationLPart0))
                        {
                            // Increment the Animation step.    
                            CurrentSwingState = SwingAnimationState.Release;
                        }
                        break;
                    case Hand.right:
                        if (PerformAnimationStep(swingAnimationRPart0))
                        {
                            // Increment the Animation step.    
                            CurrentSwingState = SwingAnimationState.Release;
                        }
                        break;
                }
                break;
            case SwingAnimationState.Release:
                switch (hand)
                {
                    case Hand.left:
                        if (PerformAnimationStep(swingAnimationLPart1))
                        {
                            // Increment the Animation step.    
                            CurrentSwingState = SwingAnimationState.Recover;
                        }
                        break;
                    case Hand.right:
                        if (PerformAnimationStep(swingAnimationRPart1))
                        {
                            // Increment the Animation step.    
                            CurrentSwingState = SwingAnimationState.Recover;
                        }
                        break;
                }
                break;
            case SwingAnimationState.Recover:
                // reset
                switch (hand)
                {
                    case Hand.left:
                        if (PerformAnimationStep(swingAnimationLPart2))
                        {
                            // Increment the Animation step.    
                            CurrentSwingState = SwingAnimationState.idle;
                            isComplete = true;
                            DisableKinematics();
                        }
                        break;
                    case Hand.right:
                        if (PerformAnimationStep(swingAnimationRPart2))
                        {
                            // Increment the Animation step.    
                            CurrentSwingState = SwingAnimationState.idle;
                            isComplete = true;
                            DisableKinematics();
                        }
                        break;
                }
                break;
        }
    }

    /// <summary>
    /// Performs the animation for the clap.
    /// </summary>
    private void GiantClap()
    {
        // Perform the giant step animation.
        switch (currentClapAnimationState)
        {
            case ClapAnimationState.idle:
                currentClapAnimationState = ClapAnimationState.Bend;
                break;
            case ClapAnimationState.Bend:
                if (PerformAnimationStep(clapAnimationPart0))
                {
                    // Increment the Animation step.    
                    currentClapAnimationState = ClapAnimationState.DrawBack;
                }
                break;
            case ClapAnimationState.DrawBack:
                if (PerformAnimationStep(clapAnimationPart1))
                {
                    // Increment the Animation step.
                    currentClapAnimationState = ClapAnimationState.Release;
                }
                break;
            case ClapAnimationState.Release:
                if (PerformAnimationStep(clapAnimationPart2))
                {
                    // Increment the Animation step.
                    currentClapAnimationState = ClapAnimationState.Recover;
                }
                break;
            case ClapAnimationState.Recover:
                // Reset
                if (PerformAnimationStep(clapAnimationPart3))
                {
                    // Increment the Animation step.
                    currentClapAnimationState = ClapAnimationState.idle;
                    isComplete = true;
                    DisableKinematics();
                }
                break;
        }
    }

    /// <summary>
    /// Performs the animation for the punch.
    /// </summary>
    private void GiantDamPunch()
    {

        fistAnimation.PerformAnimation();

        // Perform the giant step animation.
        switch (currentPunchState)
        {
            case PunchAnimationState.idle:
                currentPunchState = PunchAnimationState.DrawBack;
                break;
            case PunchAnimationState.DrawBack:
                if (PerformAnimationStep(damPunchAnimationPart0))
                {
                    // Increment the Animation step.
                    currentPunchState = PunchAnimationState.Release;
                }
                break;
            case PunchAnimationState.Release:
                if (PerformAnimationStep(damPunchAnimationPart1))
                {
                    // Increment the Animation step.
                    currentPunchState = PunchAnimationState.Recover;
                }
                break;
            case PunchAnimationState.Recover:
                // Increment the Animation step.
                currentPunchState = PunchAnimationState.idle;
                isComplete = true;
                DisableKinematics();
                break;
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
            // Reset all the animations
            foreach (Step step in steps)
            {
                step.Reset();
            }
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

    /// <summary>
    /// Disable all the kinematics.
    /// </summary>
    public void DisableKinematics()
    {
        foreach (GameObject kinematic in LeftHandKinematics)
        {
            kinematic.GetComponent<InverseKinematics>().enabled = false;
        }
        foreach (GameObject kinematic in LeftArmKinematics)
        {
            kinematic.GetComponent<InverseKinematics>().enabled = false;
        }
        foreach (GameObject kinematic in LeftLegKinematics)
        {
            kinematic.GetComponent<InverseKinematics>().enabled = false;
        }
        foreach (GameObject kinematic in RightArmKinematics)
        {
            kinematic.GetComponent<InverseKinematics>().enabled = false;
        }
        foreach (GameObject kinematic in RightHandKinematics)
        {
            kinematic.GetComponent<InverseKinematics>().enabled = false;
        }
        foreach (GameObject kinematic in RightLegKinematics)
        {
            kinematic.GetComponent<InverseKinematics>().enabled = false;
        }
        foreach (GameObject kinematic in ChestKinematics)
        {
            kinematic.GetComponent<InverseKinematics>().enabled = false;
        }
    }

    /// <summary>
    /// Enable all the kinematics.
    /// </summary>
    public void EnableKinematics()
    {
        foreach (GameObject kinematic in LeftHandKinematics)
        {
            kinematic.GetComponent<InverseKinematics>().enabled = true;
        }
        foreach (GameObject kinematic in LeftArmKinematics)
        {
            kinematic.GetComponent<InverseKinematics>().enabled = true;
        }
        foreach (GameObject kinematic in LeftLegKinematics)
        {
            kinematic.GetComponent<InverseKinematics>().enabled = true;
        }
        foreach (GameObject kinematic in RightArmKinematics)
        {
            kinematic.GetComponent<InverseKinematics>().enabled = true;
        }
        foreach (GameObject kinematic in RightHandKinematics)
        {
            kinematic.GetComponent<InverseKinematics>().enabled = true;
        }
        foreach (GameObject kinematic in RightLegKinematics)
        {
            kinematic.GetComponent<InverseKinematics>().enabled = true;
        }
        foreach (GameObject kinematic in ChestKinematics)
        {
            kinematic.GetComponent<InverseKinematics>().enabled = true;
        }
    }
}