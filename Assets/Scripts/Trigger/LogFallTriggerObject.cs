using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class LogFallTriggerObject : TriggerObject
{
    public override void Trigger()
    {
        Debug.Log($"log triggered");
        GetComponent<Rigidbody>().AddRelativeTorque(Vector3.right * 1000f);
    }
}
