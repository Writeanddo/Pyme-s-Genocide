using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class DropPlatform : MonoBehaviour {

    [SerializeField] float shakeAmount = 0.02f;
    [SerializeField] float cooldownDrop = 0.5f;
    [SerializeField] float cooldownReturnToOrigin = 4f;
    [SerializeField] float returnVelocity = 1f;

    private Vector3 originalPos;

    private bool canShake = false;
    private bool droped = false;
    private bool isReturning = false;

    private void Start()
    {
        originalPos = transform.position;
    }
       

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            StartShake();
            Invoke("Drop", cooldownDrop);
        }
    }
    private void StartShake() { canShake = true; }

    private void Update()
    {
        if (canShake) transform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;
    }

    private void FixedUpdate()
    {
        if (isReturning)
        {
            transform.position = Vector3.Lerp(transform.position, originalPos, returnVelocity * Time.deltaTime);
            if (Vector3.Distance(transform.position, originalPos) < 1f) isReturning = false;
        }
    }

    private void Drop()
    {
        if (!droped)
        {
            canShake = false;
            droped = true;
            GetComponent<Rigidbody>().isKinematic = false;
            Invoke("ReturnToOriginPosition", cooldownReturnToOrigin);
        }
    }

    private void ReturnToOriginPosition()
    {
        GetComponent<Rigidbody>().isKinematic = true;
        isReturning = true;
        droped = false;
    }
}
