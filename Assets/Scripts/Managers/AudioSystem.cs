using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioSystem : MonoBehaviour
{
    public AudioMixerGroup dragMixer;
    public AudioMixerGroup wizardMixer;

    [Header("Music")]
    public AudioSource musicSource;

    [Header("Drag")]
    public GameObject dragSourceObject;
    private List<AudioSource> audioSources = new List<AudioSource>();
    public DraggableAudioClips dragAudioClips;
    public bool isDragging;

    [Header("Wizard")]
    public GameObject wizardSourceObject;
    private List<AudioSource> wizardSources = new List<AudioSource>();
    public WizardAudioClips wizardAudioClips;
    public float footStepTimer;
    public bool isWalking;

    private void Start()
    {
        for (int i = 0; i < 10; i++) 
        {
            AudioSource dragSource = dragSourceObject.AddComponent<AudioSource>();
            dragSource.outputAudioMixerGroup = dragMixer;
            
            audioSources.Add(dragSource);

            AudioSource wizardSource = wizardSourceObject.AddComponent<AudioSource>();
            wizardSource.outputAudioMixerGroup = wizardMixer;
            wizardSource.loop = false;
            wizardSource.playOnAwake = false;
            wizardSources.Add(wizardSource);
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
        foreach (AudioSource source in audioSources)
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
        foreach (AudioSource source in audioSources)
        {
            if (source != null && !source.isPlaying) 
            {
                source.clip = dragAudioClips.dragDrop;
                source.Play();
                break;
            }
        }
    }

    public void StartWalking()
    {
        isWalking = true;
        StartCoroutine(WalkingCoroutine());
        Debug.Log($"Start walking called");
    }

    public void StopWalking()
    {
        isWalking = false;
        StopCoroutine(WalkingCoroutine());
        Debug.Log($"Stop walking called");
    }

    private IEnumerator WalkingCoroutine()
    {
        while (isWalking)
        {
            PlayWizardFootstep();
            yield return new WaitForSeconds(footStepTimer);
        }
    }

    private void PlayWizardFootstep()
    {
        foreach (AudioSource source in wizardSources)
        {
            if (source != null && !source.isPlaying)
            {
                source.clip = wizardAudioClips.footStepStone;
                source.pitch = Random.Range(0.8f, 1.2f);
                source.Play();
                break;
            }
        }
    }

    public void StartAngry()
    {
        StartCoroutine(AngryCoroutine());
    }

    public void StopAngry()
    { 
        StopCoroutine(AngryCoroutine());
    }

    private IEnumerator AngryCoroutine()
    {
        foreach(AudioSource source in wizardSources)
        {
            if (source != null && !source.isPlaying)
            {
                source.clip = wizardAudioClips.wizardBaloon;
            }
        }
        yield return null;
    }

    public void PlayWizardStaffBonk()
    {
        foreach (AudioSource source in wizardSources)
        {
            if (source != null && !source.isPlaying)
            {
                source.clip = wizardAudioClips.wizardStaffDonk;
                source.Play();
                break;
            }
        }
    }
}
