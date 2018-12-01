using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Absorber : MonoBehaviour
{

    GameManager gameManager;
    [SerializeField] int force = 100;
    [SerializeField] float torque;

    [SerializeField] MeshRenderer rayRenderer;

    public enum Type { delete, force };
    public Type type;
    public GameObject bullet;

    bool absorbiendo;

    bool canFire = true;
    bool firing;
    Coroutine fireCoroutine;

    float fireRatePerSecond;
    [SerializeField] float maxFireRatePerSecond;
    [SerializeField] float baseFireRatePerSecond;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        if (type == Type.force)
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
        }

        absorbiendo = Input.GetButton("Fire2") && type == Type.force;
        if (type == Type.force)
        {
            rayRenderer.enabled = absorbiendo;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (type == Type.force && absorbiendo)
        {
            Vector3 heading = other.transform.position - transform.position;
            Rigidbody r = other.gameObject.GetComponent<Rigidbody>();
            if (r) r.AddForce(heading * -force, ForceMode.Force);
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (type == Type.delete && gameManager.GetAmmo() < gameManager.MaxAmmo)
        {
            col.gameObject.SetActive(false);
            gameManager.IncreaseAmmo();
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
        GameObject newBullet = Instantiate(bullet, transform.position, transform.rotation);

        Vector2 noise = 0.05f * Random.insideUnitCircle;
        Vector3 direction = transform.forward + transform.right * noise.x + transform.up * noise.y;

        newBullet.GetComponent<Rigidbody>().AddForce(direction * force);
        newBullet.GetComponent<Rigidbody>().AddTorque(torque * Random.insideUnitSphere);
    }
}
