using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Absorber : MonoBehaviour {

    public int force = 100;
    public enum Type { detele, force };
    public Type type;

    private void OnTriggerStay(Collider other) {
        if (type == Type.force) {
            Vector3 heading = other.transform.position - transform.position;
            Rigidbody r = other.gameObject.GetComponent<Rigidbody>();
            if (r) r.AddForce(heading * -force);
        }
    }

    private void OnTriggerEnter(Collider col) {
        if (type == Type.detele)
            col.gameObject.SetActive(false);
    }
}
