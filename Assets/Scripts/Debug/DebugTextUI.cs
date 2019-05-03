using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugTextUI : MonoBehaviour
{
    public Text vert;
    public Text horiz;
    public Text ascdesc;
    public Text dist;
    public GameObject character;
    public GameObject target;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        vert.text = "Input Vert: " + Input.GetAxis("LeftVertical");
        horiz.text = "Input Horiz: " + Input.GetAxis("LeftHorizontal");
        ascdesc.text = "Input AscDesc: " + Input.GetAxis("AscDesc");
        dist.text = "Dist: " + Vector3.Distance(character.transform.position, target.transform.position);
    }
}
