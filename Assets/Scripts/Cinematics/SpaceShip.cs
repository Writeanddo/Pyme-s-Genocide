using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceShip : MonoBehaviour {
    public GameObject phantom;
    bool isEjecting = false;
    float velocity = 0.0f;
    public float acceleration = 10.0f;
    private void Start() {
        phantom.SetActive(false);
    }
    public void Eject() {
        isEjecting = true;
        phantom.SetActive(true);
    }
    private void FixedUpdate() {
        if (isEjecting) {
            velocity = velocity + (Time.deltaTime * acceleration);
            //this line feeds the ever-growing force to the object, propelling it faster and faster over time
            transform.Translate(0, 0, velocity);
        }
    }
}
