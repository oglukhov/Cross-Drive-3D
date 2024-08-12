using Unity.VisualScripting;
using UnityEngine;

public class BackgroundSound : MonoBehaviour
{
    public AudioSource backgroundMusic;

    private bool isMuted;

    void Start()
    {
        if(PlayerPrefs.GetString("Music") != "Off")
            {isMuted = false;
            backgroundMusic.volume = isMuted ? 0f : 0.7f;}
        if(PlayerPrefs.GetString("Music") == "Off")
            {isMuted = true;
            backgroundMusic.volume = isMuted ? 0f : 0.7f;}
        // Ensure AudioManager persists across scene changes

        // Optionally, play background music when the scene starts
        PlayBackgroundMusic();
    }

    public void PlayBackgroundMusic()
    {
        if (!backgroundMusic.isPlaying)
        {
            backgroundMusic.Play();
        }
    }

    public void ToggleMute()
    {
        isMuted = !isMuted;
        backgroundMusic.volume = isMuted ? 0f : 0.7f;
    }
}
