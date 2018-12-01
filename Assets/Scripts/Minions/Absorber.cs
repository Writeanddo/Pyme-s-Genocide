using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Absorber : MonoBehaviour {

    GameManager gameManager;
    public int force = 100;
    public enum Type { detele, force };
    public Type type;
    public GameObject bullet;

    private void Start() {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void Update() {
        if (Input.GetMouseButtonDown(0) && type == Type.force && gameManager.GetAmmo() > 0) {
            gameManager.DecreaseAmmo();
            GameObject newBullet = Instantiate(bullet, transform.position, transform.rotation);
            newBullet.GetComponent<Rigidbody>().AddForce(newBullet.transform.forward * force * 20);
        }
    }

    private void OnTriggerStay(Collider other) {
        if (type == Type.force) {
            Vector3 heading = other.transform.position - transform.position;
            Rigidbody r = other.gameObject.GetComponent<Rigidbody>();
            if (r) r.AddForce(heading * -force);
        }
    }

    private void OnTriggerEnter(Collider col) {
        if (type == Type.detele) {
            col.gameObject.SetActive(false);
            gameManager.IncreaseAmmo();
        }
    }
}
