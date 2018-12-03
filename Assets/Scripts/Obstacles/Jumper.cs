using UnityEngine;

public class Jumper : MonoBehaviour
{
    public Animator jumperAnim;
    public float power = 150f;

    private void OnTriggerEnter(Collider other)
    {
        var rigidbody = other.gameObject.GetComponent<Rigidbody>();
        if (rigidbody)
        {
            GetComponentInChildren<Animator>().Play("Boing");
            var playerController = other.GetComponent<PlayerControllerRB>();
            if (playerController)
            {
                Vector3 forward = transform.forward;

                PlayerControllerExternalForce force = new PlayerControllerExternalForce
                {
                    force = transform.forward * power,
                    mode = ForceMode.Impulse,
                    resetVelocity = true,
                    resetVelocityDirection = forward
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
