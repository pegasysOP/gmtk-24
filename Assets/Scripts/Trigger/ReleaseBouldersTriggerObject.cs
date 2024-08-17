public class ReleaseBouldersTriggerObject : TriggerObject
{
    public override void Trigger()
    {
        Destroy(gameObject);
    }
}