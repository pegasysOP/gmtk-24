using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class PathSystem : MonoBehaviour
{
    public static PathSystem Instance;

    public List<CheckPoint> points = new List<CheckPoint>();
    public Wizard wizard;
    public int lastReachedCheckPointIndex = 0;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        points.Clear();

        foreach (Transform child in transform)
        {
            if (child.TryGetComponent<CheckPoint>(out CheckPoint point))
            {
                points.Add(point);
            }
        }
    }

    private void Start()
    {
        wizard.CheckPointTrigger += CheckPointReached;
    }

    private void CheckPointReached(CheckPoint checkPoint)
    {
        lastReachedCheckPointIndex++;
    }
}
