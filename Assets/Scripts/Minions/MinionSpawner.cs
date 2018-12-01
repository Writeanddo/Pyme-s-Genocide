using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionSpawner : MonoBehaviour {

    public GameObject enemy;
    public float spawnTime = 3f;
    public float range = 10f;

    void Start() {
        InvokeRepeating("Spawn", spawnTime, spawnTime);
    }

    void Spawn() {
        Vector3 randomPos = new Vector3(Random.Range(-10, 10), transform.position.y, Random.Range(-10, 10));
        transform.LookAt(randomPos);

        Minion.Type type;
        float percent = Random.Range(0, 100);
        if(percent < 30) 
            type = Minion.Type.Coward;
        else if(percent < 80)
            type = Minion.Type.Crazy;
        else type = Minion.Type.Follower;

        enemy.GetComponent<Minion>().type = type;
        enemy.GetComponent<Minion>().detectionDist = Random.Range(6, 10);

        Vector3 position = transform.position;
            position = position + transform.forward * Random.Range(0, 10);
            Instantiate(enemy, position, transform.rotation);
        }
}
