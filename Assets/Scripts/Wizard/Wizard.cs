using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Splines;

public class Wizard : MonoBehaviour
{
    private SplineAnimate splineAnimate;
    public Action<CheckPoint> CheckPointTrigger;
    public float time;

    public CheckPoint currentCheckPoint;
    private void Awake()
    {
        splineAnimate = GetComponent<SplineAnimate>();
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.TryGetComponent<CheckPoint>(out CheckPoint checkPoint))
        {
            currentCheckPoint = checkPoint;
            if (checkPoint.hasCompleted)
            {
                CheckPointTrigger?.Invoke(checkPoint);
                time = splineAnimate.ElapsedTime;
                Debug.Log($"Time: {time}");
            }
            else
            {
                StartCoroutine(ResetWizard(checkPoint));
            }
        }
    }

    private IEnumerator ResetWizard(CheckPoint checkPoint)
    {
        checkPoint.OnComplete += OnOverTimeCheckPointCompleteEvent;
        splineAnimate.Pause();
        yield return new WaitForSeconds(5f);

        if (!checkPoint.hasCompleted)
        {
            splineAnimate.ElapsedTime = time;
            //this is basically you losing here and resetting
            yield return new WaitForSeconds(1f);
            splineAnimate.Play();
            yield break;
        }

        splineAnimate.Play();
    }

    private void OnOverTimeCheckPointCompleteEvent()
    {
        currentCheckPoint.OnComplete -= OnOverTimeCheckPointCompleteEvent;
        StopAllCoroutines();
        splineAnimate.Play();
    }
}
