using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalPower : MonoBehaviour {

    public GameObject particleDamage;
    public GameObject particleDestroy;
    private int life = 5;
    private bool destroyed = false;

    GameManager gm;

    private void Start()
    {
        gm = FindObjectOfType<GameManager>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Minion m = collision.gameObject.GetComponent<Minion>();

        if (m)
        {
            life--;
            m.Explode(collision.contacts[0].point);
            Instantiate(particleDamage, transform.position, Quaternion.identity);
            gm.audioManager.PlayOneShot(gm.audioManager.brokenGlass[Random.Range(0, gm.audioManager.brokenGlass.Length)], transform.position);

            if (!destroyed && life <= 0)
            {
                destroyed = true;
                GetComponentInParent<ForceShield>().CrystalDestroyed();
                Instantiate(particleDestroy, transform.position, Quaternion.identity);

                gm.audioManager.PlayOneShot(gm.audioManager.spaceshipCollected, transform.position);

                Destroy(gameObject);
            }
        }
    }
}
