using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class Intro : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    private MusicController MusicController;
    public string nextSceneName;

    void Start()
    {
        videoPlayer.loopPointReached += OnVideoEnd; // Idea from ChatGPT
        MusicController.Instance.FadeTo(7);
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        SceneManager.LoadScene(nextSceneName);
    }
}

