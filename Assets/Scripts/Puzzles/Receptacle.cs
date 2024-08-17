using UnityEngine;

public class Receptacle : MonoBehaviour
{
    public DragableObject targetDragable;
    public CheckPoint checkpoint;

    public float magnetiseDistance = 0.5f;
    public float magnetiseRotation = 10f;
    public float magnetiseScale = 0.2f;

    [SerializeField] private Vector3 solutionPosition;
    [SerializeField] private Quaternion solutionRotation;
    [SerializeField] private Vector3 solutionScale;
    private bool completed = false;

    private void Update()
    {
        if (completed)
            return;

        float distance = (targetDragable.GetPosition() - solutionPosition).magnitude;
        if (distance > magnetiseDistance)
            return;

        float angleBetween = Quaternion.Angle(targetDragable.GetRotation(), solutionRotation);
        if (angleBetween > magnetiseRotation)
            return;

        float scaleDif = (targetDragable.GetScale() - solutionScale).magnitude;
        if (scaleDif > magnetiseScale)
            return;

        completed = true;
        targetDragable.SetPosition(solutionPosition);
        targetDragable.SetRotation(solutionRotation);
        targetDragable.SetScale(solutionScale);
        targetDragable.Lock(true);

        //checkpoint.Complete();
        Debug.Log("CHECKPOINT COMPLETE");
    }

    public void SaveSolution()
    {
        solutionPosition = targetDragable.transform.position;
        solutionRotation = targetDragable.transform.rotation;
        solutionScale = targetDragable.transform.localScale;
    }
}
