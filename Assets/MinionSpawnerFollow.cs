using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionSpawnerFollow : MonoBehaviour
{
    [SerializeField] float radius = 20.0f;
    public float Radius { get { return radius; } }

    [SerializeField] float spawnRateInSecs = 1.0f;
    [SerializeField] float spawnRateDeviation = 0.1f;
    [SerializeField] bool spawnImmediately = true;

    [SerializeField] int retries = 10;

    float spawnRate;

    Transform playerTransform;
    Transform _transform;

    void Start()
    {
        spawnRate = Random.Range(spawnRateInSecs * (1 - spawnRateDeviation), spawnRateInSecs * (1 + spawnRateDeviation));
        playerTransform = FindObjectOfType<PlayerControllerRB>().transform;
        _transform = transform;

        _transform.position = new Vector3(playerTransform.position.x, 50.0f, playerTransform.position.z);

        StartCoroutine(Spawn());
    }

    void Update()
    {
        _transform.position = new Vector3(playerTransform.position.x, _transform.position.y, playerTransform.position.z);
    }

    IEnumerator Spawn()
    {
        if (spawnImmediately)
        {
            TrySpawn();
            RecalculateSpawnRate();
        }

        while (true)
        {
            yield return new WaitForSeconds(spawnRate);
            TrySpawn();
            RecalculateSpawnRate();
        }
    }

    private void RecalculateSpawnRate()
    {
        spawnRate = Random.Range(spawnRateInSecs * (1 - spawnRateDeviation), spawnRateInSecs * (1 + spawnRateDeviation));
    }

    private void TrySpawn()
    {
        for (int i = 0; i < retries; i++)
        {
            Vector2 position = radius * Random.insideUnitCircle;
            Ray spawnRay = new Ray(new Vector3(_transform.position.x + position.x, 50.0f, _transform.position.z + position.y), Vector3.down);

            

            RaycastHit hit;
            if (Physics.Raycast(spawnRay.origin, Vector3.down, out hit, 100.0f, -5, QueryTriggerInteraction.Ignore))
            {

                Minion enemy = MinionsPool.Instance.Get();
                if (enemy == null)
                {
                    return;
                }

                enemy.DisableHarvestingForSeconds(0.5f);

                enemy.transform.localScale = Vector3.zero;
                enemy.transform.position = hit.point + 0.5f * Vector3.up;

                Debug.DrawRay(spawnRay.origin, 100.0f * Vector2.down, Color.green, 1.0f);

                break;
            }
            else
            {
                Debug.DrawRay(spawnRay.origin, 100.0f * Vector2.down, Color.red, 1.0f);
            }
        }
    }
#if UNITY_EDITOR

    private void OnDrawGizmosSelected()
    {
        UnityEditor.Handles.color = new Color(0.0f, 1.0f, 0.0f, 0.2f);
        UnityEditor.Handles.DrawSolidDisc(FindObjectOfType<PlayerControllerRB>().transform.position, Vector3.up, radius);
    }

#endif
}
