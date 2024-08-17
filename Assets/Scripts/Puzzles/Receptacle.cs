using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Solution
{
    [SerializeField] public Vector3 Position;
    [SerializeField] public Quaternion Rotation;
    [SerializeField] public Vector3 Scale;
}

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
    [SerializeField] private List<Solution> solutions;

    //[SerializeField] private Vector3 solutionPosition;
    //[SerializeField] private Quaternion solutionRotation;
    //[SerializeField] private Vector3 solutionScale;

    private bool completed = false;

    private void Update()
    {
        if (completed)
            return;

        foreach (Solution solution in solutions)
        {
            float distance = (targetDragable.GetPosition() - transform.position - solution.Position).magnitude;
            if (distance > magnetiseDistance)
                continue;

            float angleBetween = Quaternion.Angle(targetDragable.GetRotation(), solution.Rotation);
            if (angleBetween > magnetiseRotation)
                continue;

            float scaleDif = (targetDragable.GetScale() - solution.Scale).magnitude;
            if (scaleDif > magnetiseScale)
                continue;

            completed = true;
            targetDragable.AnimateToSolution(solution.Position + transform.position, solution.Rotation, solution.Scale, magnetiseDuration);

            checkpoint?.Complete();
            Debug.Log("CHECKPOINT COMPLETE");

            break;
        }
    }

    public void ClearSolutions()
    {
        solutions.Clear();
    }

    public void AddSolution()
    {
        Solution newSolution = new Solution();

        newSolution.Position = targetDragable.transform.position - transform.position;
        newSolution.Rotation = targetDragable.transform.rotation;
        newSolution.Scale = targetDragable.transform.localScale;

        solutions.Add(newSolution);
    }
}
