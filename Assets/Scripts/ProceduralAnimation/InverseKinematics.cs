using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]

public class InverseKinematics : MonoBehaviour {

    // Joints
	public Transform startingJoint;
	public Transform middleJoint;
	public Transform finishedJoint;
	public Transform middleTarget;
	public Transform target;
	[Space(20)]
    // Setup Rotations
	public Vector3 startingJoint_OffsetRotation;
	public Vector3 middleJoint_OffsetRotation;
	public Vector3 finishedJoint_OffsetRotation;
	[Space(20)]
	public bool finishedJointMatchesTargetRotation = true;
	[Space(20)]
	public bool debug;

    // Calculation variables.
	float angle;
	float startingJoint_Length;
	float middleJoint_Length;
	float middleToStart_Length;
	float targetDistance;
	float adyacent;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void LateUpdate () {
		if(startingJoint != null && middleJoint != null && finishedJoint != null && middleTarget != null && target != null){
			startingJoint.LookAt (target, middleTarget.position - startingJoint.position);
			startingJoint.Rotate (startingJoint_OffsetRotation);

			Vector3 cross = Vector3.Cross (middleTarget.position - startingJoint.position, middleJoint.position - startingJoint.position);

            // find the length of the joints.
			startingJoint_Length = Vector3.Distance (startingJoint.position, middleJoint.position);
			middleJoint_Length =  Vector3.Distance (middleJoint.position, finishedJoint.position);
			middleToStart_Length = startingJoint_Length + middleJoint_Length;
            // calulate the distance from the joints to the targets
			targetDistance = Vector3.Distance (startingJoint.position, target.position);
			targetDistance = Mathf.Min (targetDistance, middleToStart_Length - middleToStart_Length * 0.001f);

			adyacent = ((startingJoint_Length * startingJoint_Length) - (middleJoint_Length * middleJoint_Length) + (targetDistance * targetDistance)) / (2*targetDistance);

			angle = Mathf.Acos (adyacent / startingJoint_Length) * Mathf.Rad2Deg;

			startingJoint.RotateAround (startingJoint.position, cross, -angle);

			middleJoint.LookAt(target, cross);
			middleJoint.Rotate (middleJoint_OffsetRotation);

			if(finishedJointMatchesTargetRotation){
				finishedJoint.rotation = target.rotation;
				finishedJoint.Rotate (finishedJoint_OffsetRotation);
			}
			
			if(debug){
				if (middleJoint != null && middleTarget != null) {
					Debug.DrawLine (middleJoint.position, middleTarget.position, Color.blue);
				}

				if (startingJoint != null && target != null) {
					Debug.DrawLine (startingJoint.position, target.position, Color.red);
				}
			}
					
		}
		
	}

	void OnDrawGizmos(){
		if (debug) {
			if(startingJoint != null && middleTarget != null && finishedJoint != null && target != null && middleTarget != null){
				Gizmos.color = Color.gray;
				Gizmos.DrawLine (startingJoint.position, middleJoint.position);
				Gizmos.DrawLine (middleJoint.position, finishedJoint.position);
				Gizmos.color = Color.red;
				Gizmos.DrawLine (startingJoint.position, target.position);
				Gizmos.color = Color.blue;
				Gizmos.DrawLine (middleJoint.position, middleTarget.position);
			}
		}
	}

}
