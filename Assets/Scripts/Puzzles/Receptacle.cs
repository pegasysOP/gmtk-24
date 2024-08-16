using UnityEngine;

public class Receptacle : MonoBehaviour
{
    public DragableObject targetDragable;
    public CheckPoint checkpoint;

    public float magnetiseDistance = 0.5f;

    private bool completed = false;

    private void Update()
    {
        if (completed)
            return;

        float distance = (targetDragable.GetPosition() - transform.position).magnitude;
        if (distance < magnetiseDistance)
        {
            completed = true;
            targetDragable.SetPosition(transform.position);
            //checkpoint.Complete();
            Debug.Log("CHECKPOINT COMPLETE");
        }
    }
}
