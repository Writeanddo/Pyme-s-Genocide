using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceShield : MonoBehaviour {

    private int power;

	// Use this for initialization
	void Start () {
        power = transform.childCount;
    }
	
    public void CrystalDestroyed()
    {
        power--;
        if (power <= 0)
        {
            Destroy(gameObject);
        }
    }
}
