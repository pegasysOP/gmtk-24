using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent (typeof(Rigidbody))]
public class DraggableObject : MonoBehaviour, IDraggable
{

    [Header("Movement")]
    public float dragForce = 500f;
    public float maxDragSpeed = 20f;
    public float movementDamping = 15f;

    [Header("Rotation")]
    public float rotationForce = 1001f;
    public float maxRotationSpeed = 1f;
    public float rotationDamping = 1f;

    [Header("Scaling")]
    public float scaleRate = 0.1f;
    public float minScale = 0.1f;
    public float maxScale = 10f;

    [Header("Constraints")]
    public PuzzleZone puzzleZone;
    public bool canDrag = true;
    public bool canRotate = true;
    public bool canScale = true;
    public bool useGravity = true;
    public bool lockHorizontal = false;
    public bool lockVertical = false;

    [Header("Impacts")]
    public float impactForceThreshold = 1f;
    public GameObject objectDust;
    public GameObject floorDust;
    public float particleDeathTimer = 3f;

    private Rigidbody rigidBody;
    private Vector3 startPosition;
    private Quaternion startRotation;
    private Vector3 startScale;
    private float defaultMovementDamping;
    private float defaultRotationDamping;

    private bool selected = false;
    private bool locked = false;

    private Vector3 targetPos = Vector3.zero;
    private float rotationInput = 0;
    private float scaleInput = 0;

    private UnityEvent onLocked = new UnityEvent();
    public UnityEvent OnLocked { get { return onLocked; } }

    private void OnValidate()
    {
        rigidBody = GetComponent<Rigidbody>();

        startPosition = transform.position;
        startRotation = transform.rotation;
        startScale = transform.localScale;

        defaultMovementDamping = rigidBody.drag;
        defaultRotationDamping = rigidBody.angularDrag;

        rigidBody.interpolation = RigidbodyInterpolation.Interpolate;
        rigidBody.collisionDetectionMode = CollisionDetectionMode.Continuous;
        rigidBody.constraints = RigidbodyConstraints.None;
        rigidBody.useGravity = useGravity;
    }

    private void Update()
    {
        if (locked || !selected)
            return;

        // drag input
        if (canDrag)
        {
            Vector3 mouseScreenPosition = Input.mousePosition;
            mouseScreenPosition.z = Camera.main.WorldToScreenPoint(transform.position).z;
            targetPos = Camera.main.ScreenToWorldPoint(mouseScreenPosition);

            //clamp x or y if locked
            if (lockVertical)
                targetPos = new Vector3(targetPos.x, transform.position.y, targetPos.z);
            if (lockHorizontal)
                targetPos = new Vector3(transform.position.x, targetPos.y, transform.position.z);
        }

        // rotation input
        if (canRotate)
        {
            rotationInput = Input.GetAxis("Horizontal");
        }

        // scale input
        if (canScale)
        {
            scaleInput = Input.GetAxis("Vertical");
        }
    }

    private void FixedUpdate()
    {
        if (selected && !locked)
        {
            Vector3 targetDirection = (targetPos - transform.position).normalized;
            float targetDistance = (targetPos - transform.position).magnitude;

            // movement
            rigidBody.AddForce(targetDirection * dragForce * targetDistance);
            if (rigidBody.velocity.magnitude > maxDragSpeed)
                rigidBody.velocity = rigidBody.velocity.normalized * maxDragSpeed;

            // scaling
            Vector3 targetScale = transform.localScale + Vector3.one * scaleInput * scaleRate;
            targetScale = new Vector3(Mathf.Clamp(targetScale.x, minScale, maxScale),
                                        Mathf.Clamp(targetScale.y, minScale, maxScale),
                                        Mathf.Clamp(targetScale.z, minScale, maxScale));
            transform.localScale = targetScale;

            // rotation            
            rigidBody.AddTorque(transform.forward * rotationInput * -rotationForce);
            if (rigidBody.angularVelocity.magnitude > (maxRotationSpeed * transform.localScale.magnitude))
                rigidBody.angularVelocity = rigidBody.angularVelocity.normalized * (maxRotationSpeed * transform.localScale.magnitude);
        }

        if (puzzleZone != null)
            StartCoroutine(puzzleZone.ClampRigidbody(this));
    }

    public void SetSelected(bool selected)
    {
        this.selected = selected;

        if (selected)
        {
            rigidBody.useGravity = false;
            rigidBody.drag = movementDamping;
            rigidBody.angularDrag = rotationDamping;
        }
        else
        {
            rigidBody.useGravity = useGravity;
            rigidBody.drag = defaultMovementDamping;
            rigidBody.angularDrag = defaultRotationDamping;
        }
    }

    public void Lock(bool locked)
    {
        this.locked = locked;

        if (locked)
        {
            OnLocked?.Invoke();
            SetSelected(false);
            rigidBody.constraints = RigidbodyConstraints.FreezeAll;
        }
        else
        {
            rigidBody.constraints = RigidbodyConstraints.None;
        }
    }

    public bool IsLocked()
    {
        return locked;
    }

    public void DoReset()
    {
        SetPosition(startPosition);
        SetRotation(startRotation);
        SetScale(startScale);

        Lock(false);
        SetSelected(false);
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public Quaternion GetRotation()
    {
        return transform.rotation;
    }

    public Vector3 GetScale()
    {
        return transform.localScale;
    }

    public void SetPosition(Vector3 position)
    {
        transform.position = position;
    }

    public void SetRotation(Quaternion rotation)
    {
        transform.rotation = rotation;
    }

    public void SetScale(Vector3 scale)
    {
        transform.localScale = scale;
    }

    public void AnimateToSolution(Vector3 position, Quaternion rotation, Vector3 scale, float duration)
    {
        Lock(true);

        Sequence sequence = DOTween.Sequence();
        sequence.Insert(0f, transform.DOMove(position, duration));
        sequence.Insert(0f, transform.DORotateQuaternion(rotation, duration));
        sequence.Insert(0f, transform.DOScale(scale, duration));
        sequence.Play();
    }

    void OnCollisionEnter(Collision collision)
    {
        float impactMagnitude = collision.impulse.magnitude;
        if (objectDust == null || floorDust == null)
        {
            Debug.Log("Paricle effect not assigned");
            return;
        }

        if (impactMagnitude > impactForceThreshold)
        {
            GameManager.Instance.audioSystem.PlayDraggableCollide();
            GameObject hitParticle = Instantiate(objectDust, collision.GetContact(0).point, Quaternion.identity);
            Destroy(hitParticle, particleDeathTimer);

            if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                GameObject groundParticle = Instantiate(floorDust, collision.GetContact(0).point, Quaternion.identity);
                Destroy(groundParticle, particleDeathTimer);
            }
        }
    }

    public MeshRenderer GetRenderer()
    {
        return gameObject.GetComponent<MeshRenderer>();
    }

    public Rigidbody GetRigidbody()
    {
        return rigidBody;
    }
}
