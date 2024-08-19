using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Splines;

public class Wizard : MonoBehaviour
{
    public static Wizard Instance;

    private SplineAnimate splineAnimate;
    private float splineTotalTime;

    public RespawnPoint currentRespawnPoint;
    public Action<RespawnPoint> RespawnPointReached;
    public Action RespawnPointReset;
    public float respawnSplineTime;

    public CheckPoint currentCheckPoint;

    public GameObject wizardsMagicBeam;
    public SkinnedMeshRenderer meshRenderer;
    public GameObject popParticlesContainer;
    public GameObject[] wizardPopParticles;

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
        // save respawn point for later
        if (collider.TryGetComponent<RespawnPoint>(out RespawnPoint respawnPoint))
        {
            Debug.LogWarning("Respawn Point Reached");
            currentRespawnPoint = respawnPoint;
            respawnSplineTime = splineAnimate.ElapsedTime;

            RespawnPointReached?.Invoke(respawnPoint);
        }

        // stop at checkpoint until complete
        if (collider.TryGetComponent<CheckPoint>(out CheckPoint checkPoint))
        {
            Debug.LogWarning("Puzzle Check Point Reached");

            if (!checkPoint.hasCompleted)
                StartCoroutine(PopWizard(checkPoint));
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

   //private IEnumerator ResetWizard(CheckPoint checkPoint)
   //{
   //    checkPoint.OnComplete += OnOverTimeCheckPointCompleteEvent;
   //    WizardPause();
   //    yield return new WaitForSeconds(chargeAnimLength);
   //
   //    if (!checkPoint.hasCompleted)
   //    {
   //        splineAnimate.ElapsedTime = respawnSplineTime;
   //        RespawnPointReset?.Invoke();
   //        yield return new WaitForSeconds(1f);
   //        splineAnimate.Play();
   //        animator.SetTrigger("Walking");
   //        yield break;
   //    }
   //
   //    splineAnimate.Play();
   //}

    private IEnumerator ResetWizard(CheckPoint checkPoint, float delaySeconds)
    {
        yield return new WaitForSeconds(delaySeconds);

        UnpopWizard();

        if (!checkPoint.hasCompleted)
        {
            splineAnimate.ElapsedTime = respawnSplineTime;
            RespawnPointReset?.Invoke();
            yield return new WaitForSeconds(1f);
            splineAnimate.Play();
            animator.SetTrigger("Walking");
            yield break;
        }

        splineAnimate.Play();
    }

    private IEnumerator PopWizard(CheckPoint checkPoint)
    {
        currentCheckPoint = checkPoint;
        checkPoint.OnComplete += OnOverTimeCheckPointCompleteEvent;
        WizardPause();
        yield return new WaitForSeconds(chargeAnimLength);
        if (checkPoint.hasCompleted)
            yield return null;

        PopWizard();
        StartCoroutine(ResetWizard(checkPoint, 2f));
    }

    public void PopWizard()
    {
        meshRenderer.enabled = false;
        popParticlesContainer.SetActive(true);
    }

    public void UnpopWizard()
    {
        meshRenderer.enabled = true;
        popParticlesContainer.SetActive(false);
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
        GameManager.Instance.audioSystem.OnRespawnPointReset();
        currentCheckPoint.OnComplete -= OnOverTimeCheckPointCompleteEvent;
        currentCheckPoint = null;

        StopAllCoroutines();
        animator.SetTrigger("Walking");
        splineAnimate.Play();
    }
}
