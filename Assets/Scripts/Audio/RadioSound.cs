using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioSound : MonoBehaviour
{
    private void OnEnable()
    {
        GetComponent<AudioSource>().Play();
    }
}
