using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public Transform player;
    public ObjectPooler pooler;
    public float height = 15f;
    public float baseSpawnInterval = 2.0f;
    public float maxSpawnVariation = 1.0f;
    public float spawnSpeed = 7.0f;
    public bool extremePhaseTriggered = false;

    void Start()
    {
        ScheduleNextSpawn();
    }

    void ScheduleNextSpawn()
    {
        float nextInterval = baseSpawnInterval + Random.Range(-maxSpawnVariation, maxSpawnVariation);
        nextInterval = Mathf.Clamp(nextInterval, 0.5f, baseSpawnInterval + maxSpawnVariation);
        Invoke(nameof(SpawnObject), nextInterval);
    }

    void SpawnObject()
    {
        if (player == null || pooler == null) return;

        int numberOfObjects = extremePhaseTriggered ? 3 : 1;
        bool shouldTargetPlayer = extremePhaseTriggered && Random.value < 0.5f;
        int targetedIndex = Random.Range(0, numberOfObjects); // Index de l’objet qui vise le joueur

        for (int i = 0; i < numberOfObjects; i++)
        {
            Vector3 spawnPos;

            if (extremePhaseTriggered && shouldTargetPlayer && i == targetedIndex)
            {
                // Vise le joueur avec prédiction
                Rigidbody playerRb = player.GetComponent<Rigidbody>();
                Vector3 futurePosition = player.position;

                if (playerRb != null)
                {
                    futurePosition += playerRb.velocity * 0.3f;
                }

                spawnPos = new Vector3(
                    futurePosition.x,
                    futurePosition.y + height,
                    futurePosition.z
                );
            }
            else
            {
                // Tombe aléatoirement autour
                Vector3 predictedPosition = player.position + player.forward * 5.0f;
                spawnPos = new Vector3(
                    predictedPosition.x + Random.Range(-10f, 10f),
                    predictedPosition.y + height,
                    predictedPosition.z + Random.Range(-10f, 10f)
                );
            }

            GameObject spawnedObject = pooler.GetPooledObject();
            if (spawnedObject != null)
            {
                spawnedObject.transform.position = spawnPos;
                spawnedObject.transform.rotation = Quaternion.identity;
                spawnedObject.SetActive(true);

                Rigidbody rb = spawnedObject.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.velocity = new Vector3(0, -spawnSpeed * (extremePhaseTriggered ? 1.0f : 1.0f), 0);
                }
            }
        }

        ScheduleNextSpawn();
    }
}
