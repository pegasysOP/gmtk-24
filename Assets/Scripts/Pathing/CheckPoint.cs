using System;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    public bool hasCompleted;

    public void Complete()
    {
        hasCompleted = true;
    }
}
