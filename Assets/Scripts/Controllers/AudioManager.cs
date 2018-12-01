using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    private static AudioManager instance;
    public static AudioManager GetInterfaceController() { return instance; }

    [Header("AmbientMusic")]
    public AudioClip ambienMusic;

    //[Header("Scene")]

    void Awake()
    {
        instance = this;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
