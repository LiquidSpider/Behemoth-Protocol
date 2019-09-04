using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{

    /// <summary>
    /// The player
    /// </summary>
    public GameObject Target;

    /// <summary>
    /// The starting local Position of the object.
    /// </summary>
    private Vector3 StartingLocalPosition;

    /// <summary>
    /// The starting local Euler angles of the object.
    /// </summary>
    private Vector3 StartingLocalEuler;

    /// <summary>
    /// The starting local scale of the object.
    /// </summary>
    private Vector3 StartingLocalScale;

    /// <summary>
    /// The starting position of the object.
    /// </summary>
    private Vector3 StartingPosition;

    /// <summary>
    /// The starting rotation of the object.
    /// </summary>
    private Vector3 StartingRotation;

    /// <summary>
    /// Determines if to fully reset.
    /// </summary>
    [System.NonSerialized]
    public bool FullReset = false;

    /// <summary>
    /// The time it takes for the laser to reach the player.
    /// </summary>
    public float LaserShootTiming;

    /// <summary>
    /// The time it takes for the laser to reach the wind up.
    /// </summary>
    public float WindUpTiming;

    /// <summary>
    /// The starting time of the animation;
    /// </summary>
    private float AnimationStartingTime;

    private void Awake()
    {
    }

    // Start is called before the first frame update
    void Start()
    {
        this.StartingLocalPosition = this.transform.localPosition;
        this.StartingLocalScale = this.transform.localScale;
        this.StartingLocalEuler = this.transform.localEulerAngles;
        this.StartingPosition = this.transform.position;
        this.StartingRotation = this.transform.eulerAngles;
    }

    // Update is called once per frame
    void Update()
    {


        gameObject.GetComponent<particleControllerLaserCharge>().activate();

        // Once we're past the startup Timing
        if(AnimationStartingTime > WindUpTiming)
        {
            // Debug rotate towards player.
            Vector3 lookVector = Target.transform.position - transform.position;
            //lookVector.y = transform.position.y;
            Quaternion rot = Quaternion.LookRotation(lookVector);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, 1);

            // Stretch the object between the player and the giant.
            transform.GetChild(0).gameObject.SetActive(true);
            gameObject.GetComponent<particleControllerLaserCharge>().deactivate();

            float Distance = Vector3.Distance(Target.transform.position, this.transform.position) / 12;
            this.transform.localScale = new Vector3(this.StartingLocalScale.x, this.StartingLocalScale.y, Distance);

            if (AnimationStartingTime > WindUpTiming + LaserShootTiming)
            {
                this.Reset();
                // Reset and disable
                this.enabled = false;
            }
        }

        AnimationStartingTime += Time.deltaTime;
    }

    /// <summary>
    /// When this script is enabled.
    /// </summary>
    private void OnEnable()
    {
        // Set the initial starting time.
        this.AnimationStartingTime = 0;
    }

    /// <summary>
    /// Resets the objects LocalPosition and Scale.
    /// </summary>
    public void Reset()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        this.transform.localPosition = this.StartingLocalPosition;
        this.transform.localScale = this.StartingLocalScale;
        this.AnimationStartingTime = 0;
    }

    /// <summary>
    /// Resets the object completely.
    /// </summary>
    public void ResetAll()
    {
        this.transform.localPosition = this.StartingLocalPosition;
        this.transform.localScale = this.StartingLocalScale;
        this.transform.localEulerAngles = this.StartingLocalEuler;
    }
}
