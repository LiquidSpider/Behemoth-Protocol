
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotPartFall : MonoBehaviour {
	private bool isTouchingGound = false;

	void Update() {
		if (!isTouchingGound) gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(0, -100, 0));
	}

	//private void OnCollisionEnter(Collision collision) {
	//	if (collision.other.tag == "Environment") isTouchingGound = true;
	//}

	//private void OnCollisionLeave(Collision collision) {
	//	if (collision.other.tag == "Environment") isTouchingGound = false;
	//}
}
