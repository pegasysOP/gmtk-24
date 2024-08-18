using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioSystem : MonoBehaviour
{
    public AudioMixer mainMix;

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

    [Header("Wind")]
    public AudioSource lowWind;
    public AudioSource highWind;

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
        Debug.Log($"Angry audio called");
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
                source.Play();
                break;
            }
        }
        yield return null;
    }

    public void PlayWizardPop()
    {
        foreach (AudioSource source in wizardSources)
        {
            if (source != null && !source.isPlaying)
            {
                source.clip = wizardAudioClips.wizardPop;
                source.Play();
                break;
            }
        }
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

    public void PlayDraggableCollide()
    {
        foreach (AudioSource source in audioSources)
        {
            if (source != null && !source.isPlaying)
            {
                source.clip = dragAudioClips.dragCollision;
                source.Play();
                break;
            }
        }
    }

    float map(float x, float in_min, float in_max, float out_min, float out_max)
    {
        return (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
    }

    public void SetWindVolume(float elapsed, float total)
    {
        float volume = map(elapsed, 0, total, -25f, 0);

        Debug.Log($"{volume}");
        mainMix.SetFloat("WindVolume", volume);
    }

    public void PlayWind()
    {
        lowWind.Play();
        highWind.Play();
    }
}
