using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AspiradoraMainMenu : MonoBehaviour
{
    GameManager gm;

    private void Start()
    {
        gm = FindObjectOfType<GameManager>();    
    }

    public void BarkBark()
    {
        // gm.audioManager.PlayOneShot(gm.audioManager.absorberBark, Camera.main.transform.position);
    }
}
