using UnityEngine;

public class Receptacle : MonoBehaviour
{
    public DragableObject targetDragable;
    public CheckPoint checkpoint;

    [Header("Magnetise")]
    public float magnetiseDistance = 0.5f;
    [Range(0, 360)] public float magnetiseRotation = 10f;
    public float magnetiseScale = 0.2f;
    public float magnetiseDuration = 0.5f;

    [Header("Solution")]
    [SerializeField] private Vector3 solutionPosition;
    [SerializeField] private Quaternion solutionRotation;
    [SerializeField] private Vector3 solutionScale;

    private bool completed = false;

    private void Update()
    {
        if (completed)
            return;

        float distance = (targetDragable.GetPosition() - transform.position - solutionPosition).magnitude;
        if (distance > magnetiseDistance)
            return;

        float angleBetween = Quaternion.Angle(targetDragable.GetRotation(), solutionRotation);
        if (angleBetween > magnetiseRotation)
            return;

        float scaleDif = (targetDragable.GetScale() - solutionScale).magnitude;
        if (scaleDif > magnetiseScale)
            return;

        completed = true;
        targetDragable.AnimateToSolution(solutionPosition + transform.position, solutionRotation, solutionScale, magnetiseDuration);

        checkpoint?.Complete();
        Debug.Log("CHECKPOINT COMPLETE");
    }

    public void SaveSolution()
    {
        solutionPosition = targetDragable.transform.position - transform.position;
        solutionRotation = targetDragable.transform.rotation;
        solutionScale = targetDragable.transform.localScale;
    }
}
