using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Wobble : MonoBehaviour
{

    [SerializeField] Vector3 direction;
    [SerializeField] float magnitude;
    [SerializeField] float period;
    [SerializeField] bool randomizeStart = true;

    Vector3 basePosition;

    float t;
    float phase;

    private void Start()
    {
        basePosition = transform.position;
        if (randomizeStart)
        {
            float y = Random.Range(-magnitude, magnitude);
            transform.position = transform.position + direction * y;
            phase = period * Mathf.Asin(y / magnitude);
        }
    }

    private void FixedUpdate()
    {
        transform.position = basePosition + direction * magnitude * Mathf.Sin((t - phase) / period);
        t += Time.deltaTime;
    }

    private void OnDrawGizmosSelected()
    {
        if (EditorApplication.isPlaying)
        {
            Gizmos.DrawLine(basePosition - direction * magnitude, basePosition + direction * magnitude);
            Gizmos.DrawWireSphere(basePosition - direction * magnitude, 0.1f);
            Gizmos.DrawWireSphere(basePosition + direction * magnitude, 0.1f);
        }
        else
        {
            Gizmos.DrawLine(transform.position - direction * magnitude, transform.position + direction * magnitude);
            Gizmos.DrawWireSphere(transform.position - direction * magnitude, 0.1f);
            Gizmos.DrawWireSphere(transform.position + direction * magnitude, 0.1f);
        }
    }
}
