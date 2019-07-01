using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class missileLaunch : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void fire() {
    	StartCoroutine(firing(Random.Range(0f, 0.5f)));
    }

    private IEnumerator firing(float WaitTime) {
    	yield return new WaitForSeconds(WaitTime);
    	Debug.Log("Fire here");
    }
}
