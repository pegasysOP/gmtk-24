using UnityEngine;

[RequireComponent(typeof(Collider))]
public class DragableChildObject : MonoBehaviour, IDragableChild
{
    public DragableObject dragableParent;

    public IDragable GetDragableParent()
    {
        return dragableParent;
    }
}
