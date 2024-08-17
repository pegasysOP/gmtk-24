using System.Collections.Generic;
using UnityEngine;

public class AudioSystem : MonoBehaviour
{
    [Header("Music")]
    public AudioSource musicSource;

    [Header("Drag")]
    public GameObject dragSourceObject;
    private List<AudioSource> dragSources = new List<AudioSource>();

    public DraggableAudioClips dragAudioClips;


    private void Start()
    {
        for (int i = 0; i < 10; i++) 
        {
            dragSources.Add(dragSourceObject.AddComponent<AudioSource>());
        }
    }
    public void PlayMusic()
    {
        if (musicSource == null || musicSource.isPlaying)
            return;

        musicSource.Play();
    }


    public void PlayDragSelected()
    {
        foreach (AudioSource source in dragSources)
        {
            if (source != null && !source.isPlaying)
            {
                source.clip = dragAudioClips.dragSelect;
                source.Play();
                break;
            }
        }
    }

    public void PlayDragDeselected()
    {
        foreach (AudioSource source in dragSources)
        {
            if (source != null && !source.isPlaying) 
            {
                source.clip = dragAudioClips.dragDrop;
                source.Play();
                break;
            }
        }
    }
}
