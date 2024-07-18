using UnityEngine;
using System.Collections;

public class MusicController : MonoBehaviour
{
    public static MusicController Instance { get; private set; }
    
    public AudioSource audioSource1;
    public AudioSource audioSource2;
    public AudioSource sfxSource;

    public AudioClip[] tracks;
    public AudioClip[] soundEffects;
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

        // Ensure the sfxSource does not loop
        if (sfxSource != null)
        {
            sfxSource.loop = false;
        }
    }

    private void Start()
    {
        // Play the first track automatically in the first scene
        PlayMusicLoop(0);
    }

    public void FadeTo(int trackIndex)
    {
        if (trackIndex < 0 || trackIndex >= tracks.Length)
        {
            Debug.LogWarning("Invalid track index");
            return;
        }

        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }
        fadeCoroutine = StartCoroutine(FadeToNewMusic(tracks[trackIndex]));
    }

    private void PlayMusicLoop(int trackIndex)
    {
        if (trackIndex < 0 || trackIndex >= tracks.Length)
        {
            Debug.LogWarning("Invalid track index");
            return;
        }

        if (isPlayingSource1)
        {
            audioSource1.clip = tracks[trackIndex];
            audioSource1.volume = 1f;
            audioSource1.Play();
            audioSource2.Stop();
        }
        else
        {
            audioSource2.clip = tracks[trackIndex];
            audioSource2.volume = 1f;
            audioSource2.Play();
            audioSource1.Stop();
        }
        isPlayingSource1 = !isPlayingSource1;
    }

    private IEnumerator FadeToNewMusic(AudioClip newClip)
    {
        AudioSource activeSource = isPlayingSource1 ? audioSource1 : audioSource2;
        AudioSource newSource = isPlayingSource1 ? audioSource2 : audioSource1;

        newSource.clip = newClip;
        newSource.volume = 0f;
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
        activeSource.volume = 1f; // Reset volume for future use
        isPlayingSource1 = !isPlayingSource1;
    }

    // Method to play a sound effect by its index
    public void PlaySoundEffect(int soundIndex)
    {
        if (soundIndex < 0 || soundIndex >= soundEffects.Length)
        {
            Debug.LogWarning("Invalid sound effect index");
            return;
        }

        if (sfxSource != null)
        {
            sfxSource.clip = soundEffects[soundIndex];
            sfxSource.Play();
        }
    }
}
