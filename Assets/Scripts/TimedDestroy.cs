using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedDestroy : MonoBehaviour
{
    // Start is called before the first frame update
    private float time = 0f;
    public float maxTime = 1f;

    // Update is called once per frame
    void Update() {
        time += Time.deltaTime;
        if (time > maxTime) {
            Destroy(gameObject);
        }
    }
}
