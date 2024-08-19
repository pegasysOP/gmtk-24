using UnityEngine;

[RequireComponent(typeof(Collider))]
public class DraggableChildObject : MonoBehaviour
{
    public DraggableObject draggableParent;

    public DraggableObject GetDraggableParent()
    {
        return draggableParent;
    }
}
