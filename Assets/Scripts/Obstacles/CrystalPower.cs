using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalPower : MonoBehaviour {

    public GameObject particleDamage;
    public GameObject particleDestroy;
    private int life = 5;
    private bool destroyed = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Minion")
        {
            life--;
            Instantiate(particleDamage, transform.position, Quaternion.identity);
            Destroy(collision.gameObject);
            if (!destroyed && life <= 0)
            {
                destroyed = true;
                GetComponentInParent<ForceShield>().CrystalDestroyed();
                Instantiate(particleDestroy, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
        }
    }
}
