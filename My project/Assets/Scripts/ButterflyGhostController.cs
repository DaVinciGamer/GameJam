using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButterflyGhostController : MonoBehaviour
{
    private Animator animator;
    private bool isTransitionedOriginal = false;
    private bool isTransitionedInverted = false;
    
    // private StateBar stateBarScript;
    // private VarInvertedWorld varInvertedWorld;

    // Testweise im Inspector hinzugefügte Variablen
    public bool testIsDangerActive;
    public string testInvertedWorld;


    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if (testInvertedWorld == "false")
        {
            // if (VarInvertedWorld.invertedWorld == "false") //when original world is active
            if (testIsDangerActive == false) // Testweise Variable
            {
                animator.Play("Butterfly");
            }

            // if (stateBarScript.isDangerActive) //in original world and danger zone begins

            else if (testIsDangerActive && !isTransitionedOriginal) // Testweise Variable
            {
                isTransitionedOriginal = true;
                StartCoroutine(PlayDangerAnimations("ButterflyDangerTransition", "Ghost")); //play transition animation from normal to monster and then keep monster state
            }

        }

        else
        {
            if (testIsDangerActive == false) // Testweise Variable
            {
                animator.Play("CorporateSlave");
            }

            // if (stateBarScript.isDangerActive) //in original world and danger zone begins

            else if (testIsDangerActive && !isTransitionedInverted) // Testweise Variable
            {
                isTransitionedInverted = true;
                StartCoroutine(PlayDangerAnimations("CorporateSlaveDangerTransition", "Fly")); //play transition animation from normal to monster and then keep monster state
            }
        }
    }

    private IEnumerator PlayDangerAnimations(string animation1, string animation2)
    {
        
        animator.Play(animation1);
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        animator.Play(animation2);
    }
}
