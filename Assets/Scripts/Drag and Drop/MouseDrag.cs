using UnityEngine;

public class MouseDrag : MonoBehaviour
{
    private IDragable selectedDragable;

    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (selectedDragable != null)
            {
                selectedDragable.SetSelected(false);
                selectedDragable = null;
            }
        }
        else if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                IDragable dragable = hit.collider.GetComponent<IDragable>() ?? hit.collider.GetComponent<IDragableChild>()?.GetDragableParent();
                if (dragable == null || dragable.IsLocked())
                    return;

                if (selectedDragable != null)
                {
                    selectedDragable.SetSelected(false);
                    selectedDragable.OnLocked.RemoveListener(() => selectedDragable = null);
                }                

                dragable.SetSelected(true);
                selectedDragable = dragable;
                selectedDragable.OnLocked.AddListener(() => selectedDragable = null);
            }
        }
    }
}
