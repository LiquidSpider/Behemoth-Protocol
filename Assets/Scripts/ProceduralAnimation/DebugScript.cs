using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugScript : MonoBehaviour
{

    public GameObject joint;
    public GameObject parent;
    public GameObject target;

    private float maxdistance;

    private Vector3 gizmoTargetPositionLocation;
    private Vector3 parentLocal;

    // Start is called before the first frame update
    void Start()
    {
        parentLocal = joint.gameObject.transform.InverseTransformPoint(parent.transform.position);
        maxdistance = Vector3.Distance(parentLocal, joint.transform.localPosition);
        Debug.Log(maxdistance);
    }

    // Update is called once per frame
    void Update()
    {

        gizmoTargetPositionLocation = joint.gameObject.transform.InverseTransformPoint(target.transform.position);
        parentLocal = joint.gameObject.transform.InverseTransformPoint(parent.transform.position);

        Debug.Log(joint.transform.localRotation);

    }

    private void OnDrawGizmos()
    {
        Gizmos.matrix = joint.transform.localToWorldMatrix;
        Gizmos.DrawWireSphere(gizmoTargetPositionLocation, 0.1f);
        Gizmos.DrawWireSphere(parentLocal, 0.1f);
    }
}
