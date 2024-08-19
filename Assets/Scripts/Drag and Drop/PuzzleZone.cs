using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class PuzzleZone : MonoBehaviour
{
    public bool showBoundsInEditor = true;
    [Range(0, 100)] public float width = 10f;
    [Range(0, 100)] public float height = 10f;
    [Range(0, 360)] public float angle = 0f;

    BoxCollider boxCollider;
    Plane plane;

    private void OnValidate()
    {
        boxCollider = GetComponent<BoxCollider>();
        boxCollider.isTrigger = true;
        boxCollider.size = new Vector3(width, height, 0f);
        gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
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
        bool lockHorizontal = draggableObject.lockHorizontal;
        bool lockVertical = draggableObject.lockVertical;

        //clamp x or y if locked
        if (lockHorizontal)
        {
            rigidbody.velocity = new Vector3(0f, rigidbody.velocity.y, 0f);
            rigidbody.position = new Vector3(transform.position.x, rigidbody.position.y, transform.position.z);
        }
        if (lockVertical)
        {
            rigidbody.velocity = new Vector3(rigidbody.velocity.x, 0, rigidbody.velocity.z);
            rigidbody.position = new Vector3(rigidbody.position.x, transform.position.y, rigidbody.position.z);
        }

        // map velocity onto plane
        rigidbody.velocity = Vector3.ProjectOnPlane(rigidbody.velocity, plane.normal);
        //rigidbody.angularVelocity = Vector3.ProjectOnPlane(rigidbody.angularVelocity, plane.normal);

        // wait till physics has been calculated
        yield return new WaitForFixedUpdate();

        // lock rotation
        Quaternion alignToPlaneNormal = Quaternion.FromToRotation(Vector3.forward, plane.normal);
        Quaternion localRotation = Quaternion.Inverse(alignToPlaneNormal) * rigidbody.transform.rotation;
        localRotation = Quaternion.Euler(0, 0, localRotation.eulerAngles.z);
        rigidbody.transform.rotation = alignToPlaneNormal * localRotation;

        // move to closest position            
        Vector3 closestPoint = boxCollider.ClosestPoint(rigidbody.position);
        if (Vector3.Distance(rigidbody.position, closestPoint) > Mathf.Epsilon)
        {
            Vector3 targetDirection = (closestPoint - rigidbody.position).normalized;

            float targetDistance = (closestPoint - rigidbody.position).magnitude;

            rigidbody.AddForce(targetDirection * draggableObject.dragForce * targetDistance);
            if (rigidbody.velocity.magnitude > draggableObject.maxDragSpeed)
                rigidbody.velocity = rigidbody.velocity.normalized * draggableObject.maxDragSpeed;

            Debug.DrawRay(rigidbody.position, targetDirection * targetDistance);
        }
    }
}