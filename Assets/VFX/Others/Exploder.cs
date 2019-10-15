using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exploder : MonoBehaviour
{

    public GameObject explosion;
    public GameObject damageSmoke;

    private static GameObject explosionStat;
    private static GameObject damageSmokeStat;

    // Start is called before the first frame update
    void Start()
    {
        explosionStat = explosion;
        damageSmokeStat = damageSmoke;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void explode(Transform location)
    {
        GameObject expTemp = Instantiate(explosionStat, location);
        Destroy(expTemp, 4);
    }

	public static void damagedSmoke(GameObject parentObject, Transform spawnLocation)
    {
        GameObject damageSmokeTemp = Instantiate(damageSmokeStat, spawnLocation);
        damageSmokeTemp.transform.SetParent(parentObject.transform);
    }

    public static void damagedSmokeWRotation(GameObject parentObject, Vector3 rotateAmount)
    {
        Transform newLoc = parentObject.transform;
        GameObject damageSmokeTemp = Instantiate(damageSmokeStat, parentObject.transform);
        damageSmokeTemp.transform.Rotate(rotateAmount.x, rotateAmount.y, rotateAmount.z);
    }
}
