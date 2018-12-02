using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phantom : MonoBehaviour {

    GameObject player;

	void Start () {
        player = GameObject.Find("Player");
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        transform.LookAt(player.transform.position);
	}
}
