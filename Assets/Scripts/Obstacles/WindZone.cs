using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindZone : MonoBehaviour
{
    [SerializeField] float minPower = 0;
    [SerializeField] float maxPower = 2f;

    private void OnTriggerStay(Collider other)
    {
        var rigidbody = other.GetComponent<Rigidbody>();
        if (rigidbody)
        {
            var playerController = other.GetComponent<PlayerControllerRB>();
            if (playerController)
            {
                PlayerControllerExternalForce force = new PlayerControllerExternalForce
                {
                    force = transform.forward * Random.Range(minPower, maxPower) * Time.deltaTime,
                    mode = ForceMode.Force,
                    scale = true
                };
                playerController.AddExternalForce(force);
            }
            else
            {
                rigidbody.AddForce(transform.forward * Random.Range(minPower, maxPower) * Time.deltaTime, ForceMode.Force);
            }
        }
    }
}
