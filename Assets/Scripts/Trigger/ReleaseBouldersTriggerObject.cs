public class ReleaseBouldersTriggerObject : TriggerObject
{
    public override void Trigger()
    {
        if (gameObject.activeInHierarchy)
            gameObject.SetActive(false);
        else 
            gameObject.SetActive(true);
    }

    public override void DoReset()
    {
        base.DoReset();

        Trigger();
    }
}