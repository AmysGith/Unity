using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public Transform player; // Le joueur
    public ObjectPooler pooler; // Pooler pour récupérer les objets
    public float height = 15f; // Hauteur de spawn
    public float baseSpawnInterval = 2.0f; // Intervalle de spawn de base
    public float maxSpawnVariation = 1.0f; // Variabilité du temps de spawn
    public float spawnSpeed = 7.0f; // Vitesse initiale de chute des objets
    public bool extremePhaseTriggered = false; // Indicateur pour la phase extrême

    void Start()
    {
        ScheduleNextSpawn();
    }

    void ScheduleNextSpawn()
    {
        float nextInterval = baseSpawnInterval + Random.Range(-maxSpawnVariation, maxSpawnVariation);
        nextInterval = Mathf.Clamp(nextInterval, 0.5f, baseSpawnInterval + maxSpawnVariation); // Évite un intervalle trop court
        Invoke("SpawnObject", nextInterval);
    }

    void SpawnObject()
    {
        if (player == null || pooler == null) return;

        Vector3 spawnPos;

        if (extremePhaseTriggered)
        {
            // Prédire une position future du joueur en fonction de sa vitesse actuelle
            Rigidbody playerRb = player.GetComponent<Rigidbody>(); // Vérifie si le joueur a un Rigidbody
            Vector3 futurePosition = player.position;

            if (playerRb != null)
            {
                futurePosition += playerRb.velocity * 0.3f; // Ajuste le "0.3f" pour le délai entre spawn et chute
            }

            spawnPos = new Vector3(
                futurePosition.x,
                futurePosition.y + height,
                futurePosition.z
            );
        }
        else
        {
            // Position normale avec une légère prédiction
            Vector3 predictedPosition = player.position + player.forward * 5.0f;
            spawnPos = new Vector3(
                predictedPosition.x + Random.Range(-1.5f, 1.5f),
                predictedPosition.y + height,
                predictedPosition.z + Random.Range(-1.5f, 1.5f)
            );
        }

        // Activer et positionner l'objet
        GameObject spawnedObject = pooler.GetPooledObject();
        if (spawnedObject != null)
        {
            spawnedObject.transform.position = spawnPos;
            spawnedObject.transform.rotation = Quaternion.identity;
            spawnedObject.SetActive(true);

            // Configurer la vitesse de chute
            Rigidbody rb = spawnedObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = new Vector3(0, -spawnSpeed * (extremePhaseTriggered ? 5.0f : 1.0f), 0); // Augmente la vitesse en phase extrême
            }
        }

        // Planifier le prochain spawn
        ScheduleNextSpawn();
    }
}
