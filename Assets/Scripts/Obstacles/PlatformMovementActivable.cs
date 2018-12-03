using System.Collections.Generic;
using UnityEngine;

public class PlatformMovementActivable : IActivable {

    public Transform positions;
    private List<Vector3> childPositions;

    public bool active = false;

    private int nextPosition = 0;

    [SerializeField] float velocity = 0.5f;
    [SerializeField] float distanceThreshold = 1f;

    // Use this for initialization
    void Start () {
        childPositions = new List<Vector3>();
        foreach (Transform childTransform in positions)
        {
            childPositions.Add(childTransform.position);
        }
    }
	
    private void FixedUpdate()
    {
        nextPosition = active ? 0 : 1;
        transform.position = Vector3.Lerp(transform.position, childPositions[nextPosition], velocity * Time.deltaTime);
        UpdateCurrentPosition();
    }

    private void UpdateCurrentPosition()
    {
        if(Vector3.Distance(transform.position, childPositions[nextPosition]) < distanceThreshold)
        {
            nextPosition = nextPosition == 1 ? 0 : 1;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        other.transform.parent = transform;
    }

    private void OnTriggerExit(Collider other)
    {
        other.transform.parent = null;
    }

    public override void Activate() { active = !active; }
}
