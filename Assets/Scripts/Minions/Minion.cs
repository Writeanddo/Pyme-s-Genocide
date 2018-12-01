using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minion : MonoBehaviour
{

    [SerializeField] int moveSpeed = 3;

    public int detectionDist = 6;

    public enum Type { Follower, Coward, Crazy }
    public Type type;

    Transform playerTransform;
    private int rotationSpeed = 6;
    float counter = 3;
    Vector3 randomPos;

    Transform _transform;

    void Start()
    {
        _transform = transform;
        playerTransform = GameObject.FindWithTag("Player").transform;
    }

    void Update()
    {

        float sqrDistance = (playerTransform.position - _transform.position).sqrMagnitude;

        if (sqrDistance < detectionDist * detectionDist)
        {
            Vector3 targetPos = new Vector3(playerTransform.position.x, _transform.position.y, playerTransform.position.z);
            // Debug.DrawLine(playerTransform.position, _transform.position, Color.red, Time.deltaTime, false);
            switch (type)
            {
                case Type.Follower:
                    transform.rotation = Quaternion.Lerp(_transform.rotation,
                    Quaternion.LookRotation(targetPos - _transform.position), rotationSpeed * Time.deltaTime);
                    _transform.position += _transform.forward * moveSpeed * Time.deltaTime;
                    break;
                case Type.Coward:
                    _transform.rotation = Quaternion.Lerp(_transform.rotation,
                    Quaternion.LookRotation(_transform.position - targetPos), rotationSpeed * Time.deltaTime);

                    _transform.position += _transform.forward * moveSpeed * Time.deltaTime;
                    break;
                case Type.Crazy:
                    counter += Time.deltaTime;
                    if (counter > 0.5f)
                    {
                        randomPos = new Vector3(Random.Range(-10, 10), _transform.position.y, Random.Range(-10, 10));
                        counter = 0;
                    }

                    Quaternion rotation = Quaternion.LookRotation(randomPos - _transform.position, Vector3.up);
                    transform.rotation = Quaternion.Lerp(_transform.rotation, rotation, rotationSpeed * Time.deltaTime);

                    Vector3 heading = randomPos - _transform.position;
                    heading.y = 0;
                    float distance = heading.magnitude;
                    Vector3 direction = heading / distance;

                    _transform.position += _transform.forward * moveSpeed * Time.deltaTime;
                    break;
            }
        }
    }
}
