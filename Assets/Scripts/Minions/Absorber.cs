using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Absorber : MonoBehaviour
{

    GameManager gameManager;
    [SerializeField] int force = 100;
    [SerializeField] MeshRenderer rayRenderer;

    public enum Type { delete, force };
    public Type type;
    public GameObject bullet;

    bool absorbiendo;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1") && type == Type.force && gameManager.GetAmmo() > 0)
        {
            gameManager.DecreaseAmmo();
            GameObject newBullet = Instantiate(bullet, transform.position, transform.rotation);
            newBullet.GetComponent<Rigidbody>().AddForce(newBullet.transform.forward * force * 20);
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
}
