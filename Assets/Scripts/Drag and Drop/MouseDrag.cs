using UnityEngine;

public class MouseDrag : MonoBehaviour
{
    IDragable selectedDragable;

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
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
            {
                IDragable dragable = hit.collider.gameObject.GetComponent<IDragable>();
                if (dragable != null)
                {
                    if(selectedDragable != null)
                        selectedDragable.SetSelected(false);

                    dragable.SetSelected(true);
                    selectedDragable = dragable;
                }
            }
        }
        
    }
}
