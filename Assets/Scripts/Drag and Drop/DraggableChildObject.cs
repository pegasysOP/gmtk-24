using UnityEngine;

[RequireComponent(typeof(Collider))]
public class DraggableChildObject : MonoBehaviour, IDraggableChild
{
    public DraggableObject draggableParent;

    public IDraggable GetDraggableParent()
    {
        return draggableParent;
    }
}
