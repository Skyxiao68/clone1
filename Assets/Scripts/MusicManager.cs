using UnityEngine;
using UnityEngine.UI;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    public Slider volumeSlider;

    public AudioSource backgroundMusic;

    private void Awake()
    {
        // Singleton
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Load saved volume
        float volume = PlayerPrefs.GetFloat("MasterVolume", 1f);

        // Set game volume
        AudioListener.volume = volume;

        // Set slider position
        volumeSlider.value = volume;

        // Play music if it isn't already playing
        /*if (!backgroundMusic.isPlaying)
        {
            backgroundMusic.Play();
        } */
    }

    public void ChangeVolume(float volume)
    {
        // Change ALL game sounds
        AudioListener.volume = volume;

        // Save it
        PlayerPrefs.SetFloat("MasterVolume", volume);
        PlayerPrefs.Save();

    }

    public void ToggleMute()
    {
        if (AudioListener.volume > 0)
        {
            AudioListener.volume = 0;
            volumeSlider.value = 0;
        }
        else
        {
            AudioListener.volume = 1;
            volumeSlider.value = 1;
        }
    }

    public void StopMusic()
    {
        backgroundMusic.Stop();
    }

    public void PlayMusic()
    {
        if (!backgroundMusic.isPlaying)
            backgroundMusic.Play();
    }

}