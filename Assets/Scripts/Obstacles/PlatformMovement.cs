using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMovement : MonoBehaviour {

    public Transform positions;
    public List<Vector3> childPositions;

    public float velocity = 0.5f;
    private int currentPosition = 0;
    private int nextPosition;
    private bool fordwardMovement = true;
    private float distanceThreshold = 0.5f;

    // Use this for initialization
    void Start () {
        foreach (Transform childTransform in positions)
        {
            childPositions.Add(childTransform.position);
        }

        if (childPositions.Count > 1) nextPosition = currentPosition + 1;
    }
	
    private void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, childPositions[nextPosition], velocity * Time.deltaTime);
        UpdateCurrentPosition();
    }

    private void UpdateCurrentPosition()
    {
        if(Vector3.Distance(transform.position, childPositions[nextPosition]) < distanceThreshold)
        {
            CalculateNextPosition();
        }
    }

    private void CalculateNextPosition()
    {
        currentPosition = nextPosition;

        if (fordwardMovement)
        {
            if (currentPosition == childPositions.Count - 1)
            {
                fordwardMovement = false;
                nextPosition--;
            }
            else nextPosition++;
        }
        else
        {
            if (currentPosition == 0)
            {
                fordwardMovement = true;
                nextPosition++;
            }
            else nextPosition--;
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




}
