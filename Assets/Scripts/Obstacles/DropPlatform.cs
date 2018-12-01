using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class DropPlatform : MonoBehaviour {

    private float cooldown = 0.5f;
    private bool droped = false;

    private void OnCollisionEnter(Collision collision)
    {
        Invoke("Drop", cooldown);
    }

    private void Drop()
    {
        if (!droped)
        {
            droped = true;
            GetComponent<Rigidbody>().isKinematic = false;
            Destroy(gameObject, 5f);
        }
    }
}
