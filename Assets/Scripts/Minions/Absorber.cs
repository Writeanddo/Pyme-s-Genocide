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

    [SerializeField] MeshRenderer rayRenderer;

    public Transform spawnPoint;

    Capturer capturer;

    bool absorbing;

    bool canFire = true;
    bool firing;
    Coroutine fireCoroutine;

    float fireRatePerSecond;
    [SerializeField] float maxFireRatePerSecond;
    [SerializeField] float baseFireRatePerSecond;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        capturer = GetComponentInChildren<Capturer>();
    }

    void Update()
    {
        if (Input.GetButton("Fire1"))
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

        absorbing = Input.GetButton("Fire2");
        rayRenderer.enabled = absorbing;
    }

    private void OnTriggerStay(Collider other)
    {
        if (absorbing)
        {
            Minion m = other.GetComponent<Minion>();
            if (m)
            {
                Vector3 heading = other.transform.position - capturer.transform.position;
                m.AddForce(heading * -pullForce * Time.deltaTime, ForceMode.Force);
            }
        }
    }

    IEnumerator RestartFireDelay()
    {
        canFire = false;
        yield return new WaitForSeconds(baseFireRatePerSecond);
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

    private void InstantiateBullet()
    {
        gameManager.DecreaseAmmo();

        Minion newBullet = MinionsPool.Instance.Get(true);
        newBullet.transform.SetPositionAndRotation(spawnPoint.position, spawnPoint.rotation);

        Vector2 noise = 0.05f * Random.insideUnitCircle;
        Vector3 direction =
            Quaternion.AngleAxis(-pushAngle, spawnPoint.right) * Camera.main.transform.forward +
            Camera.main.transform.right * noise.x +
            Camera.main.transform.up * noise.y;

        newBullet.AddForce(direction * pushForce, ForceMode.Impulse);
        newBullet.AddTorque(torque * Random.insideUnitSphere, ForceMode.Impulse);
    }
}
