using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Absorber : MonoBehaviour {

    public int force = 100;
    public enum Type { detele, force };
    public Type type;
    public GameObject bullet;

    void Update() {
        if (Input.GetButtonDown("Fire1") && type == Type.force) {
            GameObject newBullet = Instantiate(bullet, transform.position, transform.rotation);
            newBullet.GetComponent<Rigidbody>().AddForce(newBullet.transform.forward * force * 20);
        }

        /* if (Input.GetMouseButtonDown(1))
             Debug.Log("Pressed secondary button.");

         if (Input.GetMouseButtonDown(2))
             Debug.Log("Pressed middle click.");*/
    }

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
