using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Capturer : MonoBehaviour
{
    GameManager gm;

    private void Start()
    {
        gm = FindObjectOfType<GameManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (gm.GetAmmo() < gm.MaxAmmo)
        {
            Minion m = other.GetComponent<Minion>();
            if (m)
            {
                gm.IncreaseAmmo();
                MinionsPool.Instance.Put(m);
            }
        }
    }
}
