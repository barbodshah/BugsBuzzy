using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bleeding : MonoBehaviour
{
    private Animator animator;
    public bool isBleeding;
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();

    }

    void Update()
    {
        animator.SetBool("bleed", isBleeding);
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("idle") &&
            animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            isBleeding = false;
        }
    }
}
