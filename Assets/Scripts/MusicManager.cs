using UnityEngine;
using UnityEngine.UI;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    public Slider volumeSlider;

    public AudioSource backgroundMusic;

    public AudioSource sfxSource;

    [Header("Sound Effects")]
    public AudioClip attackSound;
    public AudioClip hurtSound;

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
       // playerPrefs.DeleteAll();
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

    /* void update
     {
         Debug.Log(AudioListner.volume);
     }
    */

    public void PlaySound(AudioClip clip)
    {
        if (clip != null)
        {
            sfxSource.PlayOneShot(clip);
        }
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