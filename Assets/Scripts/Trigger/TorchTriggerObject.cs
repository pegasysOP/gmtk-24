using UnityEngine;

public class TorchTriggerObject : TriggerObject
{
    public Light lightToSwitch;
    public bool startOn;
    private void Start()
    {
        if (startOn)
        {
            lightToSwitch.enabled = true;
        }
        else
        {
            lightToSwitch.enabled = false;
        }
    }
    public override void Trigger()
    {
        if (lightToSwitch.enabled)
        {
        lightToSwitch.enabled = false;
        }
        else
        {
            lightToSwitch.enabled = true;
        }
    }
}
