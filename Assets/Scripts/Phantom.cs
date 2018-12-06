using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Phantom : MonoBehaviour {

    GameObject player;
    public bool deathEnabled = false;
    public float velocity = 1f;

	void Start () {
        player = GameObject.Find("Player");
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        transform.LookAt(player.transform.position);
        if (deathEnabled) {
            //move towards the player
            transform.position += transform.forward * velocity * Time.deltaTime;
        }
	}

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Player")
            SceneManager.LoadScene(0);
    }
}
