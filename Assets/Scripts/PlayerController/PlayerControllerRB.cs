
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerRB : MonoBehaviour
{
    [SerializeField] float m_GroundSpeed = 10.0f;
    [SerializeField] float m_AirSpeed = 8.0f;
    [SerializeField] float m_JumpForce = 4000.0f;
    [SerializeField] float m_GroundCheckDistance = 1.2f;
    [SerializeField] float m_TurnSmoothTime;
    [SerializeField] float m_GroundFriction;
    [SerializeField] float m_AirFriction;
    [SerializeField] float m_ExtraGravityFactor;
    [SerializeField] float m_MaxMovementVelocity;
    [SerializeField] float m_MaxGravityVelocity;

    private GameManager gameManager;

    private bool m_WantsToJump;
    private bool m_WantsToJetpack;

    private ThirdPersonCamera thirdPersonCamera;
    private Transform m_Cam;

    Rigidbody m_RigidBody;
    Vector3 m_movement;

    private bool m_IsGrounded;
    private Vector3 m_GroundNormal;

    private Jetpack jetPack;
    private bool readyForJetpack;

    private float m_turnSmoothVelocity;

    private float m_TargetRotation;

    private List<PlayerControllerExternalForce> externalForces = new List<PlayerControllerExternalForce>();

    private Vector3 m_CurrentVelocity;
    private Animator animator;

    public bool inputEnabled = true;

    public Vector3 RespawnPosition { get; private set; }

    public bool FreeCamera { get; private set; }

    void Start()
    {
        m_RigidBody = GetComponent<Rigidbody>();
        jetPack = GetComponentInChildren<Jetpack>();
        m_Cam = Camera.main.transform;
        thirdPersonCamera = FindObjectOfType<ThirdPersonCamera>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        animator = GetComponent<Animator>();

        RespawnPosition = transform.position;
    }

    void Update()
    {

        if (!inputEnabled) { return; }

        Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        Vector2 inputDir = input.normalized;

        m_CurrentVelocity = inputDir.x * m_Cam.right - inputDir.y * Vector3.Cross(Vector3.up, m_Cam.right);

        CheckGroundStatus();

        float speed = m_IsGrounded ? m_GroundSpeed : m_AirSpeed;
        m_CurrentVelocity = speed * m_CurrentVelocity.normalized;

        if (m_IsGrounded)
        {
            HandleGroundedInput();
        }
        else
        {
            HandleAirborneInput();
        }

        FreeCamera = Input.GetButton("Fire3") && inputDir.x == 0.0f && inputDir.y == 0.0f;

        if (!FreeCamera)
        {
            m_TargetRotation = m_Cam.eulerAngles.y;
        }

        animator.SetBool("running", false);
        animator.SetBool("strafing", false);
        animator.SetBool("jump", false);

        if (inputDir.x != 0.0f || inputDir.y != 0.0f)
        {
            animator.SetBool("running", true);
            animator.SetBool("running_backwards", inputDir.y < 0.0f);

            if (Mathf.Abs(inputDir.x) > 0.3f)
            {
                animator.SetBool("strafing", true);
                animator.SetFloat("strafe_direction", inputDir.x > 0 ? 1.0f : 0.0f);
            }
        }

        animator.SetBool("grounded", m_IsGrounded);

        if (m_IsGrounded && m_WantsToJump)
        {
            animator.SetBool("jump", true);
        }
    }

    private void FixedUpdate()
    {
        if (m_WantsToJump)
        {
            m_RigidBody.AddForce(Vector3.up * m_JumpForce);
        }

        if (m_WantsToJetpack && gameManager.GetAmmo() > 0)
        {
            jetPack.SpawnMinions();
            gameManager.DecreaseAmmo(jetPack.AmmoPerSecond * Time.deltaTime);
            m_RigidBody.AddForce(Vector3.up * jetPack.Strength);
        }

        m_RigidBody.rotation = Quaternion.Slerp(m_RigidBody.rotation, Quaternion.Euler(0.0f, m_TargetRotation, 0.0f), 15.0f * Time.deltaTime);
        m_RigidBody.AddForce(m_CurrentVelocity * Time.deltaTime, ForceMode.VelocityChange);

        for (int i = 0; i < externalForces.Count; i++)
        {
            if (externalForces[i].resetVelocity)
            {
                Vector3 v = m_RigidBody.velocity;
                if (externalForces[i].resetVelocityDirection.x != 0)
                {
                    v.x = 0.0f;
                }
                if (externalForces[i].resetVelocityDirection.y != 0)
                {
                    v.y = 0.0f;
                }
                if (externalForces[i].resetVelocityDirection.z != 0)
                {
                    v.z = 0.0f;
                }
                m_RigidBody.velocity = v;
            }

            if (externalForces[i].scale && m_IsGrounded)
            {
                m_RigidBody.AddForce(externalForces[i].force * 200.0f, externalForces[i].mode);
            }
            else
            {
                m_RigidBody.AddForce(externalForces[i].force, externalForces[i].mode);
            }
        }

        float friction = m_IsGrounded ? m_GroundFriction : m_AirFriction;
        Vector2 horizontalVelocity = new Vector3(m_RigidBody.velocity.x, m_RigidBody.velocity.z);
        if (friction == 0.0f)
        {
            friction = 1.0f;
        }

        Vector3 vel = transform.InverseTransformDirection(m_RigidBody.velocity);
        vel.x *= friction;
        vel.z *= friction;

        if (!m_IsGrounded)
        {
            if (m_RigidBody.velocity.y <= 0.0f)
            {
                if (m_WantsToJetpack)
                {
                    vel.y *= 0.99f;
                }
                else
                {
                    // vel.y *= m_ExtraGravityFactor;
                }
            }
        }

        vel.y = Mathf.Sign(vel.y) * Mathf.Min(Mathf.Abs(vel.y), m_MaxGravityVelocity);

        Vector2 hMov = new Vector2(vel.x, vel.z);
        hMov = hMov.normalized * Mathf.Min(hMov.magnitude, m_MaxMovementVelocity);
        vel.x = hMov.x;
        vel.z = hMov.y;

        m_RigidBody.velocity = transform.TransformDirection(vel);

        thirdPersonCamera.ManualUpdate();

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
        if (!readyForJetpack && jetPack.Ready && m_RigidBody.velocity.y <= 0.0f)
        {
            readyForJetpack = true;
        }

        if (readyForJetpack && Input.GetButton("Jump"))
        {
            jetPack.StartJetpack();
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
        if (Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, out hitInfo, m_GroundCheckDistance, -5, QueryTriggerInteraction.Ignore))
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
    public bool resetVelocity;
    public Vector3 resetVelocityDirection;
    public bool scale;
}