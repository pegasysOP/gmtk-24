using System.Collections;
using UnityEngine;

public class TorchTriggerObject : TriggerObject
{
    public GameObject lightToSwitch;
    public bool startOn;

    public bool switchOffAfterTime;
    public float lightSwitchTime;

    private void Start()
    {
        if (startOn)
        {
            lightToSwitch.SetActive(true);
        }
        else
        {
            lightToSwitch.SetActive(false);
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
        if (lightToSwitch.activeSelf)
        {
            lightToSwitch.SetActive(false);
        }
        else
        {
            lightToSwitch.SetActive(true);
        }
    }

    public override void DoReset()
    {
        base.DoReset();

        if (startOn)
        {
            lightToSwitch.SetActive(true);
        }
        else
        {
            lightToSwitch.SetActive(false);
        }
    }
}
