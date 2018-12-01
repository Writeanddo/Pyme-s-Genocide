using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minion : MonoBehaviour {

	public enum Type { Follower, Coward, Crazy }

    public Type type;

    Transform player; 
    public int moveSpeed = 3;
    private int rotationSpeed = 6;
    public int detectionDist = 6;
    float counter = 3;
    Vector3 randomPos;

    void Start() {
        player = GameObject.FindWithTag("Player").transform;
    }

    void Update() {
        float distancia = Vector3.Distance(player.transform.position, transform.position);
        Vector3 targetPos = new Vector3(player.position.x, this.transform.position.y, player.position.z);

        if (distancia < detectionDist) {
            Debug.DrawLine(player.transform.position, transform.position, Color.red, Time.deltaTime, false);
            switch (type) {
                case Type.Follower:
                    transform.rotation = Quaternion.Slerp(transform.rotation,
                    Quaternion.LookRotation(targetPos - transform.position), rotationSpeed * Time.deltaTime);

                    transform.position += transform.forward * moveSpeed * Time.deltaTime;
                    break;
                case Type.Coward:
                    transform.rotation = Quaternion.Slerp(transform.rotation,
                    Quaternion.LookRotation(transform.position - targetPos), rotationSpeed * Time.deltaTime);

                    transform.position += transform.forward * moveSpeed * Time.deltaTime;
                    break;
                case Type.Crazy:
                    counter += Time.deltaTime;
                    if(counter > 0.5f) {
                        randomPos = new Vector3(Random.Range(-10, 10), transform.position.y, Random.Range(-10, 10));
                        counter = 0;
                    }
          
                    Quaternion rotation = Quaternion.LookRotation(randomPos - transform.position, Vector3.up);
                    transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);

                    Vector3 heading = randomPos - transform.position;
                    heading.y = 0;
                    float distance = heading.magnitude;
                    Vector3 direction = heading / distance;

                    transform.position += transform.forward * moveSpeed * Time.deltaTime;
                    break;
            }
        }
    }
}
