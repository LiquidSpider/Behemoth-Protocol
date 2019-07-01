using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DFSpawner : MonoBehaviour {

	GameObject myDF;
	public GameObject DF;

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        if(myDF == null) {
        	spawnDF();
        }
    }

    public void spawnDF() {
    	myDF = Instantiate(DF, this.transform.position, Quaternion.identity);
    }
}
