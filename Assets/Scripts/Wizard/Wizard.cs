using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Splines;

public class Wizard : MonoBehaviour
{
    private SplineAnimate splineAnimate;
    public Action<CheckPoint> CheckPointTrigger;
    public float time;

    private Coroutine current;

    private void Awake()
    {
        splineAnimate = GetComponent<SplineAnimate>();
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.TryGetComponent<CheckPoint>(out CheckPoint checkPoint))
        {
            if (checkPoint.hasCompleted)
            {
                CheckPointTrigger?.Invoke(checkPoint);
                time = splineAnimate.ElapsedTime;
                Debug.Log($"Time: {time}");
            }
            else
            {
                splineAnimate.Pause();
                current = StartCoroutine(ResetWizard(checkPoint));
            }
        }
    }

    private IEnumerator ResetWizard(CheckPoint checkPoint)
    {
        yield return new WaitForSeconds(5f);
        splineAnimate.ElapsedTime = time;
        yield return new WaitForSeconds(1f);
        current = null;
        splineAnimate.Play();
    }
}
