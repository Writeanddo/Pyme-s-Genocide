using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// !!!! IMPORTANTE - NO MOVER DE LA CARPETA RESOURCES !!!!
// Este prefab se carga de forma dinámica

public class Minion : MonoBehaviour
{

    [SerializeField] int moveSpeed = 3;
    [SerializeField] float realScale = 0.2f;

    [SerializeField] float minDetectionDistance;
    [SerializeField] float maxDetectionDistance;

    float detectionDistance = 6;

    public enum Type { Follower, Coward, Crazy }
    public Type type;

    Transform playerTransform;
    private int rotationSpeed = 6;
    float counter = 3;
    Vector3 randomPos;

    Transform _transform;
    Rigidbody rb;

    bool wasAwake;
    BoxCollider boxCollider;

    public bool explosive = true;

    private void Awake()
    {
        _transform = transform;
        rb = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();

        detectionDistance = Random.Range(minDetectionDistance, maxDetectionDistance);
    }

    void Start()
    {
        transform.localScale = Vector3.one * Time.deltaTime;
        playerTransform = GameObject.FindWithTag("Player").transform;
    }

    public void EnterPhysicsMode()
    {
        // rb.isKinematic = false;
        rb.constraints = RigidbodyConstraints.None;
    }

    public void EnterTransformMode()
    {
        // rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        rb.rotation = Quaternion.identity;
    }

    // Shortcut
    public void AddForce(Vector3 force, ForceMode forceMode)
    {
        EnterPhysicsMode();
        rb.AddForce(force, forceMode);
    }

    public void AddTorque(Vector3 torque, ForceMode forceMode)
    {
        EnterPhysicsMode();
        rb.AddTorque(torque, forceMode);
    }

    void Update()
    {
        if (transform.localScale.x < realScale) transform.localScale += Vector3.one * realScale * Time.deltaTime;

        if (Mathf.Abs(rb.velocity.y) < 0.1f)
        {
            EnterTransformMode();
        }
        else
        {
            EnterPhysicsMode();
            return;
        }

        float sqrDistance = (playerTransform.position - _transform.position).sqrMagnitude;

        if (sqrDistance < detectionDistance * detectionDistance)
        {
            Vector3 targetPos = new Vector3(playerTransform.position.x, _transform.position.y, playerTransform.position.z);


            switch (type)
            {
                case Type.Follower:
                    transform.rotation = Quaternion.Lerp(_transform.rotation,
                    Quaternion.LookRotation(targetPos - _transform.position), rotationSpeed * Time.deltaTime);
                    break;

                case Type.Coward:
                    _transform.rotation = Quaternion.Lerp(_transform.rotation,
                    Quaternion.LookRotation(_transform.position - targetPos), rotationSpeed * Time.deltaTime);
                    break;

                case Type.Crazy:
                    counter += Time.deltaTime;
                    if (counter > 0.5f)
                    {
                        randomPos = new Vector3(Random.Range(-10, 10), _transform.position.y, Random.Range(-10, 10));
                        counter = 0;
                    }

                    Quaternion rotation = Quaternion.LookRotation(randomPos - _transform.position, Vector3.up);
                    transform.rotation = Quaternion.Lerp(_transform.rotation, rotation, rotationSpeed * Time.deltaTime);

                    Vector3 heading = randomPos - _transform.position;
                    heading.y = 0;
                    float distance = heading.magnitude;
                    Vector3 direction = heading / distance;

                    break;
            }

            rb.AddForce(_transform.forward * moveSpeed * Time.deltaTime, ForceMode.VelocityChange);

            Vector2 hMov = new Vector2(rb.velocity.x, rb.velocity.z);
            hMov = hMov.normalized * Mathf.Min(hMov.magnitude, moveSpeed);
            rb.velocity = new Vector3(hMov.x, rb.velocity.y, hMov.y);
        }
    }

    bool SnapToGround()
    {
        RaycastHit hitInfo;

        bool grounded = Physics.Raycast(
            _transform.position,
            Vector3.down,
            out hitInfo,
            _transform.localScale.y * (0.5f * boxCollider.size.y + 0.15f),
            -5,
            QueryTriggerInteraction.Ignore);

        if (grounded)
        {
            Vector3 pos = _transform.position;
            pos.y = hitInfo.point.y + _transform.localScale.y * (0.5f * boxCollider.size.y);
            _transform.position = pos;
        }

        return grounded;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (explosive)
        {
            MinionsPool.Instance.Put(this);
        }
    }
}
