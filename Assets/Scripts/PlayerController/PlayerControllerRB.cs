
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerRB : MonoBehaviour
{
    [SerializeField] float m_Speed = 5.0f;
    [SerializeField] float m_JumpForce = 4000.0f;
    [SerializeField] float m_GroundCheckDistance = 1.2f;
    [SerializeField] float m_TurnSmoothTime;

    private float m_CurrentSpeed;

    private bool m_WantsToJump;
    private bool m_WantsToJetpack;

    private Transform m_Cam;

    Rigidbody m_RigidBody;
    Vector3 m_movement;

    private float y;

    private bool m_IsGrounded;
    private Vector3 m_GroundNormal;

    private Jetpack jetPack;
    private bool readyForJetpack;

    private float m_turnSmoothVelocity;

    private float m_TargetRotation;

    private List<PlayerControllerExternalForce> externalForces = new List<PlayerControllerExternalForce>();

    void Start()
    {
        m_RigidBody = GetComponent<Rigidbody>();
        jetPack = GetComponentInChildren<Jetpack>();
        m_Cam = Camera.main.transform;
    }

    void Update()
    {
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Vector2 inputDir = input.normalized;

        if (inputDir != Vector2.zero)
        {
            m_TargetRotation = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg + m_Cam.eulerAngles.y;
        }

        m_CurrentSpeed = m_Speed * inputDir.magnitude;

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

        m_RigidBody.rotation = Quaternion.Euler(Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y,
            m_TargetRotation,
            ref m_turnSmoothVelocity,
            m_TurnSmoothTime));

        if (externalForces.Count == 0)
        {
            m_RigidBody.velocity = new Vector3(
                transform.forward.x * m_CurrentSpeed,
                m_RigidBody.velocity.y,
                transform.forward.z * m_CurrentSpeed);
        }
        else
        {
            m_RigidBody.MovePosition(m_RigidBody.position + new Vector3(
            transform.forward.x * m_CurrentSpeed * Time.deltaTime,
            0,
            transform.forward.z * m_CurrentSpeed * Time.deltaTime));

            for (int i = 0; i < externalForces.Count; i++)
            {
                m_RigidBody.AddForce(externalForces[i].force, externalForces[i].mode);
            }
        }

        externalForces.Clear();

        m_WantsToJump = false;
        m_WantsToJetpack = false;
    }

    public void AddExternalForce(PlayerControllerExternalForce force)
    {
        externalForces.Add(force);
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

public class PlayerControllerExternalForce
{
    public Vector3 force;
    public ForceMode mode;
}