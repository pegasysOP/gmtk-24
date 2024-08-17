using UnityEngine;
using UnityEngine.Events;

public interface IDraggable
{
    public void SetSelected(bool selected);
    public void Lock(bool locked);
    public bool IsLocked();
    public void Reset();
    public UnityEvent OnLocked { get; }

    public Vector3 GetPosition();
    public Quaternion GetRotation();
    public Vector3 GetScale();

    public MeshRenderer GetRenderer();

    public void SetPosition(Vector3 position);
    public void SetRotation(Quaternion rotation);
    public void SetScale(Vector3 scale);

    public void AnimateToSolution(Vector3 position, Quaternion rotation, Vector3 scale, float duration);
}
