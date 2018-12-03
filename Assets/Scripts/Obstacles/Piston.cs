using UnityEngine;

public class Piston : MonoBehaviour {

    [SerializeField] bool activable = false;

    private enum pistonState { charge, ready, launch}
    private pistonState currentPistonState;

    [SerializeField] float delayedTime = 0f;
    private bool canStart = false;

    private Vector3 chargedPosition;
    [SerializeField] float chargeVelocity = 0.3f;

    private bool isReady = false;

    private Vector3 launchPosition;
    [SerializeField] float launchPower = 10f;
    [SerializeField] float maxDistance = 3f;

    // Use this for initialization
    void Start () {
        currentPistonState = pistonState.charge;
        chargedPosition = transform.TransformPoint(-Vector3.forward);
        launchPosition = transform.TransformPoint(Vector3.forward * maxDistance);
        if (!activable) Invoke("Initiate", delayedTime);
    }

	
	// Update is called once per frame
	void FixedUpdate () {

        if (!canStart) return;

        //Debug.Log(currentPistonState);
        switch (currentPistonState)
        {
            case pistonState.charge:
                transform.position = Vector3.Lerp(transform.position, chargedPosition, chargeVelocity * Time.deltaTime);
                if (Vector3.Distance(transform.position, chargedPosition) < 0.5f)
                {
                    currentPistonState = pistonState.ready;
                    //Debug.Log(currentPistonState);
                }
                break;
            case pistonState.ready:
                if (!isReady)
                {
                    isReady = true;
                    Invoke("ReadyCooldown", 1f);
                }
                break;
            case pistonState.launch:
                transform.Translate(Vector3.forward * launchPower * Time.deltaTime);
                //transform.position = Vector3.Lerp(transform.position, launchPosition, launchPower);
                if (Vector3.Distance(transform.position, launchPosition) < 0.5f)
                {
                    currentPistonState = pistonState.charge;
                    //Debug.Log(currentPistonState);
                }
                break;
            default:
                break;
        }
    }

    private void ReadyCooldown() {
        isReady = false;
        currentPistonState = pistonState.launch;
        //Debug.Log(currentPistonState);
    }

    private void Initiate() { canStart = true; }
}
