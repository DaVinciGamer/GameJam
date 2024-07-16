using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pauseMenu : MonoBehaviour
{
    public GameObject menuOverlay;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (menuOverlay.activeSelf)
            {
                closeMenu();
            }
            else
            {
                openMenu();
            }
        }
    }
    public void openMenu()
    {
        menuOverlay.SetActive(true);
    }

    public void closeMenu()
    {
        menuOverlay.SetActive(false);
    }

}
