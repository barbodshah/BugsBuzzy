using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAnimation : MonoBehaviour
{
    public Animator animator;

    public bool revolver;
    public GameObject magazine;

    public Vector3 localAxis = Vector3.up;
    public float duration;

    public void PlayAnimation()
    {
        animator.Play("Shoot");
        if(revolver) StartCoroutine(RotateBy60());
    }

    IEnumerator RotateBy60()
    {
        Quaternion startRot = magazine.transform.localRotation;
        Quaternion targetRot =
            startRot * Quaternion.AngleAxis(60f, localAxis.normalized);

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / duration;
            magazine.transform.localRotation = Quaternion.Slerp(startRot, targetRot, t);
            yield return null;             // wait one frame
        }

        magazine.transform.localRotation = targetRot; // snap exactly to the end
    }
}
