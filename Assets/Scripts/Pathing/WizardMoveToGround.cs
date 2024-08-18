using UnityEngine;

public class WizardMoveToGround : MonoBehaviour
{
    public LayerMask layerMask;
    public float checkDistance = 5;
    public Transform wizard;

    void Update()
    {
        if (Physics.Raycast(transform.position, -Vector3.up, out RaycastHit hit, checkDistance, layerMask))
        {
            wizard.transform.position = hit.point;
        }
        else
        {
            wizard.transform.position = transform.position;
        }
    }
}
