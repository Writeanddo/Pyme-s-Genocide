using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalPower : MonoBehaviour {

    private int life = 5;
    private bool destroyed = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Minion")
        {
            life--;
            Destroy(collision.gameObject);
            if (!destroyed && life <= 0)
            {
                destroyed = true;
                GetComponentInParent<ForceShield>().CrystalDestroyed();
                Destroy(gameObject);
            }
        }
    }
}
