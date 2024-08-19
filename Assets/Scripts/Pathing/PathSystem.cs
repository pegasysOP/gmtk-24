using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class PathSystem : MonoBehaviour
{
    public static PathSystem Instance;
    private SplineContainer _splineContainer;

    //public List<CheckPoint> points = new List<CheckPoint>();
    public Wizard wizard;
    public RespawnPoint currentRespawnPoint = null;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        //points.Clear();
        //
        //foreach (Transform child in transform)
        //{
        //    if (child.TryGetComponent<CheckPoint>(out CheckPoint point))
        //    {
        //        points.Add(point);
        //    }
        //}
    }

    private void Start()
    {
        wizard.RespawnPointReached += OnRespawnPointReached;
        wizard.RespawnPointReset += OnSpawnPointReset;
        _splineContainer = gameObject.GetComponent<SplineContainer>();
    }

    private void OnRespawnPointReached(RespawnPoint respawnPoint)
    {
        currentRespawnPoint = respawnPoint;
    }

    private void OnSpawnPointReset()
    {
        currentRespawnPoint?.DoReset();
    }

    public SplineContainer GetSpline()
    {
        return _splineContainer; 
    }
}
