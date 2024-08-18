using DG.Tweening;
using UnityEngine;

public class DrawBridgeTriggerObject : TriggerObject
{
    public Transform bridgeRotate;
    public Quaternion bridgeStartRotation;


    private void Start()
    {
        bridgeStartRotation = bridgeRotate.rotation;
    }

    public override void Trigger()
    {
        bridgeRotate.DORotate(new Vector3(0, 0, 0), 2f);
    }

    public override void DoReset()
    {
        base.DoReset();

        bridgeRotate.rotation = bridgeStartRotation;
    }
}
