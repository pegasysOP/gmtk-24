using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public Action StartGame;

    public void Awake()
    {
        if (Instance == null)
            Instance = this;
    }


}
