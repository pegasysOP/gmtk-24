using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class CheckPoint : MonoBehaviour
{
    [Header("Collider")]
    public BoxCollider boxCollider;
    public bool showBoundsInEditor = true;

    [Header("Properties")]
    public Action OnComplete;
    public bool hasCompleted;

    public List<Receptacle> receptacles = new List<Receptacle>();
    public List<DraggableObject> draggableObjects = new List<DraggableObject>();
    public List<TriggerCollider> triggerColliders = new List<TriggerCollider>();
    public List<TriggerObject> triggerObjects = new List<TriggerObject>();

    public void Complete()
    {
        hasCompleted = true;
        OnComplete?.Invoke();
    }

    public void OnDrawGizmos()
    {
        if (!showBoundsInEditor)
            return;

        Matrix4x4 matrix = Gizmos.matrix;
        Color color = Gizmos.color;

        Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);
        Gizmos.color = Color.blue;

        Gizmos.DrawWireCube(boxCollider.center, boxCollider.size);

        Gizmos.matrix = matrix;
        Gizmos.color = color;
    }

    public void DoReset()
    {
        foreach (Receptacle receptacle in receptacles)
        {
            receptacle.DoReset();
        }

        foreach (DraggableObject draggableObject in draggableObjects) 
        { 
            draggableObject.DoReset();
        }

        foreach (TriggerCollider triggerCollider in triggerColliders)
        {
            triggerCollider.DoReset();
        }

        foreach (TriggerObject triggerObject in triggerObjects)
        {
            triggerObject.DoReset();
        }
    }
}
