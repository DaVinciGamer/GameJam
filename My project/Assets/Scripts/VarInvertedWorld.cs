using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class VarInvertedWorld : MonoBehaviour
{
    public static string invertedWorld = "true"; // True = Adult-World, False = Child-World
    public InputAction invertWorldAction; // InputAction for the P key

    void OnEnable()
    {
        invertWorldAction.Enable();
        invertWorldAction.performed += OnInvertWorld;
    }

    void OnDisable()
    {
        invertWorldAction.performed -= OnInvertWorld;
        invertWorldAction.Disable();
    }

    private void OnInvertWorld(InputAction.CallbackContext context)
    {
        if (invertedWorld == "true")
        {
            invertedWorld = "false";
        }
        else
        {
            invertedWorld = "true";
        }
        Debug.Log("VarInvertedWorld set to " + invertedWorld);
    }
}
