using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Absorber : MonoBehaviour
{

    GameManager gameManager;
    [SerializeField] float pushAngle = 15.0f;
    [SerializeField] float pushForce = 2000.0f;
    [SerializeField] float pullForce = 100.0f;
    [SerializeField] float torque = 100.0f;

    public Transform spawnPoint;

    Capturer capturer;

    bool canFire = true;
    bool firing;
    public bool Absorbing { get; private set; }

    Coroutine fireCoroutine;

    float fireRatePerSecond;
    [SerializeField] float maxFireRatePerSecond;
    [SerializeField] float baseFireRatePerSecond;
    [SerializeField] float absorberRadius = 1.0f;
    [SerializeField] float absorberMaxDistance = 10.0f;

    [SerializeField] Transform head;

    [SerializeField] float autoDeactivateWeaponTimeInSeconds = 2.0f;
    private float autoDeactivateWeaponTimer;

    public bool ReadyToUse { get; private set; }

    bool weaponOut;

    [SerializeField] ParticleSystem absorbParticleSystem;
    [SerializeField] ParticleSystem pushParticleSystem;

    RaycastHit[] results = new RaycastHit[15];

    Animator animator;
    ThirdPersonCamera tps;

    PlayerControllerRB playerController;

    AudioSource audioSource;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        capturer = GetComponentInChildren<Capturer>();
        animator = GetComponent<Animator>();
        tps = FindObjectOfType<ThirdPersonCamera>();
        playerController = FindObjectOfType<PlayerControllerRB>();
        audioSource = GetComponent<AudioSource>();
    }

    private void FixedUpdate()
    {
        if (!Input.GetButton("Fire2") || !ReadyToUse)
        {
            audioSource.Stop();
            absorbParticleSystem.Stop();
            return;
        }

        if (!absorbParticleSystem.isPlaying)
        {
            absorbParticleSystem.Play();
        }

        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }

        Ray ray;
        if (playerController.FreeCamera)
        {
            ray = new Ray(head.position, absorbParticleSystem.transform.parent.forward);
        }
        else
        {
            ray = new Ray(head.position, (tps.FocalPoint - head.position).normalized);
        }

        Debug.DrawRay(ray.origin, absorberMaxDistance * ray.direction, Color.red);

        int ammount = Physics.SphereCastNonAlloc(
            ray, absorberRadius, results, absorberMaxDistance, 1 << LayerMask.NameToLayer("Minions"), QueryTriggerInteraction.Ignore);

        if (ammount > 0)
        {
            for (int i = 0; i < ammount; i++)
            {
                RaycastHit hit = results[i];
                Minion m = hit.collider.GetComponent<Minion>();

                if (!m.readyToHarvest) { continue; }

                Vector3 A = hit.collider.transform.position;
                Vector3 B = ray.GetPoint(0.0f);
                Vector3 d = ray.direction.normalized;
                Vector3 v = A - B;

                Vector3 P = B + Vector3.Dot(v, d) * d;

                Vector3 dir = P - A;

                Transform t = hit.collider.transform;
                Debug.DrawRay(t.position, dir, Color.blue);

                Rigidbody rb = hit.collider.GetComponent<Rigidbody>();

                float factor = Mathf.Clamp01(1.0f - hit.distance / absorberMaxDistance);

                Vector3 vel = rb.velocity;
                vel.y = 0.0f;
                rb.velocity = vel;

                rb.AddTorque(120.0f * Random.insideUnitSphere * Time.deltaTime, ForceMode.Force);
                rb.MovePosition(Vector3.MoveTowards(rb.position, P, 0.07f));
                rb.AddForce((head.position - rb.position).normalized * factor * Time.deltaTime * pullForce, ForceMode.Force);
            }
        }
    }

    void Update()
    {
        Absorbing = Input.GetButton("Fire2");
        bool firingIsDown = Input.GetButton("Fire1");

        if (ReadyToUse && firingIsDown && gameManager.GetAmmo() > 0.0f)
        {
            if (!firing && canFire)
            {
                if (gameManager.GetAmmo() > 0)
                {
                    firing = true;
                    fireRatePerSecond = baseFireRatePerSecond;
                    StartCoroutine(RestartFireDelay());
                    fireCoroutine = StartCoroutine(Fire());
                }
                else
                {
                    firing = false;
                    fireRatePerSecond = baseFireRatePerSecond;
                    if (fireCoroutine != null) { StopCoroutine(fireCoroutine); }
                }
            }
        }
        else
        {
            firing = false;
            fireRatePerSecond = baseFireRatePerSecond;
            if (fireCoroutine != null) { StopCoroutine(fireCoroutine); }
        }

        animator.SetBool("pull", false);
        animator.SetBool("activate", false);
        animator.SetBool("deactivate", false);

        if (Absorbing)
        {
            animator.SetBool("pull", true);
        }

        autoDeactivateWeaponTimer -= Time.deltaTime;
        if (ReadyToUse && autoDeactivateWeaponTimer <= 0.0f)
        {
            DeactivateWeapon();
        }

        if (ReadyToUse && (Absorbing || firingIsDown))
        {
            autoDeactivateWeaponTimer = autoDeactivateWeaponTimeInSeconds;
        }
        else if (!weaponOut && (Absorbing || firingIsDown))
        {
            weaponOut = true;
            animator.SetBool("activate", true);
            gameManager.audioManager.PlayOneShot(gameManager.audioManager.absorberOut, transform.position);
        }

        if (!playerController.FreeCamera)
        {
            absorbParticleSystem.transform.parent.LookAt(tps.FocalPoint);
        }

        audioSource.volume = 0.5f * gameManager.audioManager.m_soundVolume;
    }

    private void WeaponHidden()
    {
        weaponOut = false;
    }

    private void DeactivateWeapon()
    {
        ReadyToUse = false;
        animator.SetBool("deactivate", true);
        gameManager.audioManager.PlayOneShot(gameManager.audioManager.absorberIn, transform.position);
    }

    IEnumerator RestartFireDelay()
    {
        canFire = false;
        yield return new WaitForSeconds(fireRatePerSecond);
        canFire = true;
    }

    IEnumerator Fire()
    {
        InstantiateBullet();

        while (true)
        {
            yield return new WaitForSeconds(fireRatePerSecond);
            fireRatePerSecond = Mathf.Max(maxFireRatePerSecond, fireRatePerSecond * 0.7f);
            InstantiateBullet();
        }
    }

    public void WeaponReadyToUse()
    {
        ReadyToUse = true;
        autoDeactivateWeaponTimer = autoDeactivateWeaponTimeInSeconds;
    }

    private void InstantiateBullet()
    {
        gameManager.DecreaseAmmo();

        animator.SetTrigger("throw");

        Minion newBullet = MinionsPool.Instance.Get(true);
        newBullet.DisableHarvestingForSeconds(1.0f);

        newBullet.transform.SetPositionAndRotation(spawnPoint.position, spawnPoint.rotation);

        Vector2 noise = 0.05f * Random.insideUnitCircle;

        Vector3 direction;
        if (playerController.FreeCamera)
        {
            direction = absorbParticleSystem.transform.parent.forward +
            absorbParticleSystem.transform.parent.right * noise.x +
            absorbParticleSystem.transform.parent.up * noise.y;
        }
        else
        {
            direction =
            Quaternion.AngleAxis(-pushAngle, absorbParticleSystem.transform.parent.right) * absorbParticleSystem.transform.parent.forward +
            Camera.main.transform.right * noise.x +
            Camera.main.transform.up * noise.y;
        }

        newBullet.AddForce(direction * pushForce, ForceMode.Impulse);
        newBullet.AddTorque(torque * Random.insideUnitSphere, ForceMode.Impulse);

        gameManager.audioManager.PlayOneShot(gameManager.audioManager.absorberPush, transform.position);
        gameManager.audioManager.PlayOneShot(gameManager.audioManager.thrownMinion[Random.Range(0, 3)], transform.position);

        pushParticleSystem.Play();
    }

    public void BarkBark()
    {
    }
}
