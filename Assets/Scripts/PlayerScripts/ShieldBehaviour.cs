using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldBehaviour : MonoBehaviour {

	public GameObject shield;
	public bool shieldActive;
    public float drainAmount = 50;

    private UISounds ui;


	void Start() {
        //shield.SetActive(false);
        ui = FindObjectOfType<UISounds>();
		shieldActive = false;
	}
	
	void Update() {

        if (!FindObjectOfType<GameManager>().gameOver)
        {
            if (gameObject.transform.root.GetComponent<PlayerHealth>().battery > drainAmount && !gameObject.transform.root.GetComponent<PlayerController>().isCruising)
            {
                if (Input.GetButton("Shield") && shieldActive == false)
                {
                    shieldActive = true;
                    shield.SetActive(true);
                    shield.transform.GetChild(0).localScale = new Vector3(0.05f, 0.05f, 0.05f);
                    ui.ShieldActivate();

                }
                else if (Input.GetButtonUp("Shield"))
                {
                    shieldActive = false;
                    shield.SetActive(false);
                }
            }
            else
            {
                shieldActive = false;
                shield.SetActive(false);
            }


            if (shieldActive) gameObject.transform.root.GetComponent<PlayerHealth>().UseBattery(drainAmount * Time.deltaTime);
        }
	}
}
