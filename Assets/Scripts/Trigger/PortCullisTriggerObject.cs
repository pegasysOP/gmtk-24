using DG.Tweening;
using UnityEngine;

public class PortCullisTriggerObject : TriggerObject
{
    public Transform portCullis;

    private Vector3 startPos;

    private void Start()
    {
        startPos = portCullis.position;
    }

    public override void Trigger()
    {
        portCullis.DOMoveY(startPos.y + 3f, 1f);
    }
}
