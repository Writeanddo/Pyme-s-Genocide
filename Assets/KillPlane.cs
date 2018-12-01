using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillPlane : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Minion m = other.GetComponent<Minion>();
        if (m)
        {
            MinionsPool.Instance.Put(m);
        }
    }
}
