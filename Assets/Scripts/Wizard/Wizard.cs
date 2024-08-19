using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Splines;

public class Wizard : MonoBehaviour
{
    public static Wizard Instance;

    private SplineAnimate splineAnimate;
    private float splineTotalTime;
    public Action<CheckPoint> CheckPointTrigger;
    public Action ResetCheckPoint;
    public float time;

    public CheckPoint currentCheckPoint;

    public GameObject wizardsMagicBeam;

    public Animator animator;
    private float chargeAnimLength; 

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        splineAnimate = GetComponent<SplineAnimate>();
    }

    private void Start()
    {
        GameManager.Instance.StartGame += OnStartGameEvent;
        splineTotalTime = splineAnimate.Duration;
    }

    private void OnDestroy()
    {
        GameManager.Instance.StartGame -= OnStartGameEvent;
    }

    private void OnStartGameEvent()
    {
        animator.SetTrigger("Walking");
        //GameManager.Instance.audioSystem.StartWalking();
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

        if (IsWalking()) 
        {
            GameManager.Instance.audioSystem.SetWindVolume(splineAnimate.ElapsedTime, splineTotalTime);
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

        if (collider.GetComponent<AmbienceTrigger>())
        {
            GameManager.Instance.audioSystem.PlayNextAmbience();
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
        yield return new WaitForSeconds(chargeAnimLength);

        if (!checkPoint.hasCompleted)
        {
            splineAnimate.ElapsedTime = time;
            ResetCheckPoint?.Invoke();
            yield return new WaitForSeconds(1f);
            splineAnimate.Play();
            animator.SetTrigger("Walking");
            yield break;
        }

        splineAnimate.Play();
    }

    private void WizardPause()
    {
        splineAnimate.Pause();
        animator.SetTrigger("Angry");
        if (chargeAnimLength == 0)
            chargeAnimLength = animator.GetCurrentAnimatorStateInfo(0).length;
    }

    private void OnOverTimeCheckPointCompleteEvent()
    {
        GameManager.Instance.audioSystem.OnResetCheckPoint();
        currentCheckPoint.OnComplete -= OnOverTimeCheckPointCompleteEvent;
        StopAllCoroutines();
        animator.SetTrigger("Walking");
        splineAnimate.Play();
    }
}
