using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandAnimator : MonoBehaviour
{
    public Animator handAnim;
    public FPSWalkerEnhanced walkerEnhanced;
    public FPSController fPSController;

    [SerializeField] float speedTarget = 1;
    [SerializeField] float currentSpeed = 1;

    Coroutine accelerate, deccelerate;


    private void Update()
    {
        Vector2 moveDirection = new Vector2(walkerEnhanced.m_MoveDirection.x, walkerEnhanced.m_MoveDirection.z);

        if(moveDirection.magnitude >= 0.1f)
        {
            if (walkerEnhanced.running)
            {
                speedTarget = 2;
            }
            else
            {
                speedTarget = 1;
                //handAnim.SetFloat("WalkSpeed", 1);
            }
        }
        else
        {
            speedTarget = 0.1f;
            //handAnim.SetFloat("WalkSpeed", 0.1f);
        }

        if(Mathf.Abs(speedTarget - currentSpeed) < 0.05)
        {
            currentSpeed = speedTarget;
        }
        else
        {
            if(currentSpeed < speedTarget)
            {
                currentSpeed += Time.deltaTime;
            }
            else
            {
                currentSpeed -= Time.deltaTime;
            }
        }
        handAnim.SetFloat("WalkSpeed", currentSpeed);
    }
}
