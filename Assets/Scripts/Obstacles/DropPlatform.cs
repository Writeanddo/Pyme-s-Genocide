using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class DropPlatform : MonoBehaviour {

    [SerializeField] float shakeAmount = 0.01f;
    [SerializeField] float cooldownDrop = 0.5f;
    [SerializeField] float cooldownReturnToOrigin = 4f;
    [SerializeField] float returnVelocity = 0.1f;

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
        if (!canShake && !isReturning)
            if (collision.gameObject.tag == "Player")
            {
                canShake = true;
                Invoke("Drop", cooldownDrop);
            }
    }

    //private void Update()
    //{
    //    if (canShake) transform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;
    //}

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
            if (returnVelocity > 0 && cooldownReturnToOrigin > 0) Invoke("ReturnToOriginPosition", cooldownReturnToOrigin);
            else Destroy(gameObject, 5f);
        }
    }

    private void ReturnToOriginPosition()
    {
        GetComponent<Rigidbody>().isKinematic = true;
        isReturning = true;
        droped = false;
    }
}
