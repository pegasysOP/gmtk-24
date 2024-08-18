using System;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    public Action OnComplete;
    public bool hasCompleted;

    public List<DraggableObject> draggableObjects = new List<DraggableObject>();
    public List<TriggerCollider> triggerColliders = new List<TriggerCollider>();

    public void Complete()
    {
        hasCompleted = true;
        OnComplete?.Invoke();
    }

    public void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(gameObject.transform.position, new Vector3(2f, 1, 0.2f));
    }

    public void DoReset()
    {
        foreach (DraggableObject draggableObject in draggableObjects) 
        { 
            draggableObject.DoReset();
        }

        foreach (TriggerCollider triggerCollider in triggerColliders)
        {
            triggerCollider.DoReset();
        }
    }
}
