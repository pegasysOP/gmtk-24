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

    public GameObject wizardsMagicBeam;

    private void Awake()
    {
        splineAnimate = GetComponent<SplineAnimate>();
    }

    private void Start()
    {
        GameManager.Instance.StartGame += OnStartGameEvent;
    }

    private void OnDestroy()
    {
        GameManager.Instance.StartGame -= OnStartGameEvent;
    }

    private void OnStartGameEvent()
    {
        splineAnimate.Play();
    }

    private void Update()
    {
        if (wizardsMagicBeam == null)
        {
            return;
        }

        IDraggable draggable = GameManager.Instance.mouseDrag.GetDraggable();
        if (draggable != null)
        {
            wizardsMagicBeam.transform.LookAt(draggable.GetPosition());
            wizardsMagicBeam.SetActive(true);
        }
        else
        {            
            wizardsMagicBeam.SetActive(false);
        }
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

        if (collider.TryGetComponent<TriggerCollider>(out TriggerCollider trigger))
        {
            trigger.Trigger();
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
