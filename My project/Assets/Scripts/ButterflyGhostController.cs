using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButterflyGhostController : MonoBehaviour
{
    private Animator animatorButterfly;
    private Animator animatorCorporate;
    private bool isTransitionedOriginal = false;
    private bool isTransitionedInverted = false;
    
    // private StateBar stateBarScript;
    // private VarInvertedWorld varInvertedWorld;

    // Testweise im Inspector hinzugef�gte Variablen
    public bool testIsDangerActive;
    public string testInvertedWorld;

    private GameObject butterflyGameObject;
    private GameObject corporateSlaveGameObject;


    private void Start()
    {
        butterflyGameObject = transform.Find("Butterfly").gameObject;
        corporateSlaveGameObject = transform.Find("CorporateSlave").gameObject;
        animatorButterfly = butterflyGameObject.GetComponent<Animator>();
        animatorCorporate = corporateSlaveGameObject.GetComponent<Animator>();
    }

    private void Update()
    {
        if (testInvertedWorld == "false")
        {
            isTransitionedInverted = false;
            butterflyGameObject.SetActive(true);
            corporateSlaveGameObject.SetActive(false);

            // if (VarInvertedWorld.invertedWorld == "false") //when original world is active
            if (testIsDangerActive == false) // Testweise Variable
            {
                animatorButterfly.Play("ButterflyEnemy");
            }
                
            // if (stateBarScript.isDangerActive) //in original world and danger zone begins

            else if (testIsDangerActive && !isTransitionedOriginal) // Testweise Variable
            {
                StartCoroutine(PlayDangerAnimations(animatorButterfly, "ButterflyDangerTransition", "Ghost")); //play transition animation from normal to monster and then keep monster state
                isTransitionedOriginal = true;
            }
        }

        else
        {
            isTransitionedOriginal = false;
            butterflyGameObject.SetActive(false);
            corporateSlaveGameObject.SetActive(true);

            if (testIsDangerActive == false) // Testweise Variable
            {
                animatorCorporate.Play("CorporateSlave");
            }

            // if (stateBarScript.isDangerActive) //in original world and danger zone begins

            else if (testIsDangerActive && !isTransitionedInverted) // Testweise Variable
            {
                StartCoroutine(PlayDangerAnimations(animatorCorporate, "CorporateDangerTransition", "Fly")); //play transition animation from normal to monster and then keep monster state
                isTransitionedInverted = true;
            }
        }
    }

    private IEnumerator PlayDangerAnimations(Animator animator, string animation1, string animation2)
    {
        animator.Play(animation1);
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        animator.Play(animation2);
    }
}