using System;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public Action StartGame;
    public Action EndGame;

    public MouseDrag mouseDrag;
    public AudioSystem audioSystem;
    public CameraMan cameraMan;

    [SerializeField]
    private Transform cameraRotateAround;

    public void Awake()
    {
        if (Instance == null)
            Instance = this;

        CameraMan.OnCameraTransition += OncameraTransition;
    }

    public void OnDestroy()
    {
        CameraMan.OnCameraTransition -= OncameraTransition;
    }

    private void OncameraTransition(string newCam, string oldCam)
    {
        if (newCam == cameraMan.gameVirtualCamera.name && oldCam == cameraMan.flyThroughVirtualCamera.name)
        {
            // wait 2 seconds to start the game as could not find when a blend/cut event for virtual camera is done
            StartCoroutine(DelayedGameStart(2));
        }
        else
        {
            Debug.Log($"no logic for {newCam} from {oldCam}");
        }
    }

    IEnumerator DelayedGameStart(float duration)
    {
        float time = 0;
        while (time < duration)
        {
            time += Time.deltaTime;
            yield return null;
        }

        FireStartGameEvent();
    }

    public Transform GetCameraRotatePoint()
    {
        return cameraRotateAround; 
    }

    private void Start()
    {
        audioSystem.PlayMusic();
    }

    public void FireStartGameEvent()
    {
        StartGame?.Invoke();
    }

    public void FireEndGameEvent()
    {
        EndGame?.Invoke();
    }
}
