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
                Minion m = other.gameObject.GetComponent<Minion>();
                if (m)
                {
                    gm.audioManager.PlayOneShot(gm.audioManager.thrownMinion[Random.Range(0, gm.audioManager.thrownMinion.Length)], transform.position);
                }
                rigidbody.AddForce(transform.up * 0.1f * power, ForceMode.Impulse);

                if (transform.up == Vector3.up)
                {
                    rigidbody.AddForce(transform.right * 0.01f * power, ForceMode.Impulse);
                }
            }
        }
    }
}
