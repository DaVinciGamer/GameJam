using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class VarInvertedWorld : MonoBehaviour
{
    public static string invertedWorld = "true"; // True = Adult-World, False = Child-World
    public InputAction invertWorldAction; // InputAction for the P key
    public string invertedWorldNonStatic = invertedWorld;
    private void Update()
    {
        invertedWorldNonStatic = invertedWorld;
    }

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
        invertedWorld = invertedWorld == "true" ? "false" : "true";
        invertedWorldNonStatic = invertedWorld;

        Debug.Log("VarInvertedWorld set to " + invertedWorld);
    }
}
