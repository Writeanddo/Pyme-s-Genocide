using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    private static AudioManager instance;
    public static AudioManager GetInterfaceController() { return instance; }

    public AudioClip ambienMusic;

    [Header("SFx")]
    public AudioClip playerJump;
    public AudioClip[] playerRun;
    public AudioClip playerShoot;
    public AudioClip playerJetpack;

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
