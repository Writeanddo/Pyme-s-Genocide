using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JetpackPipe : MonoBehaviour
{
    [SerializeField] Jetpack jetpack;

    public Transform spawnPoint;

    public void SetReady()
    {
        jetpack.SetReady();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(spawnPoint.position, 0.1f * Vector3.one);
    }
}
