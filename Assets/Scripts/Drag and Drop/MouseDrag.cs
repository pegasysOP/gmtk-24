using UnityEngine;

public class MouseDrag : MonoBehaviour
{
    private IDraggable selectedDraggable;

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
                IDraggable draggable = hit.collider.GetComponent<IDraggable>() ?? hit.collider.GetComponent<IDraggableChild>()?.GetDraggableParent();
                if (draggable == null || draggable.IsLocked())
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
