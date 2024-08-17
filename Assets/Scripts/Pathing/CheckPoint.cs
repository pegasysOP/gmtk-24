using System;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    public Action OnComplete;
    public bool hasCompleted;

    public void Complete()
    {
        hasCompleted = true;
        OnComplete?.Invoke();
    }
}
