using UnityEngine;
using System.Collections;

public class MusicController : MonoBehaviour
{
    public static MusicController Instance { get; private set; }
    
    public AudioSource audioSource1;
    public AudioSource audioSource2;
    public AudioClip[] tracks; // Array to hold the four music tracks
    public AudioClip[] soundEffects; // Array to hold the sound effects
    public AudioSource sfxSource; // AudioSource for playing sound effects

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
            audioSource1.Play();
            audioSource1.volume = 1f;
            audioSource2.Stop();
        }
        else
        {
            audioSource2.clip = tracks[trackIndex];
            audioSource2.Play();
            audioSource2.volume = 1f;
            audioSource1.Stop();
        }
        isPlayingSource1 = !isPlayingSource1;
    }

    private IEnumerator FadeToNewMusic(AudioClip newClip)
    {
        AudioSource activeSource = isPlayingSource1 ? audioSource1 : audioSource2;
        AudioSource newSource = isPlayingSource1 ? audioSource2 : audioSource1;

        newSource.clip = newClip;

        float time = 0f;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            float t = time / fadeDuration;

            activeSource.volume = Mathf.Lerp(1f, 0f, t);
            newSource.volume = Mathf.Lerp(0f, 0f, t); // Ensure newSource volume remains 0 during fade

            yield return null;
        }

        // Stop both sources briefly to ensure no overlap
        activeSource.Stop();
        newSource.Stop();

        // Start the new clip after a brief pause
        yield return new WaitForSeconds(0.1f); // Brief pause to ensure no overlap

        newSource.Play();
        time = 0f;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            float t = time / fadeDuration;

            newSource.volume = Mathf.Lerp(0f, 1f, t);

            yield return null;
        }

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
