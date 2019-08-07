using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {
	//  Components
	private Rigidbody rb;                                               // Player body
	public GameObject body;                                             // Player gameobject body
	private Animator animator;                                          // Body animator
																		//private TrailRenderer trail;                                        // Trail renderer in player body
																		//  Camera Vars
	private GameObject cameraObj;                                       // The camera object
	private Camera camera;                                              // The camera component
	public float speedH = 2.0f;                                         // Sensitivity horizontal
	public float speedV = 2.0f;                                         // Sensitivity vertical
	public float maxY = 89.9f;                                          // Clamps player's vertical angle between this
	public float minY = -89.9f;                                         // And this
	public float cruiseLMod = 0.5f;                                     // Multiplier for look speed when cruising
	public bool isZoom = false;                                         // Is the player zooming
	[SerializeField]
	private Vector3 cZoom;                                              // Where the camera is when zoomed
	public Vector3 cUnZoom = new Vector3(-1, 1, -3);                    // Where it is when it's not
	private Vector3 target;                                             // Where the camera wants to be
	private float rotX = 0.0f;                                          // Current rotation, don't touch this
	private float rotY = 0.0f;                                          // Ditto
	private float uFOV;                                                 // Unzoomed FOV
	public float zFOV = 60;                                             // Zoomed FOV
	private float tFOV;                                                 // Target FOV
	public float zSpeed = 1f;                                           // How long it takes to zoom
	private float zCLerp = 0f;                                          // Current lerp time
	public float zCLerpSpeed = 1f;                                      // Lerp speed, lower = better, should never be 0

	//  Movement Vars
	public float accSpeed = 200.0f;                                     // How much force is applied each tick
	public float dTime = 1f;                                            // How long the player is "dodging" for
	private bool isDodge = false;                                       // Is the player dodging
	public float dForce = 10000.0f;                                     // How much force is applied on dodge
	public bool isCruising = false;                                    // Is the player afterburning
	public float cruiseMod = 0.10f;                                     // Multiplier for all movement except forward when cruising
	public float cruiseFwd = 2f;                                        // Multiplier for forward movement when cruising
	private float cHoldTime = 0f;                                       // How long player has held dodge key for
	public float cHoldThreshold = 0.5f;                                 // How long the player has to hold dodge to cruise

	//  Weapon vars
	public GameObject arm;                                              // Object that holds the gun
	public List<GameObject> weapons = new List<GameObject>();           // Current inventory
	public int cWweap = 0;                                              // Current weapon selected


	// Josh Addition for Displacement Movement
	private float flightSpeedScaleFactor = 6f;

	private float maxVelocity = 1000f;
	private bool flightStopped = false;


	public GameObject hudFrame;
	private Color hudMainColour;

	//private float startTimeHoldingW = -1;
	//private float startTimeHoldingA = -1;
	//private float startTimeHoldingS = -1;
	//private float startTimeHoldingD = -1;
	//private float startTimeHoldingCtrl = -1;
	//private float startTimeHoldingSpace = -1;

	//private float timeHoldingW = 0;
	//private float timeHoldingA = 0;
	//private float timeHoldingS = 0;
	//private float timeHoldingD = 0;
	//private float timeHoldingCtrl = 0;
	//private float timeHoldingSpace = 0;

	private float speedDecreaseRate;

	void Awake() {
		weapons = new List<GameObject>() { gameObject.transform.GetChild(2).GetChild(0).gameObject, gameObject.transform.GetChild(6).gameObject, gameObject.transform.GetChild(7).gameObject };
	}

	void Start() { // Mainly component getting
		camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
		cameraObj = camera.transform.parent.gameObject;
		cZoom = cameraObj.transform.localPosition;
		uFOV = camera.GetComponent<Camera>().fieldOfView;
		rb = GetComponent<Rigidbody>();
		//trail = body.GetComponent<TrailRenderer>();
		animator = body.GetComponent<Animator>();
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
		SwapWeapon(0);

		
		hudMainColour = hudFrame.GetComponent<Image>().color;
	}


	void FixedUpdate() {
		PlayerControls();
		AimWeapon();
		//AvoidObstruction();
	}

	void Update() {
		CameraMove();
	}

	void CameraMove() { // Camera controls and manipulation goes here

		// Deactivate Cruise Mod if button not pressed
		if (!Input.GetKey(KeyCode.LeftShift)) {
			isCruising = false;
			animator.SetBool("IsBoosting", false);
		}

		// Camera Movement based on cruising
		if (!isCruising) {
			rotX += speedH * Input.GetAxis("RightHorizontal");
			rotY -= speedV * Input.GetAxis("RightVertical");
			rotX += speedH * Input.GetAxis("Mouse X");
			rotY -= speedV * Input.GetAxis("Mouse Y");
			//if (!isDodge) {
			//	trail.time = 0.5f;
			//	trail.startWidth = 0.5f;
			//}
		} else {
			rotX += speedH * Input.GetAxis("RightHorizontal") * cruiseLMod;
			rotY -= speedV * Input.GetAxis("RightVertical") * cruiseLMod;
			rotX += speedH * Input.GetAxis("Mouse X") * cruiseLMod;
			rotY -= speedV * Input.GetAxis("Mouse Y") * cruiseLMod;
			//if (!isDodge) {
			//	trail.time = 2;
			//	trail.startWidth = 1f;
			//}

		}
		rotY = Mathf.Clamp(rotY, minY, maxY);
		transform.eulerAngles = new Vector3(rotY, rotX, 0.0f);
		zCLerp += Time.deltaTime;
		if (zCLerp > zCLerpSpeed) {
			zCLerp = zCLerpSpeed;
		}
		float t = zCLerp / zCLerpSpeed;
		t = Mathf.Sin(t * Mathf.PI * 0.5f);
		cameraObj.transform.localPosition = Vector3.Lerp(cameraObj.transform.localPosition, target, t);
		camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, tFOV, t);
	}

	void PlayerControls() { // Player controls and manipulation goes here
							//if (Input.GetButtonDown("Pause")) { // Release key
							//	Cursor.lockState = CursorLockMode.None;
							//	Cursor.visible = true;
							//}

		// Scanning
		if (gameObject.transform.root.GetComponent<PlayerHealth>().battery > 500) {
			if (Input.GetButton("Zoom")) {
				zCLerp = 0f;
				target = cUnZoom;
				tFOV = zFOV;
				isZoom = true;

				gameObject.transform.root.GetComponent<PlayerHealth>().UseBattery(50 * Time.deltaTime);
			} else {
				zCLerp = 0f;
				tFOV = uFOV;
				target = cZoom;
				isZoom = false;
			}
		} else {
			zCLerp = 0f;
			tFOV = uFOV;
			target = cZoom;
			isZoom = false;
		}






		// Movement
		// Check the inputs for which are being used
		if (!flightStopped) {
			if (isCruising) maxVelocity = 2500f;
			else maxVelocity = 1000f;

			float speedFactor = accSpeed;
			if (isCruising) gameObject.transform.root.GetComponent<PlayerHealth>().UseBattery(50f * Time.deltaTime);
			if (isCruising) rb.AddForce(rb.velocity.normalized * speedFactor * cruiseFwd);
			if (isCruising) gameObject.transform.GetChild(0).transform.rotation = Quaternion.LookRotation(rb.velocity.normalized);
			else gameObject.transform.GetChild(0).transform.rotation = gameObject.transform.rotation;// = Quaternion.LookRotation(rb.velocity.normalized);
			rb.AddForce(transform.forward * Input.GetAxis("LeftVertical") * accSpeed);
			rb.AddForce(transform.right * Input.GetAxis("LeftHorizontal") * accSpeed);
			rb.AddForce(transform.up * Input.GetAxis("AscDesc") * accSpeed);

			//if (Input.GetKey(KeyCode.W) && !isCruising) rb.AddForce(transform.forward * speedFactor);
			//if (Input.GetKey(KeyCode.S) && !isCruising) rb.AddForce(-transform.forward * speedFactor);
			//if (Input.GetKey(KeyCode.D) && !isCruising) rb.AddForce(transform.right * speedFactor);
			//if (Input.GetKey(KeyCode.A) && !isCruising) rb.AddForce(-transform.right * speedFactor);
			//if (Input.GetKey(KeyCode.Space) && !isCruising) rb.AddForce(transform.up * speedFactor);
			//if (Input.GetKey(KeyCode.LeftControl) && !isCruising) rb.AddForce(-transform.up * speedFactor);

			// Change the max velocity
			if (rb.velocity.magnitude > maxVelocity) rb.velocity = rb.velocity.normalized * maxVelocity;

		}

		// Check if the player can move in that direction




		// Inputs - Dodge/Afterburners
		if (Input.GetButtonDown("Dodge")) cHoldTime = 0f;
		if (Input.GetButton("Dodge")) {
			if (cHoldTime > cHoldThreshold && gameObject.transform.root.GetComponent<PlayerHealth>().battery >= 500) {
				isCruising = true;
				animator.SetBool("IsBoosting", true);
			} else cHoldTime += Time.deltaTime;
		}


		// Dodge if boost button tapped instead of held
		if (Input.GetButtonUp("Dodge") && cHoldTime < cHoldThreshold) {
			StartCoroutine(Dodge());
		} else if (( Input.GetButtonUp("Dodge") && isCruising ) || gameObject.transform.root.GetComponent<PlayerHealth>().battery < 500) {
			isCruising = false;
			animator.SetBool("IsBoosting", false);
		}

		// Weapon Select
		if (Input.GetButtonDown("Weapon1")) SwapWeapon(0);
		if (Input.GetButtonDown("Weapon2")) SwapWeapon(1);
		if (Input.GetButtonDown("Weapon3")) SwapWeapon(2);
	}


	private bool CheckMovement(Vector3 movementDirection) {
		RaycastHit hit;
		bool hitObject;
		hitObject = Physics.Raycast(transform.position, movementDirection, out hit, movementDirection.magnitude);
		if (!hitObject || hit.collider.gameObject.transform.root.gameObject.tag == "Player") {
			transform.position += movementDirection;
			return true;
		}
		return false;
	}

	void AimWeapon() {
		Ray ray = camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
		var layerMask = ~((1 << 9 | 1 << 10));
		if (Physics.Raycast(ray, out RaycastHit aimPoint, layerMask)) {
			arm.transform.LookAt(aimPoint.point);
		} else {
			arm.transform.localEulerAngles = new Vector3(0, 0, 0);
		}
	}

	void SwapWeapon(int wIndex) {
		if (wIndex < weapons.Count) {
			foreach (GameObject cWeapon in weapons) {
				cWeapon.tag = "Untagged";
				cWeapon.transform.GetComponent<Animator>().SetBool("active", false);
			}

			weapons[wIndex].tag = "CurrentWeapon";
			weapons[wIndex].GetComponent<Animator>().SetBool("active", true);
		}




		//if (wIndex < weapons.Count) {
		//	foreach (GameObject cWeapon in GameObject.FindGameObjectsWithTag("CurrentWeapon")) {
		//		Destroy(cWeapon);
		//	}
		//	GameObject nWeapon = Instantiate(weapons[wIndex], arm.transform, false);
		//	nWeapon.transform.localPosition = Vector3.zero;
		//	nWeapon.transform.localRotation = Quaternion.identity;
		//}
	}

	void AvoidObstruction() {
		if (Physics.Linecast(transform.position, cameraObj.transform.position, out RaycastHit obstruction)) {
			camera.transform.position = obstruction.point;
		} else {
			camera.transform.localPosition = Vector3.zero;
		}
	}

	IEnumerator Dodge() {
		if (!isDodge) {
			isDodge = true;
			if (FindObjectOfType<CameraMotion>()) {
				CameraMotion cMotion = FindObjectOfType<CameraMotion>();
				cMotion.mSpeed = cMotion.mSpeed * 3f;
			}
			if (Input.GetAxis("LeftHorizontal") > 0) animator.SetTrigger("DodgeRight");
			if (Input.GetAxis("LeftHorizontal") < 0) animator.SetTrigger("DodgeLeft");
			if (Input.GetAxis("LeftVertical") > 0) animator.SetTrigger("DodgeForward");
			rb.AddForce(transform.forward * Input.GetAxis("LeftVertical") * dForce * 100);
			rb.AddForce(transform.right * Input.GetAxis("LeftHorizontal") * dForce * 100);
			rb.AddForce(transform.up * Input.GetAxis("AscDesc") * dForce * 100);
			yield return new WaitForSeconds(dTime);
			if (FindObjectOfType<CameraMotion>()) {
				CameraMotion cMotion = FindObjectOfType<CameraMotion>();
				cMotion.mSpeed = cMotion.mSpeed / 3f;
			}
			isDodge = false;
			animator.ResetTrigger("DodgeRight");
			animator.ResetTrigger("DodgeLeft");
			animator.ResetTrigger("DodgeForward");
		}
	}

	private IEnumerator stopFlying() {
		flightStopped = true;
		Debug.Log("Warning: Engines cut to avoid detection");

		StartCoroutine(fall(Time.time));

		yield return new WaitForSeconds(2);

		//hudFrame.GetComponent<Image>().color = hudMainColour;
		flightStopped = false;
	}

	private IEnumerator fall(float startFallTime) {
		rb.AddForce(-Vector3.up * 750f * Mathf.Pow(startFallTime - Time.time, 2));
		yield return new WaitForSeconds(Time.deltaTime);

		if (flightStopped) {
			StartCoroutine(fall(startFallTime));
		}
	}

	private void OnTriggerEnter(Collider collider) {
		if (collider.gameObject.tag == "OrangeZon") {
			Debug.Log("Warning: Approaching 'No Fly' Zone");

			hudFrame.GetComponent<Image>().color = Color.yellow;
		}

		if (collider.gameObject.tag == "RedZone") {
			StartCoroutine(stopFlying());

			hudFrame.GetComponent<Image>().color = Color.red;
		}
	}

	private void OnTriggerExit(Collider collider) {
		if (collider.gameObject.tag == "OrangeZon") hudFrame.GetComponent<Image>().color = hudMainColour;
	}
}





// Removed Code from before the re-implementation of Sebastiens force movement
// Feel free to ignore this



//// Scale the factor of the flight speed based on how long the buttom has be held for
//speedDecreaseRate = Time.deltaTime * 3;

//if (Input.GetKey(KeyCode.W)) {
//	if (startTimeHoldingW == -1) startTimeHoldingW = Time.time;
//	timeHoldingW = timeHoldingW + Time.time - startTimeHoldingW;
//} else {
//	startTimeHoldingW = -1;
//	timeHoldingW = Mathf.Max(0, timeHoldingW - speedDecreaseRate);
//}

//if (Input.GetKey(KeyCode.A)) {
//	if (startTimeHoldingA == -1) startTimeHoldingA = Time.time;
//	timeHoldingA = timeHoldingA + Time.time - startTimeHoldingA;
//} else {
//	startTimeHoldingA = -1;
//	timeHoldingA = Mathf.Max(0, timeHoldingA - speedDecreaseRate);
//}

//if (Input.GetKey(KeyCode.S)) {
//	if (startTimeHoldingS == -1) startTimeHoldingS = Time.time;
//	timeHoldingS = timeHoldingS + Time.time - startTimeHoldingS;
//} else {
//	startTimeHoldingS = -1;
//	timeHoldingS = Mathf.Max(0, timeHoldingS - speedDecreaseRate);
//}

//if (Input.GetKey(KeyCode.D)) {
//	if (startTimeHoldingD == -1) startTimeHoldingD = Time.time;
//	timeHoldingD = timeHoldingD + Time.time - startTimeHoldingD;
//} else {
//	startTimeHoldingD = -1;
//	timeHoldingD = Mathf.Max(0, timeHoldingD - speedDecreaseRate);
//}

//if (Input.GetKey(KeyCode.LeftControl)) {
//	if (startTimeHoldingCtrl == -1) startTimeHoldingCtrl = Time.time;
//	timeHoldingCtrl = timeHoldingCtrl + Time.time - startTimeHoldingCtrl;
//} else {
//	startTimeHoldingCtrl = -1;
//	timeHoldingCtrl = Mathf.Max(0, timeHoldingCtrl - speedDecreaseRate);
//}

//if (Input.GetKey(KeyCode.Space)) {
//	if (startTimeHoldingSpace == -1) startTimeHoldingSpace = Time.time;
//	timeHoldingSpace = timeHoldingSpace + Time.time - startTimeHoldingSpace;
//} else {
//	startTimeHoldingSpace = -1;
//	timeHoldingSpace = Mathf.Max(0, timeHoldingSpace - speedDecreaseRate);
//}


//if (!isDodge) {
//	Vector3 movementDirection = new Vector3(0, 0, 0);


//	float mods = 1f;

//	if (isCruising) {
//		mods = cruiseMod;

//		if (gameObject.transform.root.GetComponent<PlayerHealth>().battery >= 500) {
//			bool moved = CheckMovement(transform.forward * accSpeed * cruiseFwd * Time.deltaTime / flightSpeedScaleFactor);

//			if (moved) gameObject.transform.root.GetComponent<PlayerHealth>().UseBattery(10);
//		}
//	} else {
//		mods = 1f;

//		timeHoldingW = Mathf.Min(1f, timeHoldingW);
//		timeHoldingS = Mathf.Min(1f, timeHoldingS);

//		float factorFB = timeHoldingW - timeHoldingS;

//		CheckMovement(factorFB * transform.forward * accSpeed * mods * Time.deltaTime / flightSpeedScaleFactor);
//	}

//	timeHoldingA = Mathf.Min(1f, timeHoldingA);
//	timeHoldingD = Mathf.Min(1f, timeHoldingD);
//	float factorLR = Mathf.Sin(timeHoldingD * Mathf.PI / 4) - Mathf.Sin(timeHoldingA * Mathf.PI / 4);
//	CheckMovement(factorLR * transform.right * accSpeed * mods * Time.deltaTime / flightSpeedScaleFactor);

//	timeHoldingCtrl = Mathf.Min(1f, timeHoldingCtrl);
//	timeHoldingSpace = Mathf.Min(1f, timeHoldingSpace);
//	float factorUP = timeHoldingSpace - timeHoldingCtrl;
//	CheckMovement(factorUP * transform.up * accSpeed * mods * Time.deltaTime / flightSpeedScaleFactor);

//	//float mods = 1f;

//	//if (isCruising) {
//	//	mods = cruiseMod;

//	//	movementDirection += transform.forward * accSpeed * cruiseFwd * Time.deltaTime / flightSpeedScaleFactor;

//	//} else {
//	//	mods = 1f;

//	//	timeHoldingW = Mathf.Min(1f, timeHoldingW);
//	//	timeHoldingS = Mathf.Min(1f, timeHoldingS);

//	//	float factorFB = timeHoldingW - timeHoldingS;

//	//	movementDirection += factorFB * transform.forward * accSpeed * mods * Time.deltaTime / flightSpeedScaleFactor;
//	//}

//	//timeHoldingA = Mathf.Min(1f, timeHoldingA);
//	//timeHoldingD = Mathf.Min(1f, timeHoldingD);

//	//timeHoldingCtrl = Mathf.Min(1f, timeHoldingCtrl);
//	//timeHoldingSpace = Mathf.Min(1f, timeHoldingSpace);

//	//float factorLR = Mathf.Sin(timeHoldingD * Mathf.PI / 4) - Mathf.Sin(timeHoldingA * Mathf.PI / 4);
//	//float factorUP = timeHoldingSpace - timeHoldingCtrl;


//	//movementDirection += factorLR * transform.right * accSpeed * mods * Time.deltaTime / flightSpeedScaleFactor;
//	//movementDirection += factorUP * transform.up * accSpeed * mods * Time.deltaTime / flightSpeedScaleFactor;

//	//RaycastHit hit;
//	//bool hitObject = Physics.Raycast(transform.position, movementDirection, out hit, movementDirection.magnitude);
//	//if (hitObject && hit.collider.gameObject.transform.root.gameObject.tag != "Player") {
//	//	movementDirection = Vector3.zero;
//	//}
//	//transform.position += movementDirection;
//}