using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButterflyGhostController : MonoBehaviour
{
    private Animator animator;
    // private StateBar stateBarScript;
    // private VarInvertedWorld varInvertedWorld;

    // Testweise im Inspector hinzugefügte Variablen
    public bool testIsDangerActive;
    public string testInvertedWorld;

    private void Start()
    {
        // Automatisches Finden der Referenzen
        // stateBarScript = FindObjectOfType<StateBar>();
        // if (stateBarScript == null)
        // {
        //     Debug.LogError("StateBar script not found in the scene.");
        // }

        // varInvertedWorld = FindObjectOfType<VarInvertedWorld>();
        // if (varInvertedWorld == null)
        // {
        //     Debug.LogError("VarInvertedWorld script not found in the scene.");
        // }

        animator = GetComponentInChildren<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator not found in child objects.");
        }
    }

    private void Update()
    {
        // Check if all required components are initialized
        if (animator == null) // || stateBarScript == null || varInvertedWorld == null
        {
            return;
        }

        // if (VarInvertedWorld.invertedWorld == "false") //when original world is active
        if (testInvertedWorld == "false") // Testweise Variable
        {
            PlayButterflyNormalAnimation();
        }

        // if (stateBarScript.isDangerActive) //in original world and danger zone begins
        if (testIsDangerActive) // Testweise Variable
        {
            StartCoroutine(PlayDangerAnimations()); //play transition animation from normal to monster and then keep monster state
        }
        else
        {
            StartCoroutine(PlaySafeAnimations()); //danger zone no longer active so transition back to normal state and stay there
        }
    }

    private void PlayButterflyNormalAnimation()
    {
        animator.Play("Butterfly");
    }

    private IEnumerator PlayDangerAnimations()
    {
        animator.Play("ButterflyDangerTransition");
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        animator.Play("Ghost");
    }

    private IEnumerator PlaySafeAnimations()
    {
        animator.Play("GhostSafeTransition");
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        animator.Play("Butterfly");
    }
}
