using UnityEngine;

public class AudioSystem : MonoBehaviour
{
    public AudioSource musicSource;

    public void PlayMusic()
    {
        musicSource.Play();
    }
}
