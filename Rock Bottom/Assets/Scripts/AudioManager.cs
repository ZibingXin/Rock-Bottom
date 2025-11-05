using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Private member variable (example: AudioSource reference)
    [SerializeField] private AudioSource audioSource;

    // Mute the audio
    public void Mute()
    {
        if (audioSource != null)
        {
            audioSource.mute = true;
            Debug.Log("Audio muted");
        }
    }

    // Unmute the audio
    public void Unmute()
    {
        if (audioSource != null)
        {
            audioSource.mute = false;
            Debug.Log("Audio unmuted");
        }
    }

    // Change the volume (value between 0 and 1)
    public void ChangeVolume(float newVolume)
    {
        if (audioSource != null)
        {
            audioSource.volume = Mathf.Clamp01(newVolume);
            Debug.Log("Volume changed to: " + audioSource.volume);
        }
    }
}
