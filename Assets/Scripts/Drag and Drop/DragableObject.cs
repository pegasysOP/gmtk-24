using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent (typeof(Rigidbody))]
public class DragableObject : MonoBehaviour, IDragable
{
    [Header("Movement")]
    public float dragForce = 500f;
    public float maxDragSpeed = 20f;
    public float movementDamping = 15f;

    [Header("Rotation")]
    public float rotationForce = 100f;
    public float maxRotationSpeed = 1f;
    public float rotationDamping = 1f;

    [Header("Scaling")]
    public float scaleRate = 0.1f;
    public float minScale = 0.1f;
    public float maxScale = 10f;

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
        rigidBody.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
    }

    private void Update()
    {
        if (locked || !selected)
            return;

        // drag input
        Vector3 mouseScreenPosition = Input.mousePosition;
        mouseScreenPosition.z = Camera.main.WorldToScreenPoint(transform.position).z;
        targetPos = Camera.main.ScreenToWorldPoint(mouseScreenPosition);

        // rotation input
        rotationInput = Input.GetAxis("Horizontal");

        // scale input
        scaleInput = Input.GetAxis("Vertical");
    }

    private void FixedUpdate()
    {
        if (locked || !selected)
            return;
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
            rigidBody.useGravity = true;
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
            rigidBody.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
        }
    }

    public bool IsLocked()
    {
        return locked;
    }

    public void Reset()
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
}
