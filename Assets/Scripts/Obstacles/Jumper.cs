using UnityEngine;

public class Jumper : MonoBehaviour
{
    Animator animator;
    public float power = 150f;

    GameManager gm;

    private void Start()
    {
        gm = FindObjectOfType<GameManager>();
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        var rigidbody = other.gameObject.GetComponent<Rigidbody>();
        if (rigidbody)
        {
            animator.Play("Boing");
            gm.audioManager.PlayOneShot(gm.audioManager.boing, transform.position);

            var playerController = other.GetComponent<PlayerControllerRB>();
            if (playerController)
            {
                Vector3 forward = transform.up;

                PlayerControllerExternalForce force = new PlayerControllerExternalForce
                {
                    force = transform.up * power,
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
