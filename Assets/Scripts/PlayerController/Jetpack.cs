
using UnityEngine;

public class Jetpack : MonoBehaviour
{
    [SerializeField] float m_strength;
    public float Strength { get { return m_strength; } }

    [SerializeField] int m_AmmoPerSecond;
    public int AmmoPerSecond { get { return m_AmmoPerSecond; } }

    [SerializeField] float m_PushForce;
    [SerializeField] float m_PushTorque;

    float minionsSpawned;
    int totalMinionsSpawned;

    private void Update()
    {
        if (!Input.GetButton("Jump"))
        {
            minionsSpawned = 0;
            totalMinionsSpawned = 0;
        }
    }

    public void SpawnMinions()
    {
        float t = Time.deltaTime;
        minionsSpawned += m_AmmoPerSecond * t;

        if (Mathf.Ceil(minionsSpawned) > totalMinionsSpawned)
        {
            int spawnAmmount = (int)Mathf.Ceil(minionsSpawned) - totalMinionsSpawned;

            for (int i = 0; i < spawnAmmount; i++)
            {
                Minion m = MinionsPool.Instance.Get(true);
                m.transform.position = transform.position;
                m.transform.rotation = Random.rotation;
                m.AddForce(m_PushForce * Vector3.down, ForceMode.Impulse);
                m.AddTorque(m_PushTorque * Random.insideUnitSphere, ForceMode.Impulse);
            }

            totalMinionsSpawned = (int)Mathf.Ceil(minionsSpawned);
        }

    }
}
