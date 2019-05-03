using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthIndicator : MonoBehaviour
{
    public GameObject enemy;
    TextMesh text;

    void Start() {
        text = GetComponent<TextMesh>();
    }

    void Update()
    {
        text.text = enemy.GetComponent<Health>().health.ToString();
    }
}
