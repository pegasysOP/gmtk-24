using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class RespawnPoint : MonoBehaviour
{
    [Header("Collider")]
    public BoxCollider boxCollider;
    public bool showBoundsInEditor = true;

    [Header("Resettables")]
    public List<CheckPoint> checkPoints = new List<CheckPoint>();

    public void OnDrawGizmos()
    {
        if (!showBoundsInEditor)
            return;

        Matrix4x4 matrix = Gizmos.matrix;
        Color color = Gizmos.color;

        Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);
        Gizmos.color = Color.red;

        Gizmos.DrawWireCube(boxCollider.center, boxCollider.size);

        Gizmos.matrix = matrix;
        Gizmos.color = color;
    }

    public void DoReset()
    {
        foreach (CheckPoint checkPoint in checkPoints)
        {
            checkPoint.DoReset();
        }
    }
}