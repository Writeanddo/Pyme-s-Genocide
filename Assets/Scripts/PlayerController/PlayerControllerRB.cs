
using UnityEngine;

public class PlayerControllerRB : MonoBehaviour
{
    [SerializeField] float m_Speed = 5.0f;
    [SerializeField] float m_JumpForce = 4000.0f;
    [SerializeField] float m_GroundCheckDistance = 1.2f;

    private bool m_WantsToJump;
    private bool m_WantsToJetpack;

    private Transform m_Cam;
    private Vector3 m_CamForward;

    Rigidbody m_RigidBody;
    Vector3 m_movement;

    private float y;

    private bool m_IsGrounded;
    private Vector3 m_GroundNormal;

    private Jetpack jetPack;
    private bool readyForJetpack;

    private Quaternion m_TargetRotation;

    void Start()
    {
        m_RigidBody = GetComponent<Rigidbody>();
        jetPack = GetComponentInChildren<Jetpack>();
        m_Cam = Camera.main.transform;
    }

    void Update()
    {
        m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;
        m_movement = Input.GetAxisRaw("Vertical") * m_CamForward + Input.GetAxisRaw("Horizontal") * m_Cam.right;
        m_movement *= m_Speed;

        y = m_RigidBody.velocity.y;

        CheckGroundStatus();

        if (m_IsGrounded)
        {
            HandleGroundedInput();
        }
        else
        {
            HandleAirborneInput();
        }

        if (m_movement.x != 0.0f || m_movement.y != 0.0f)
        {
            m_TargetRotation = Quaternion.Euler(0.0f, Mathf.Rad2Deg * Mathf.Atan2(m_movement.x, m_movement.z), 0.0f);
        }
    }

    private void FixedUpdate()
    {
        if (m_WantsToJump)
        {
            m_RigidBody.AddForce(Vector3.up * m_JumpForce);
        }

        if (m_WantsToJetpack)
        {
            m_RigidBody.AddForce(Vector3.up * jetPack.Strength);
        }

        m_RigidBody.MoveRotation(Quaternion.RotateTowards(m_RigidBody.rotation, m_TargetRotation, 240.0f * Time.deltaTime));
        m_RigidBody.velocity = new Vector3(m_movement.x, y, m_movement.z);

        m_WantsToJump = false;
        m_WantsToJetpack = false;
    }

    private void HandleGroundedInput()
    {
        if (Input.GetButtonDown("Jump"))
        {
            m_WantsToJump = true;
        }

        readyForJetpack = false;
    }

    private void HandleAirborneInput()
    {
        if (!readyForJetpack && m_RigidBody.velocity.y <= 0.0f)
        {
            readyForJetpack = true;
        }

        if (readyForJetpack && Input.GetButton("Jump"))
        {
            m_WantsToJetpack = true;
        }
    }

    void CheckGroundStatus()
    {
        RaycastHit hitInfo;
#if UNITY_EDITOR
        // helper to visualise the ground check ray in the scene view
        Debug.DrawLine(transform.position + (Vector3.up * 0.1f), transform.position + (Vector3.up * 0.1f) + (Vector3.down * m_GroundCheckDistance));
#endif
        // 0.1f is a small offset to start the ray from inside the character
        // it is also good to note that the transform position in the sample assets is at the base of the character
        if (Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, out hitInfo, m_GroundCheckDistance))
        {
            m_GroundNormal = hitInfo.normal;
            m_IsGrounded = true;
        }
        else
        {
            m_IsGrounded = false;
            m_GroundNormal = Vector3.up;
        }
    }
}
