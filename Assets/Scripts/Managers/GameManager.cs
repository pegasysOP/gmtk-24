using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public Action StartGame;
    public Action EndGame;

    public AudioSystem audioSystem;

    public void Awake()
    {
        if (Instance == null)
            Instance = this;
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
