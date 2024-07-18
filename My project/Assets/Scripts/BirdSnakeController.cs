using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdSnakeController : MonoBehaviour
{
    private Animator animatorBird;
    private Animator animatorSnake;
    private bool isTransitionedOriginal = false;
    private bool isTransitionedInverted = false;

    private StateBar stateBarScript;
    private VarInvertedWorld varInvertedWorld;
    private EnemyHealth enemyHealthScript;

    private GameObject birdGameObject;
    private GameObject corporateSlaveGameObject;

    CircleCollider2D circleCollider;


    private void Start()
    {
        birdGameObject = transform.Find("Bird").gameObject;
        corporateSlaveGameObject = transform.Find("CorporateSlave").gameObject;
        animatorBird = birdGameObject.GetComponent<Animator>();
        animatorSnake = corporateSlaveGameObject.GetComponent<Animator>();
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
        if (!enemyHealthScript.isDead) //if the enemy is not dead yet
        {

            if (varInvertedWorld.invertedWorldNonStatic == "false") //if the player is in the original world
            {
                isTransitionedInverted = false; //reset the bool for the inverted state so that it can tansition again when worlds are changed
                birdGameObject.SetActive(true);
                corporateSlaveGameObject.SetActive(false);
                //Debug.Log(stateBarScript.isDangerActive);
                yield return new WaitForSeconds(0.5f);
                if (stateBarScript.isDangerActive == false) //player has not yet reached the danger zone
                {
                    //Debug.Log("Original normal zone");
                    animatorBird.Play("Bird"); //play idle animation from enemies
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
                    StartCoroutine(PlayDangerAnimations(animatorBird, "BirdDangerTransition", "Snake")); //play transition animation from normal to monster and then keep monster state
                    isTransitionedOriginal = true; //set flag that the enemy has already transitioned
                }
            }

            else //if the player is in the inverted world
            {
                isTransitionedOriginal = false; //reset the bool for the original state so that it can tansition again when worlds are changed
                birdGameObject.SetActive(false);
                corporateSlaveGameObject.SetActive(true);
                yield return new WaitForSeconds(0.5f);
                if (stateBarScript.isDangerActive == false) //player has not yet reached the danger zone
                {
                    animatorSnake.Play("CorporateSlave"); //play idle animation from enemies
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
                    StartCoroutine(PlayDangerAnimations(animatorSnake, "CorporateDangerTransition", "Fly")); //play transition animation from normal to monster and then keep monster state
                    isTransitionedInverted = true; //set flag that the enemy has already transitioned
                }
            }
        }
    }
}
