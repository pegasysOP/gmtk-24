using System.Collections;
using UnityEngine;

public class TorchTriggerObject : TriggerObject
{
    public Light lightToSwitch;
    public bool startOn;

    public bool switchOffAfterTime;
    public float lightSwitchTime;

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
        if (switchOffAfterTime && lightSwitchTime > 0)
        {
            StartCoroutine(SwitchLight());
        }
        else
        {
            Switch();
        }
    }

    private IEnumerator SwitchLight()
    {
        Switch();
        yield return new WaitForSeconds(lightSwitchTime);
        Switch();
    }

    private void Switch()
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

    public override void DoReset()
    {
        base.DoReset();

        if (startOn)
        {
            lightToSwitch.enabled = true;
        }
        else
        {
            lightToSwitch.enabled = false;
        }
    }
}
