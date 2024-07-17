using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pauseMenu : MonoBehaviour
{
    public GameObject menuOverlay;
    private MusicController MusicController;

void Start(){
    MusicController.Instance.FadeTo(1);
}
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
        Time.timeScale = 0;
        menuOverlay.SetActive(true);
    }

    public void closeMenu()
    {
        Time.timeScale = 1;
        menuOverlay.SetActive(false);
    }

}
