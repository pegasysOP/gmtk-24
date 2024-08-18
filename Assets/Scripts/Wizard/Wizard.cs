using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Splines;

public class Wizard : MonoBehaviour
{
    private SplineAnimate splineAnimate;
    public Action<CheckPoint> CheckPointTrigger;
    public Action ResetCheckPoint;
    public float time;

    public CheckPoint currentCheckPoint;

    public GameObject wizardsMagicBeam;

    public Animator animator;

    private void Awake()
    {
        splineAnimate = GetComponent<SplineAnimate>();
    }

    private void Start()
    {
        GameManager.Instance.StartGame += OnStartGameEvent;
        animator = GetComponent<Animator>();
    }

    private void OnDestroy()
    {
        GameManager.Instance.StartGame -= OnStartGameEvent;
    }

    private void OnStartGameEvent()
    {
        animator.SetTrigger("Walking");
        GameManager.Instance.audioSystem.StartWalking();
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

    public bool IsWalking()
    {
        return splineAnimate.IsPlaying;
    }

    private IEnumerator ResetWizard(CheckPoint checkPoint)
    {
        checkPoint.OnComplete += OnOverTimeCheckPointCompleteEvent;
        WizardPause();
        yield return new WaitForSeconds(5f);

        if (!checkPoint.hasCompleted)
        {
            splineAnimate.ElapsedTime = time;
            ResetCheckPoint?.Invoke();
            //this is basically you losing here and resetting
            yield return new WaitForSeconds(1f);
            splineAnimate.Play();
            yield break;
        }

        splineAnimate.Play();
    }

    private void WizardPause()
    {
        splineAnimate.Pause();
        GameManager.Instance.audioSystem.StopWalking();
        animator.SetTrigger("Angry");
        Debug.Log($"Wizard is getting angry. Hurry up.");
        GameManager.Instance.audioSystem.StopAngry();
    }

    private void OnOverTimeCheckPointCompleteEvent()
    {
        currentCheckPoint.OnComplete -= OnOverTimeCheckPointCompleteEvent;
        StopAllCoroutines();
        GameManager.Instance.audioSystem.StopAngry();
        animator.SetTrigger("Walking");
        Debug.Log($"Wizard starts to move again.");
        splineAnimate.Play();
        GameManager.Instance.audioSystem.StartWalking();
    }
}
