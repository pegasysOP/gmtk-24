using System.Collections.Generic;
using UnityEngine;

public class TriggerCollider : MonoBehaviour, ITriggerObject
{
    public List<TriggerObject> objectsToTrigger;

    public void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(gameObject.transform.position, transform.GetComponent<BoxCollider>().size);
    }

    public void Trigger()
    {
        foreach (TriggerObject triggerObject in objectsToTrigger)
        {
            triggerObject.Trigger();
        }
    }
}
