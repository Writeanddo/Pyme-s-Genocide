using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// !!!! IMPORTANTE - NO MOVER DE LA CARPETA RESOURCES !!!!
// Este prefab se carga de forma dinámica

public class Minion : MonoBehaviour
{
    [SerializeField] int moveSpeed = 3;

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
    Animator animator;

    bool wasAwake;
    BoxCollider boxCollider;

    [System.NonSerialized]
    public bool explosive = false;
    [SerializeField] ParticleSystem ps;

    [Header("Explosion")]
    [SerializeField] float explosionRadius;
    [SerializeField] float explosionForce;

    [SerializeField] AudioClip[] sfxs;
    float talkDelayValue;
    [SerializeField] Vector2 minMaxTalkDelayValues;

    bool walkEnabled = false;

    public bool readyToHarvest = true;

    GameManager gm;

    MinionSpawnerFollow spawner;

    private void Awake()
    {
        _transform = transform;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();

        detectionDistance = Random.Range(minDetectionDistance, maxDetectionDistance);
    }

    Coroutine randomNoiseCoroutine;

    IEnumerator RandomNoise()
    {
        talkDelayValue = Random.Range(minMaxTalkDelayValues.x, minMaxTalkDelayValues.y);
        yield return new WaitForSeconds(talkDelayValue);

        while (true)
        {
            if (sfxs != null)
            {
                gm.audioManager.PlayOneShot(sfxs[Random.Range(0, sfxs.Length)], transform.position);
            }

            talkDelayValue = Random.Range(minMaxTalkDelayValues.x, minMaxTalkDelayValues.y);
            yield return new WaitForSeconds(talkDelayValue);
        }
    }

    private void OnEnable()
    {
        if (gm == null) { gm = FindObjectOfType<GameManager>(); }

        if (playerTransform == null) { playerTransform = GameObject.FindWithTag("Player").transform; }

        if (spawner == null) { spawner = FindObjectOfType<MinionSpawnerFollow>(); }

        if (Mathf.RoundToInt(Random.Range(0, 2)) == 1)
            walkEnabled = true;

        float percent = Random.Range(0, 100);
        if (percent < 70)
            type = Type.Coward;
        else if (percent < 95)
            type = Type.Crazy;
        else type = Type.Follower;

        randomNoiseCoroutine = StartCoroutine(RandomNoise());
    }

    private void OnDisable()
    {
        if (randomNoiseCoroutine != null)
        {
            StopCoroutine(randomNoiseCoroutine);
        }
    }

    public void DisableHarvestingForSeconds(float seconds)
    {
        StartCoroutine(DisableHarvesting(seconds));
    }

    IEnumerator DisableHarvesting(float seconds)
    {
        readyToHarvest = false;
        yield return new WaitForSeconds(seconds);
        readyToHarvest = true;
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
        transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one, Time.deltaTime);

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

        if (spawner != null)
        {
            if (sqrDistance > spawner.Radius * spawner.Radius)
            {
                MinionsPool.Instance.Put(this);
                return;
            }
        }

        if (sqrDistance < detectionDistance * detectionDistance)
        {
            Vector3 targetPos = new Vector3(playerTransform.position.x, _transform.position.y, playerTransform.position.z);
            if (animator)
                animator.SetBool("Running", true);

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
                    if (counter > 0.1f)
                    {
                        randomPos = new Vector3(UnityEngine.Random.Range(-10, 10), _transform.position.y, UnityEngine.Random.Range(-10, 10));
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
        else if (walkEnabled)
        {
            counter += Time.deltaTime;
            if (counter > 3f)
            {
                randomPos = new Vector3(UnityEngine.Random.Range(-10, 10), _transform.position.y, UnityEngine.Random.Range(-10, 10));
                counter = 0;
            }

            Quaternion rotation = Quaternion.LookRotation(randomPos - _transform.position, Vector3.up);
            transform.rotation = Quaternion.Lerp(_transform.rotation, rotation, rotationSpeed * Time.deltaTime);

            Vector3 heading = randomPos - _transform.position;
            heading.y = 0;
            float distance = heading.magnitude;
            Vector3 direction = heading / distance;

            rb.AddForce(_transform.forward * moveSpeed * Time.deltaTime, ForceMode.VelocityChange);

            Vector2 hMov = new Vector2(rb.velocity.x, rb.velocity.z);
            hMov = hMov.normalized * Mathf.Min(hMov.magnitude, moveSpeed);
            rb.velocity = new Vector3(hMov.x, rb.velocity.y, hMov.y);
        }
        else if (animator) animator.SetBool("Running", false);
    }

    private Vector3 velocityBeforePhysicsUpdate;
    void FixedUpdate()
    {
        velocityBeforePhysicsUpdate = rb.velocity;
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

        return grounded;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (explosive)
        {
            Vector3 explosionPos = collision.contacts[0].point;
            Collider[] colliders = Physics.OverlapSphere(explosionPos, explosionRadius);

            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].gameObject.GetInstanceID() == playerTransform.gameObject.GetInstanceID())
                {
                    PlayerControllerRB rb = colliders[i].GetComponent<PlayerControllerRB>();
                    if (rb != null)
                    {
                        Vector3 dir = (playerTransform.position - explosionPos).normalized;
                        float distance = Vector3.Distance(playerTransform.position, explosionPos);
                        float appliedForce = 0.2f * explosionForce * (1.0f - Mathf.Clamp01(distance / explosionRadius));

                        PlayerControllerExternalForce force = new PlayerControllerExternalForce
                        {
                            force = appliedForce * dir,
                            mode = ForceMode.Impulse
                        };

                        rb.AddExternalForce(force);
                    }
                }
                else
                {
                    Rigidbody rb = colliders[i].GetComponent<Rigidbody>();
                    if (rb != null)
                    {
                        rb.AddExplosionForce(explosionForce, explosionPos, explosionRadius, 3.0F);
                    }
                }
            }

            gm.audioManager.PlayOneShot(gm.audioManager.poof, transform.position);
            ParticleSystemManager.Instance.Play(ps, _transform);
            MinionsPool.Instance.Put(this);
        }
        else
        {
            if (velocityBeforePhysicsUpdate.magnitude > 10.0f)
            {
                gm.audioManager.PlayOneShot(gm.audioManager.squeaks[Random.Range(0, gm.audioManager.squeaks.Length)], _transform.position);
            }
        }
    }
}
