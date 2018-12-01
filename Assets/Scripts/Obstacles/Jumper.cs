using UnityEngine;

public class Jumper : MonoBehaviour
{

    public float power = 150f;

    private void OnTriggerEnter(Collider other)
    {
        var rigidbody = other.gameObject.GetComponent<Rigidbody>();
        if (rigidbody)
        {
            var playerController = other.GetComponent<PlayerControllerRB>();
            if (playerController)
            {
                PlayerControllerExternalForce force = new PlayerControllerExternalForce
                {
                    force = transform.forward * power,
                    mode = ForceMode.Impulse,
                    resetVelocity = true
                };
                playerController.AddExternalForce(force);
            }
            else
            {
                rigidbody.AddForce(transform.forward * power, ForceMode.Impulse);
            }
        }
    }
}
