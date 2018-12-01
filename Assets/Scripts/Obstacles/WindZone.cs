using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindZone : MonoBehaviour {
    
    private float minPower = 0;
    public float maxPower = 2f;

    private void OnTriggerStay(Collider other)
    {
        var rigidbody = other.GetComponent<Rigidbody>();
        if (rigidbody) rigidbody.velocity += transform.forward * Random.Range(minPower, maxPower) * Time.deltaTime;
    }
}
