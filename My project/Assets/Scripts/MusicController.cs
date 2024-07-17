using UnityEngine;
using System.Collections;

public class MusicController : MonoBehaviour
{
    public static MusicController Instance { get; private set; }
    public AudioSource audioSource1;
    public AudioSource audioSource2;
    private bool isPlayingSource1 = true;
    private Coroutine fadeCoroutine;

    public float fadeDuration = 1.0f; // Duration of the fade in seconds

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Ensure both audio sources loop
        audioSource1.loop = true;
        audioSource2.loop = true;
    }

    public void PlayMusicLoop(AudioClip newClip)
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }
        fadeCoroutine = StartCoroutine(FadeToNewMusic(newClip));
    }

    private IEnumerator FadeToNewMusic(AudioClip newClip)
    {
        AudioSource activeSource = isPlayingSource1 ? audioSource1 : audioSource2;
        AudioSource newSource = isPlayingSource1 ? audioSource2 : audioSource1;

        newSource.clip = newClip;
        newSource.Play();

        float time = 0f;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            float t = time / fadeDuration;

            activeSource.volume = Mathf.Lerp(1f, 0f, t);
            newSource.volume = Mathf.Lerp(0f, 1f, t);

            yield return null;
        }

        activeSource.Stop();
        isPlayingSource1 = !isPlayingSource1;
    }
}
