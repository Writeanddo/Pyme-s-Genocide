using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionSpawner : MonoBehaviour
{
    [SerializeField] float spawnTime = 3.0f;
    [SerializeField] float range = 10.0f;

    void Start()
    {
        InvokeRepeating("Spawn", spawnTime, spawnTime);
    }


    void Spawn()
    {
        Vector3 randomPos = new Vector3(Random.Range(-10, 10), transform.position.y, Random.Range(-10, 10));
        transform.LookAt(randomPos);

        Minion enemy = MinionsPool.Instance.Get();
        if (enemy == null)
        {
            return;
        }


        Vector3 position = transform.position + transform.forward * Random.Range(0, 10);

        enemy.transform.localScale = Vector3.zero;
        enemy.transform.SetPositionAndRotation(position, transform.rotation);
    }
}
