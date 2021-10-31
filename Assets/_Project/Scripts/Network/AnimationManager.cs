using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
namespace Vanguard {
    public class AnimationManager : NetworkBehaviour
    {

        public Animator animator;
        public void ChangeState(string newState)
        {
            Debug.Log("new state is " + newState);
            if (newState == "idle")
            {
                animator.SetFloat("Speed", 0);
                animator.SetBool("Crouching", false);
                animator.SetBool("Sliding", false);
                animator.SetBool("Wallrunning", false);
                animator.SetBool("Falling", false);
            }
            else if (newState == "walking")
            {
                animator.SetFloat("Speed", 1);
                animator.SetBool("Crouching", false);
                animator.SetBool("Sliding", false);
                animator.SetBool("Wallrunning", false);
                animator.SetBool("Falling", false);
            }
            else if (newState == "sliding")
            {
                animator.SetFloat("Speed", 1);
                animator.SetBool("Crouching", true);
                animator.SetBool("Sliding", true);
                animator.SetBool("Wallrunning", false);
                animator.SetBool("Falling", false);
            }
            else if (newState == "wallrunning")
            {
                animator.SetFloat("Speed", 0);
                animator.SetBool("Crouching", false);
                animator.SetBool("Sliding", false);
                animator.SetBool("Wallrunning", true);
                animator.SetBool("Falling", false);
            }
            else if (newState == "falling")
            {
                animator.SetFloat("Speed", 0);
                animator.SetBool("Crouching", false);
                animator.SetBool("Sliding", false);
                animator.SetBool("Wallrunning", false);
                animator.SetBool("Falling", true);
            }
        }
        float X,lerpSpeed=10f;
        public void Update()
        {
            X = Mathf.Lerp(X, InputManager.WalkVector.x, lerpSpeed*Time.deltaTime);
            animator.SetFloat("X", X);
           
        }
    }
}
