using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorManager : MonoBehaviour
{
    Animator animator;
    Rigidbody rb;
    ShieldBehaviour shield;
    Vector2 inputVector;
    Vector3 originalLocation;
    // Start is called before the first frame update
    void Start()
    {
        shield = FindObjectOfType<ShieldBehaviour>();
        rb = FindObjectOfType<PlayerController>().gameObject.GetComponent<Rigidbody>();
        animator = gameObject.GetComponent<Animator>();
        originalLocation = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("LeftVertical") != 0 || Input.GetAxis("LeftHorizontal") != 0 || Input.GetAxis("AscDesc") != 0)
        {
            animator.SetBool("IsMoving", true);
        } else {
            animator.SetBool("IsMoving", false);
        }
        inputVector.x = Input.GetAxis("LeftHorizontal");
        inputVector.y = Input.GetAxis("LeftVertical");
        animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), -inputVector.normalized.x, 0.1f));
        animator.SetLayerWeight(2, Mathf.Lerp(animator.GetLayerWeight(2), inputVector.normalized.x, 0.1f));
        animator.SetLayerWeight(3, Mathf.Lerp(animator.GetLayerWeight(3), -inputVector.normalized.y, 0.1f));
        animator.SetLayerWeight(4, Mathf.Lerp(animator.GetLayerWeight(4), inputVector.normalized.y, 0.1f));
        animator.SetBool("IsShielding", shield.shieldActive);
    }

    private void LateUpdate()
    {
        transform.localPosition = originalLocation;
    }
}
