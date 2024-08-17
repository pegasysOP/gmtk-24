using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    public Transform target;
    public Transform rotatePoint;

    public Vector3 positionOffset;

    [Range(-45f, 45f)]
    public float rotateOffset;

    private void LateUpdate()
    {
        rotatePoint.position = new Vector3(rotatePoint.position.x, target.position.y, rotatePoint.position.z);
        Vector3 direction = (target.position - rotatePoint.position).normalized;
        transform.position = target.position + direction * positionOffset.z;
        transform.position += new Vector3(positionOffset.x, positionOffset.y, 0);
        transform.LookAt(rotatePoint);
    }
}
