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
    public DraggableObject targetDraggable;
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

        if (targetDraggable == null)
            return;

        foreach (Solution solution in solutions)
        {
            float distance = (targetDraggable.GetPosition() - transform.position - solution.Position).magnitude;
            if (distance > magnetiseDistance)
                continue;

            float angleBetween = Quaternion.Angle(targetDraggable.GetRotation(), solution.Rotation);
            if (angleBetween > magnetiseRotation)
                continue;

            float scaleDif = (targetDraggable.GetScale() - solution.Scale).magnitude;
            if (scaleDif > magnetiseScale)
                continue;

            completed = true;
            targetDraggable.AnimateToSolution(solution.Position + transform.position, solution.Rotation, solution.Scale, magnetiseDuration);

            checkpoint?.Complete();
            Debug.Log("CHECKPOINT COMPLETE");

            break;
        }
    }

    public void DoReset()
    {
        completed = false;

        targetDraggable.DoReset();
    }

    public void ClearSolutions()
    {
        solutions.Clear();
    }

    public void AddSolution()
    {
        Solution newSolution = new Solution();

        newSolution.Position = targetDraggable.transform.position - transform.position;
        newSolution.Rotation = targetDraggable.transform.rotation;
        newSolution.Scale = targetDraggable.transform.localScale;

        solutions.Add(newSolution);
    }

    public bool IsCompleted()
    {
        return completed;
    }    
}
