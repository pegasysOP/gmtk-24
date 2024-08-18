using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class PuzzleZone : MonoBehaviour
{
    public bool showBoundsInEditor = true;
    [Range(1, 100)] public float width = 10f;
    [Range(1, 100)] public float height = 10f;
    [Range(0, 360)] public float angle = 0f;

    BoxCollider boxCollider;
    Plane plane;

    private void OnValidate()
    {
        boxCollider = GetComponent<BoxCollider>();
        boxCollider.isTrigger = true;
        boxCollider.size = new Vector3(width, height, 0f);
        transform.eulerAngles = new Vector3(0f, angle, 0f);

        plane = new Plane(transform.forward, transform.position);
    }

    private void OnDrawGizmos()
    {
        if (!showBoundsInEditor)
            return;

        Matrix4x4 matrix = Gizmos.matrix;
        Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);
        Gizmos.DrawCube(boxCollider.center, boxCollider.size);
        Gizmos.matrix = matrix;
    }


    public IEnumerator ClampRigidbody(DraggableObject draggableObject)
    {
        Rigidbody rigidbody = draggableObject.GetRigidbody();

        // map velocity onto plane
        rigidbody.velocity = Vector3.ProjectOnPlane(rigidbody.velocity, plane.normal);

        // wait till physics has been calculated
        yield return new WaitForFixedUpdate();

        // move to closest position            
        Vector3 closestPoint = boxCollider.ClosestPoint(rigidbody.position);
        if (Vector3.Distance(rigidbody.position, closestPoint) > Mathf.Epsilon)
        {
            Vector3 targetDirection = (closestPoint - rigidbody.position).normalized;
            float targetDistance = (closestPoint - rigidbody.position).magnitude;

            // movement
            rigidbody.AddForce(targetDirection * draggableObject.dragForce * targetDistance);
            if (rigidbody.velocity.magnitude > draggableObject.maxDragSpeed)
                rigidbody.velocity = rigidbody.velocity.normalized * draggableObject.maxDragSpeed;
        }
    }

}
