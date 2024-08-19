using UnityEngine;

public class MouseDrag : MonoBehaviour
{
    private DraggableObject selectedDraggable;

    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (selectedDraggable != null)
            {
                selectedDraggable.SetSelected(false);
                selectedDraggable = null;

                GameManager.Instance.audioSystem.PlayDragDeselected();
            }
        }
        else if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                DraggableObject draggable = hit.collider.GetComponent<DraggableObject>() ?? hit.collider.GetComponent<DraggableChildObject>()?.GetDraggableParent();
                if (draggable == null || (draggable.IsLocked() && draggable.stayLocked))
                    return;

                if (selectedDraggable != null)
                {
                    selectedDraggable.SetSelected(false);
                    selectedDraggable.OnLocked.RemoveListener(() => selectedDraggable = null);
                }                

                draggable.SetSelected(true);
                selectedDraggable = draggable;
                selectedDraggable.OnLocked.AddListener(() => selectedDraggable = null);

                GameManager.Instance.audioSystem.PlayDragSelected();
            }
        }
    }

    public IDraggable GetDraggable()
    {
        return selectedDraggable; 
    }
}
