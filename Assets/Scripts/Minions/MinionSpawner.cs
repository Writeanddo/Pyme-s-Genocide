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

        Minion.Type type;
        float percent = Random.Range(0, 100);
        if (percent < 30)
            type = Minion.Type.Coward;
        else if (percent < 80)
            type = Minion.Type.Crazy;
        else type = Minion.Type.Follower;

        Minion enemy = MinionsPool.Instance.Get();
        if (enemy == null)
        {
            return;
        }

        enemy.type = type;
        enemy.detectionDist = Random.Range(15, 20);
        Vector3 position = transform.position + transform.forward * Random.Range(0, 10);

        enemy.transform.SetPositionAndRotation(position, transform.rotation);
    }
}
