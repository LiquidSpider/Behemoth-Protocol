using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Crosshair : MonoBehaviour
{
    public GameObject notchU;			
    public GameObject notchD;
    public GameObject notchL;
    public GameObject notchR;
    
    void Update()
    {
        GunTemplate gun = null;
        GameObject gunObject = GameObject.FindGameObjectWithTag("CurrentWeapon");

        if (gunObject) {
            gun = gunObject.GetComponent<GunTemplate>();
        }

        if (gun) {
            notchU.GetComponent<RectTransform>().anchoredPosition = Vector3.up * (20f + (gun.cAcc * 0.50f));
            notchD.GetComponent<RectTransform>().anchoredPosition = Vector3.down * (20f + (gun.cAcc * 0.50f));
            notchL.GetComponent<RectTransform>().anchoredPosition = Vector3.left * (20f + (gun.cAcc * 0.50f));
            notchR.GetComponent<RectTransform>().anchoredPosition = Vector3.right * (20f + (gun.cAcc * 0.50f));
        }
    }
}
