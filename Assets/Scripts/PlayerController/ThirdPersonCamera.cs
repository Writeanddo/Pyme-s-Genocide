using UnityEngine;
using System.Collections;

public class ThirdPersonCamera : MonoBehaviour
{
    public bool lockCursor;

    [SerializeField] Vector2 mouseSensitivityRange = new Vector2(5.0f, 15.0f);

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

        defaultPos = transform.position;
        defaultRot = transform.rotation;
    }

    public Vector3 FocalPoint { get; private set; }

    Vector3 defaultPos;
    Quaternion defaultRot;

    public void Restart()
    {
        transform.position = defaultPos;
        transform.rotation = defaultRot;
        yaw = 0.0f;
        pitch = 0.0f;
    }

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
        if (Physics.Raycast(ray.origin, cam.transform.forward, out hit, Mathf.Infinity, layerMask, QueryTriggerInteraction.Ignore))
        {
            FocalPoint = hit.point;
        }
        else
        {
            FocalPoint = 10000.0f * cam.transform.forward;
        }

        yaw += Input.GetAxis("Mouse X") * (mouseSensitivityRange.x + OptionsObject.Instance.sensibility * (mouseSensitivityRange.y - mouseSensitivityRange.x));
        pitch -= Input.GetAxis("Mouse Y") * (mouseSensitivityRange.x + OptionsObject.Instance.sensibility * (mouseSensitivityRange.y - mouseSensitivityRange.x));
        pitch = Mathf.Clamp(pitch, pitchMinMax.x, pitchMinMax.y);

        currentRotation = Vector3.SmoothDamp(currentRotation, new Vector3(pitch, yaw), ref rotationSmoothVelocity, rotationSmoothTime);
        transform.eulerAngles = currentRotation;

        currentPosition = Vector3.SmoothDamp(currentPosition, target.position - transform.forward * dstFromTarget, ref positionSmoothVelocity, positionSmoothTime);
        transform.position = currentPosition;
    }
}