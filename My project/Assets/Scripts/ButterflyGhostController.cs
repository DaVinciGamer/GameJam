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

    // Testweise im Inspector hinzugefügte Variablen
    public bool testIsDangerActive;
    public string testInvertedWorld;

    private GameObject butterflyGameObject;
    private GameObject corporateSlaveGameObject;


    private void Start()
    {
        butterflyGameObject = transform.Find("Butterfly").gameObject;
        corporateSlaveGameObject = transform.Find("CorporateSlave").gameObject;
        animatorButterfly = butterflyGameObject.GetComponentInChildren<Animator>();
        animatorCorporate = corporateSlaveGameObject.GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if (testInvertedWorld == "false")
        {
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
                isTransitionedOriginal = true;
                StartCoroutine(PlayDangerAnimations("ButterflyDangerTransition", "Ghost")); //play transition animation from normal to monster and then keep monster state
            }
        }

        else
        {
            butterflyGameObject.SetActive(false);
            corporateSlaveGameObject.SetActive(true);

            if (testIsDangerActive == false) // Testweise Variable
            {
                animatorCorporate.Play("CorporateSlave");
            }

            // if (stateBarScript.isDangerActive) //in original world and danger zone begins

            else if (testIsDangerActive && !isTransitionedInverted) // Testweise Variable
            {
                isTransitionedInverted = true;
                StartCoroutine(PlayDangerAnimations("CorporateDangerTransition", "Fly")); //play transition animation from normal to monster and then keep monster state
            }
        }
    }

    private IEnumerator PlayDangerAnimations(string animation1, string animation2)
    {
        animatorCorporate.Play(animation1);
        yield return new WaitForSeconds(animatorButterfly.GetCurrentAnimatorStateInfo(0).length);
        animatorCorporate.Play(animation2);
    }
}
