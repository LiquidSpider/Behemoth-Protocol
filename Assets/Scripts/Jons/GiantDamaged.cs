using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiantDamaged : MonoBehaviour
{

	//public GameObject myself;

	[Space(20)]
    public GameObject rightArmSmoke1;
	public GameObject rightArmSmoke2;
	public GameObject rightArmSmoke3;
	public GameObject rightArmSmoke4;
	public GameObject rightArmSmoke5;

    [Space(20)]
	public GameObject leftArmSmoke1;
	public GameObject leftArmSmoke2;
	public GameObject leftArmSmoke3;
	public GameObject leftArmSmoke4;
	public GameObject leftArmSmoke5;

	[Space(20)]
    public GameObject legsSmoke1;
	public GameObject legsSmoke2;
	public GameObject legsSmoke3;
	public GameObject legsSmoke4;
	public GameObject legsSmoke5;

	[Space(20)]
    public GameObject bodySmoke1;
	public GameObject bodySmoke2;
	public GameObject bodySmoke3;
	public GameObject bodySmoke4;
	public GameObject bodySmoke5;

    private int whichBodyPart = -1;

    // Start is called before the first frame update
    void Start()
    {
        //myself = this.gameObject;
    }

    //Activate the respective smokes on the right body parts when enough damaged is dealt to each section
    public void checkSmoking(BaseHealth damageablePart) {

        if(damageablePart.healthLayer == 1 << 18) {
            whichBodyPart = 4;
        } else if(damageablePart.healthLayer == 1 << 14) {
            whichBodyPart = 3;
        } else if(damageablePart.healthLayer == 1 << 16) {
            whichBodyPart = 2;
        } else if(damageablePart.healthLayer == 1 << 17) {
            whichBodyPart = 1;
        }


    	if(damageablePart.health < damageablePart.startingHealth * 0.8f) {
    		//turn on first thingo
    		switch(whichBodyPart) {
                case 1:
                    rightArmSmoke1.SetActive(true);
                    break;
                case 2:
                    leftArmSmoke1.SetActive(true);
                    break;
                case 3:
                    legsSmoke1.SetActive(true);
                    break;
                case 4:
                    bodySmoke1.SetActive(true);
                    break;
                default:
                    Debug.Log("Body part not found");
                    break;
            }
    	}
    	if(damageablePart.health < damageablePart.startingHealth * 0.65f) {
    		//turn on second thingo
            switch(whichBodyPart) {
                case 1:
                    rightArmSmoke2.SetActive(true);
                    break;
                case 2:
                    leftArmSmoke2.SetActive(true);
                    break;
                case 3:
                    legsSmoke2.SetActive(true);
                    break;
                case 4:
                    bodySmoke2.SetActive(true);
                    break;
                default:
                    Debug.Log("Body part not found");
                    break;
            }
    	}
    	if(damageablePart.health < damageablePart.startingHealth * 0.5f) {
    		//turn on third thingo
            switch(whichBodyPart) {
                case 1:
                    rightArmSmoke3.SetActive(true);
                    break;
                case 2:
                    leftArmSmoke3.SetActive(true);
                    break;
                case 3:
                    legsSmoke3.SetActive(true);
                    break;
                case 4:
                    bodySmoke3.SetActive(true);
                    break;
                default:
                    Debug.Log("Body part not found");
                    break;
            }
    	}
    	if(damageablePart.health < damageablePart.startingHealth * 0.3f) {
    		//turn on fourth thingo
            switch(whichBodyPart) {
                case 1:
                    rightArmSmoke4.SetActive(true);
                    break;
                case 2:
                    leftArmSmoke4.SetActive(true);
                    break;
                case 3:
                    legsSmoke4.SetActive(true);
                    break;
                case 4:
                    bodySmoke4.SetActive(true);
                    break;
                default:
                    Debug.Log("Body part not found");
                    break;
            }
    	}
    	if(damageablePart.health < damageablePart.startingHealth * 0.1f) {
    		//turn on fifth thingo
            switch(whichBodyPart) {
                case 1:
                    rightArmSmoke5.SetActive(true);
                    break;
                case 2:
                    leftArmSmoke5.SetActive(true);
                    break;
                case 3:
                    legsSmoke5.SetActive(true);
                    break;
                case 4:
                    bodySmoke5.SetActive(true);
                    break;
                default:
                    Debug.Log("Body part not found");
                    break;
            }
    	}
    }

    //Legs should be the only thing to repair themselves - so when repair is called it will make the smoke inactive
    public void stopSmoking(BaseHealth repairedSection) {
        legsSmoke2.SetActive(false);
        legsSmoke3.SetActive(false);
        legsSmoke4.SetActive(false);
        legsSmoke5.SetActive(false);
    }

}
/*
if (health.healthLayer == 1 << 18)
            {
                baseHealth = health;
            }else if (health.healthLayer == 1 << 14)
            {
                LegsHealth = health;
            }else if (health.healthLayer == 1 << 16)    
            {
                LeftArmHealth = health;
            }else if (health.healthLayer == 1 << 17)
            {
                RightArmHealth = health;
            }
            */