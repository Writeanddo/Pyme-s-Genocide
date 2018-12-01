using UnityEngine;

public class Jumper : MonoBehaviour {

    public float power = 150f;

    private void OnTriggerEnter(Collider other)
    {
        var rigidbody = other.gameObject.GetComponent<Rigidbody>();
        if (rigidbody) rigidbody.AddForce(transform.forward * power, ForceMode.Impulse);
    }
}
