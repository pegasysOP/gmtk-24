using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class PathSystem : MonoBehaviour
{
    public static PathSystem Instance;
    private SplineContainer _splineContainer;

    public List<CheckPoint> points = new List<CheckPoint>();
    public Wizard wizard;
    public int lastReachedCheckPointIndex = 0;
    public CheckPoint lastReachedCheckPoint = null;

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
        wizard.ResetCheckPoint += OnCheckPointReset;
        _splineContainer = gameObject.GetComponent<SplineContainer>();
    }

    private void OnCheckPointReset()
    {
        lastReachedCheckPoint.DoReset();
    }

    public SplineContainer GetSpline()
    {
        return _splineContainer; 
    }

    private void CheckPointReached(CheckPoint checkPoint)
    {
        lastReachedCheckPointIndex++;
        lastReachedCheckPoint = checkPoint;
    }
}
