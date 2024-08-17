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

    public void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(gameObject.transform.position, new Vector3(1f,1f,0.2f));
    }
}
