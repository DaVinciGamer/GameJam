using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pauseMenu : MonoBehaviour
{
    public GameObject menuOverlay;
    public void openMenu()
    {
        menuOverlay.SetActive(true);
    }

    public void closeMenu()
    {
        menuOverlay.SetActive(false);
    }

}
