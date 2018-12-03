using UnityEngine;
using System.Collections;

public class ThirdPersonCamera : MonoBehaviour
{
    public bool lockCursor;
    public float mouseSensitivity = 10;
    public Transform target;
    public float dstFromTarget = 2;
    public Vector2 pitchMinMax = new Vector2(-40, 85);

    public float rotationSmoothTime = .12f;
    Vector3 rotationSmoothVelocity;
    Vector3 currentRotation;

    public float positionSmoothTime = .12f;
    Vector3 positionSmoothVelocity;
    Vector3 currentPosition;

    float yaw;
    float pitch;

    Transform playerTransform;

    Camera cam;

    void Start()
    {
        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        cam = Camera.main;
        playerTransform = FindObjectOfType<PlayerControllerRB>().transform;
    }

    public Vector3 FocalPoint { get; private set; }

    public void ManualUpdate()
    {
        if (target == null)
        {
            transform.LookAt(playerTransform);
            return;
        }

        Ray ray = cam.ViewportPointToRay(new Vector3(.5f, .5f, 0));
        ray.origin = ray.GetPoint(3.0f);

        int layerMask = ~LayerMask.GetMask("Player");

        RaycastHit hit;
        // + forward para ponernos a distancia 0 del player y no colisionar con algo que está detrás
        if (Physics.Raycast(ray.origin, cam.transform.forward, out hit, 100.0f, layerMask, QueryTriggerInteraction.Ignore))
        {
            FocalPoint = hit.point;
        }
        else
        {
            FocalPoint = 100.0f * cam.transform.forward;
        }

        yaw += Input.GetAxis("Mouse X") * mouseSensitivity;
        pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        pitch = Mathf.Clamp(pitch, pitchMinMax.x, pitchMinMax.y);

        currentRotation = Vector3.SmoothDamp(currentRotation, new Vector3(pitch, yaw), ref rotationSmoothVelocity, rotationSmoothTime);
        transform.eulerAngles = currentRotation;

        currentPosition = Vector3.SmoothDamp(currentPosition, target.position - transform.forward * dstFromTarget, ref positionSmoothVelocity, positionSmoothTime);
        transform.position = currentPosition;
    }
}