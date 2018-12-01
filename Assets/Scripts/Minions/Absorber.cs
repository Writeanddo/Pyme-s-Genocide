using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Absorber : MonoBehaviour {

    public int force = 100;
    public GameObject placeholder;

    private void Update() {
    //    transform.position = placeholder.transform.position;
    //    transform.rotation
    }

    private void OnTriggerStay(Collider other) {
        Vector3 heading = other.transform.position - transform.position;
        Rigidbody r = other.gameObject.GetComponent<Rigidbody>();
        if(r) r.AddForce(heading * -force);
    }

    private void OnCollisionEnter(Collision collision) {
        print("hola?");
        collision.gameObject.SetActive(false);
    }
}
