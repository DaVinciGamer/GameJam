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
        circleCollider = GetComponent<CircleCollider2D>();
        circleCollider.enabled = false;
        AIPath aiPath = GetComponentInParent<AIPath>();
        aiPath.enabled = false;
    }

    private void Update()
    {
        if (!enemyHealthScript.isDead)
        {

            if (varInvertedWorld.ToString() == "false")
            {
                isTransitionedInverted = false;
                butterflyGameObject.SetActive(true);
                corporateSlaveGameObject.SetActive(false);

                // if (VarInvertedWorld.invertedWorld == "false") //when original world is active
                if (stateBarScript.isDangerActive == false) // Testweise Variable
                {
                    animatorButterfly.Play("ButterflyEnemy");
                    AIPath aiPath = GetComponentInParent<AIPath>();
                    aiPath.enabled = false;
                    circleCollider.enabled = false;
                }

                // if (stateBarScript.isDangerActive) //in original world and danger zone begins

                else if (stateBarScript.isDangerActive && !isTransitionedOriginal) // Testweise Variable
                {
                    circleCollider.enabled = true;
                    AIPath aiPath = GetComponentInParent<AIPath>();
                    aiPath.enabled = true;

                    StartCoroutine(PlayDangerAnimations(animatorButterfly, "ButterflyDangerTransition", "Ghost")); //play transition animation from normal to monster and then keep monster state
                    isTransitionedOriginal = true;
                }
            }

            else
            {
                isTransitionedOriginal = false;
                butterflyGameObject.SetActive(false);
                corporateSlaveGameObject.SetActive(true);

                if (stateBarScript.isDangerActive == false) // Testweise Variable
                {
                    animatorCorporate.Play("CorporateSlave");
                    AIPath aiPath = GetComponentInParent<AIPath>();
                    aiPath.enabled = false;
                    circleCollider.enabled = false;
                }

                // if (stateBarScript.isDangerActive) //in original world and danger zone begins

                else if (stateBarScript.isDangerActive && !isTransitionedInverted) // Testweise Variable
                {
                    circleCollider.enabled = true;
                    AIPath aiPath = GetComponentInParent<AIPath>();
                    aiPath.enabled = true;
                    StartCoroutine(PlayDangerAnimations(animatorCorporate, "CorporateDangerTransition", "Fly")); //play transition animation from normal to monster and then keep monster state
                    isTransitionedInverted = true;
                }
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
