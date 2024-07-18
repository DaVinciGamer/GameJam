using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private MusicController MusicController;
    public void PlayGame()
    {
        MusicController.Instance.PlaySoundEffect(1);
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        MusicController.Instance.PlaySoundEffect(0);
        Application.Quit();
    }
}
