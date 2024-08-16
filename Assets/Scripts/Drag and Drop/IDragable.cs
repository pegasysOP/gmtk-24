using UnityEngine;

public interface IDragable
{
    public void SetSelected(bool selected);

    public Vector3 GetPosition();

    public void SetPosition(Vector3 position);
}
