using UnityEngine;

public interface IDragable
{
    public void SetSelected(bool selected);
    public void Lock(bool locked);
    public bool IsLocked();
    public void Reset();

    public Vector3 GetPosition();
    public Quaternion GetRotation();
    public Vector3 GetScale();

    public void SetPosition(Vector3 position);
    public void SetRotation(Quaternion rotation);
    public void SetScale(Vector3 scale);
}
