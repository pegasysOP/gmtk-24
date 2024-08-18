using UnityEngine;

public abstract class TriggerObject : MonoBehaviour, ITriggerObject
{
    private Vector3 position;
    private Quaternion rotation;
    private Vector3 scale;

    private void Start()
    {
        position = transform.position;
        rotation = transform.rotation;
        scale = transform.localScale;
    }

    public virtual void DoReset()
    {
        transform.position = position;
        transform.rotation = rotation;
        transform.localScale = scale;
    }

    public abstract void Trigger();
}
