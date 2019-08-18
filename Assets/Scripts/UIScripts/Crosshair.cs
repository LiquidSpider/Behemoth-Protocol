using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Crosshair : MonoBehaviour
{
    public GameObject notchU;
    public GameObject notchBR;
    public GameObject notchBL;
    private Vector3 oNotchU;
    private Vector3 oNotchBR;
    private Vector3 oNotchBL;

    void Start() {  // Get starting position
        oNotchU = notchU.GetComponent<RectTransform>().anchoredPosition;
        oNotchBR = notchBR.GetComponent<RectTransform>().anchoredPosition;
        oNotchBL = notchBL.GetComponent<RectTransform>().anchoredPosition;
    }
    void Update()
    {
        GunTemplate gun = null;
        GameObject gunObject = GameObject.FindGameObjectWithTag("CurrentWeapon");

        if (gunObject) {
            gun = gunObject.GetComponent<GunTemplate>();
        }

        if (gun) {
            notchU.GetComponent<RectTransform>().anchoredPosition = oNotchU + Vector3.up * (gun.cAcc * 0.9f);
            notchBR.GetComponent<RectTransform>().anchoredPosition = oNotchBR + Vector3.down * (gun.cAcc * 0.60f) + Vector3.right * (gun.cAcc * 0.9f);
            notchBL.GetComponent<RectTransform>().anchoredPosition = oNotchBL + Vector3.down * (gun.cAcc * 0.60f) + Vector3.left * (gun.cAcc * 0.9f);
        }
    }
}
