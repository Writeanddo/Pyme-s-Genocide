using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] float m_Speed = 5.0f;
    [SerializeField] float m_Jump = 15.0f;
    [SerializeField] float m_Gravity = -20.0f;

    private Transform m_Cam;
    private Vector3 m_CamForward;

    CharacterController m_CharacterController;
    Vector3 m_movement;

    private float y;

    private Jetpack jetPack;
    private bool readyForJetpack;

    void Start()
    {
        m_CharacterController = GetComponent<CharacterController>();
        jetPack = GetComponentInChildren<Jetpack>();
        m_Cam = Camera.main.transform;
    }

    void Update()
    {
        m_movement = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));

        m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;
        m_movement = Input.GetAxis("Vertical") * m_CamForward + Input.GetAxis("Horizontal") * m_Cam.right;
        m_movement *= m_Speed;

        if (m_CharacterController.isGrounded)
        {
            HandleGroundedInput();
        }
        else
        {
            HandleAirborneInput();
        }

        m_movement.y = y;
        m_CharacterController.Move(m_movement * Time.deltaTime);
    }

    private void HandleGroundedInput()
    {
        // y = 0.0f;

        if (Input.GetButtonDown("Jump"))
        {
            y = m_Jump;
        }

        readyForJetpack = false;
    }

    private void HandleAirborneInput()
    {
        y += m_Gravity * Time.deltaTime;

        if (!readyForJetpack && y <= 0.0f)
        {
            readyForJetpack = true;
        }

        if (readyForJetpack && Input.GetButton("Jump"))
        {
            y += jetPack.Strength * Time.deltaTime;
        }
    }
}
