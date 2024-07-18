using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButterflyGhostController : MonoBehaviour
{
    private Animator animatorButterfly;
    private Animator animatorCorporate;
    private bool isTransitionedOriginal = false;
    private bool isTransitionedInverted = false;

    private StateBar stateBarScript;
    private VarInvertedWorld varInvertedWorld;
    private EnemyHealth enemyHealthScript;

    // Testweise im Inspector hinzugefügte Variablen
    //public bool testIsDangerActive;
    //public string testInvertedWorld;

    private GameObject butterflyGameObject;
    private GameObject corporateSlaveGameObject;

    CircleCollider2D circleCollider;


    private void Start()
    {
        butterflyGameObject = transform.Find("Butterfly").gameObject;
        corporateSlaveGameObject = transform.Find("CorporateSlave").gameObject;
        animatorButterfly = butterflyGameObject.GetComponent<Animator>();
        animatorCorporate = corporateSlaveGameObject.GetComponent<Animator>();
        enemyHealthScript = FindAnyObjectByType<EnemyHealth>();
        stateBarScript = FindAnyObjectByType<StateBar>();
        varInvertedWorld = FindAnyObjectByType<VarInvertedWorld>();
        circleCollider = GetComponent<CircleCollider2D>();
        circleCollider.enabled = false;
        AIPath aiPath = GetComponentInParent<AIPath>();
        aiPath.enabled = false;
    }

    private void Update()
    {
        StartCoroutine(waitUpdate());
    }

    private IEnumerator PlayDangerAnimations(Animator animator, string animation1, string animation2)
    {
        animator.Play(animation1);
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        animator.Play(animation2);
    }

    private IEnumerator waitUpdate()
    {
        //Debug.Log("isTransitionedInverted "+ isTransitionedInverted);
        //Debug.Log("isTransitionedOriginal "+isTransitionedOriginal);
        //Debug.Log("invertedWorld " + varInvertedWorld.invertedWorldNonStatic);
        if (!enemyHealthScript.isDead) //if the enemy is not dead yet
        {

            if (varInvertedWorld.invertedWorldNonStatic == "false") //if the player is in the original world
            {
                isTransitionedInverted = false; //reset the bool for the inverted state so that it can tansition again when worlds are changed
                butterflyGameObject.SetActive(true);
                corporateSlaveGameObject.SetActive(false);
                Debug.Log(stateBarScript.isDangerActive);
                yield return new WaitForSeconds(0.5f);
                if (stateBarScript.isDangerActive == false) //player has not yet reached the danger zone
                {
                    //Debug.Log("Original normal zone");
                    animatorButterfly.Play("ButterflyEnemy"); //play idle animation from enemies
                    AIPath aiPath = GetComponentInParent<AIPath>();
                    aiPath.enabled = false; //don't chase the player yet
                    circleCollider.enabled = false;
                }

                //when player hits danger zone and enemy has not yet transitioned to it's monster state
                else if (stateBarScript.isDangerActive && !isTransitionedOriginal)
                {
                    circleCollider.enabled = true;
                    AIPath aiPath = GetComponentInParent<AIPath>();
                    aiPath.enabled = true; //start chasing the player

                    //play transition animation to monster state
                    StartCoroutine(PlayDangerAnimations(animatorButterfly, "ButterflyDangerTransition", "Ghost")); //play transition animation from normal to monster and then keep monster state
                    isTransitionedOriginal = true; //set flag that the enemy has already transitioned
                }
            }

            else //if the player is in the inverted world
            {
                isTransitionedOriginal = false; //reset the bool for the original state so that it can tansition again when worlds are changed
                butterflyGameObject.SetActive(false);
                corporateSlaveGameObject.SetActive(true);
                yield return new WaitForSeconds(0.5f);
                if (stateBarScript.isDangerActive == false) //player has not yet reached the danger zone
                {
                    animatorCorporate.Play("CorporateSlave"); //play idle animation from enemies
                    AIPath aiPath = GetComponentInParent<AIPath>();
                    aiPath.enabled = false; //don't chase the player yet
                    circleCollider.enabled = false;
                }

                //when player hits danger zone and enemy has not yet transitioned to it's monster state
                else if (stateBarScript.isDangerActive && !isTransitionedInverted)
                {
                    circleCollider.enabled = true;
                    AIPath aiPath = GetComponentInParent<AIPath>();
                    aiPath.enabled = true;

                    //play transition animation to monster state
                    StartCoroutine(PlayDangerAnimations(animatorCorporate, "CorporateDangerTransition", "Fly")); //play transition animation from normal to monster and then keep monster state
                    isTransitionedInverted = true; //set flag that the enemy has already transitioned
                }
            }
        }
    }
}
