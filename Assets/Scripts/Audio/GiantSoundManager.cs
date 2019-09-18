using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiantSoundManager : MonoBehaviour
{

    public AudioSource LeftHand;
    public AudioSource RightHand;
    AudioSource LHandFire;
    AudioSource RHandFire;
    int laserStep = -1;  // -1 = idle, 0 = aiming, 1 = fire, 2 = reset
    bool stepComplete = false;
    bool hasFired = false;

    public List<AudioClip> laserClips = new List<AudioClip>();

    GiantAnimator ga;

    void Start()
    {
        LHandFire = LeftHand.transform.GetChild(0).GetComponent<AudioSource>();
        RHandFire = RightHand.transform.GetChild(0).GetComponent<AudioSource>();
        ga = transform.root.GetComponent<GiantAnimator>();
        LeftHand.loop = false;
        RightHand.loop = false;

    }

    // Update is called once per frame
    void Update()
    {
        LaserSounds(ga.hand, laserStep);
    }

    public void SetStage(int stage)
    {
        laserStep = stage;
        stepComplete = false;
    }

    public void LaserSounds(GiantAnimator.Hand hand, int stage)
    {
        if (!stepComplete)
        {
            switch (stage)
            {
                case 0:
                    switch (hand)
                    {
                        case GiantAnimator.Hand.left:
                            LeftHand.clip = laserClips[0];
                            if (!LeftHand.isPlaying) LeftHand.Play();
                            hasFired = false;
                            stepComplete = true;
                            break;
                        case GiantAnimator.Hand.right:
                            RightHand.clip = laserClips[0];
                            if (!RightHand.isPlaying) RightHand.Play();
                            hasFired = false;
                            stepComplete = true;
                            break;
                    }
                    break;
                case 1:
                    switch (hand)
                    {
                        case GiantAnimator.Hand.left:
                            LeftHand.clip = laserClips[2];
                            LeftHand.loop = true;
                            LHandFire.clip = laserClips[1];
                            if (!hasFired)
                            {
                                LeftHand.Play();
                                LHandFire.Play();
                                hasFired = true;
                            }
                            
                            stepComplete = true;
                            break;
                        case GiantAnimator.Hand.right:
                            RightHand.clip = laserClips[2];
                            RightHand.loop = true;
                            RHandFire.clip = laserClips[1];
                            if (!hasFired)
                            {
                                RightHand.Play();
                                RHandFire.Play();
                                hasFired = true;
                            }
                            
                            stepComplete = true;
                            break;
                    }
                    break;
                case 2:
                    switch (hand)
                    {
                        case GiantAnimator.Hand.left:
                                LeftHand.loop = false;
                                LeftHand.Stop();
                                laserStep = -1;
                                stepComplete = true;
                            break;
                        case GiantAnimator.Hand.right:
                                RightHand.loop = false;
                                RightHand.Stop();
                                laserStep = -1;
                                stepComplete = true;
                            break;
                    }
                    break;
            }
        }
        
    }

}
