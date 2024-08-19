using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    public Transform target;
    public Transform rotatePoint;

    public Vector3 positionOffset;

    /// <summary>
    /// set to true if using cinemachine cam as position is set through "Body" fields
    /// </summary>
    public bool avoidPositionUpdate;
    [Range(-45f, 45f)]
    public float rotateOffset;
    // height offset for camera above wizzward this is to point down as the camera sits above the wizzy
    private float heightOffset = 10f;

    private void LateUpdate()
    {
        if (avoidPositionUpdate)
        {
            rotatePoint.position = new Vector3(rotatePoint.position.x, target.position.y, rotatePoint.position.z);
            Vector3 direction = (target.position - rotatePoint.position).normalized;
            transform.position = target.position + direction * positionOffset.z;
            transform.position += new Vector3(positionOffset.x, positionOffset.y, 0);
        }

        Vector3 offsetLookat = new Vector3(rotatePoint.position.x, this.transform.position.y - heightOffset , rotatePoint.position.z);
        transform.LookAt(offsetLookat);
    }
}
