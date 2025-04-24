using UnityEngine;
using System.Collections.Generic;

public class TileSystem : MonoBehaviour
{
    [System.Serializable]
    public class PhaseSettings
    {
        public string name;
        public GameObject[] tilePrefabs;
    }

    [Header("Settings")]
    public PhaseSettings[] phases;
    public float tileLength = 10f;
    public Transform player;
    [Range(0.1f, 0.5f)] public float spawnThreshold = 0.3f;

    private Queue<GameObject> activeTiles = new Queue<GameObject>();
    private int currentPhaseIndex = 0;
    private int currentVariantIndex = 0;
    private float nextSpawnZ;
    private bool hasSpawnedNext = false;

    void Awake()
    {
        if (!ValidateConfiguration()) return;
        nextSpawnZ = player.position.z;
        SpawnInitialTiles();
    }


    bool ValidateConfiguration()
    {
        // Vérification du nombre de phases et de prefabs
        if (phases.Length != 3 || phases[0].tilePrefabs.Length != 2)
        {
            Debug.LogError("Configurez 3 phases avec 2 prefabs chacune!");
            return false;
        }

        // Vérification que les prefabs ont bien un Terrain
        foreach (var phase in phases)
        {
            foreach (var prefab in phase.tilePrefabs)
            {
                if (prefab.GetComponent<Terrain>() == null)
                {
                    Debug.LogError($"Le prefab {prefab.name} n'a pas de composant Terrain!");
                    return false;
                }
            }
        }

        // Si tout est valide
        return true;
    }

    void Update()
    {
        if (currentPhaseIndex >= phases.Length || activeTiles.Count == 0) return;

        // Progression du joueur par rapport à la première tuile active
        float playerProgress = (player.position.z - activeTiles.Peek().transform.position.z) / tileLength;

        // Spawn de la prochaine tuile quand le joueur atteint le seuil
        if (!hasSpawnedNext && playerProgress > spawnThreshold)
        {
            SpawnNextTile();
            hasSpawnedNext = true;
        }

        // Désactivation de la tuile la plus ancienne seulement quand le joueur a dépassé 2 tuiles
        if (playerProgress > 2f && activeTiles.Count > 2)
        {
            RemoveOldestTile();
            hasSpawnedNext = false;
        }
    }

    void SpawnInitialTiles()
    {
        // Spawn 2 tuiles au départ
        for (int i = 0; i < 2; i++)
        {
            SpawnNextTile();
        }
    }

    void SpawnNextTile()
    {
        if (currentPhaseIndex >= phases.Length) return;

        GameObject newTile = Instantiate(
            phases[currentPhaseIndex].tilePrefabs[currentVariantIndex],
            new Vector3(0, 0, nextSpawnZ),
            Quaternion.identity
        );

        activeTiles.Enqueue(newTile);
        nextSpawnZ += tileLength;

        // Alternance entre les variantes et passage à la phase suivante si nécessaire
        currentVariantIndex = (currentVariantIndex + 1) % 2;
        if (currentVariantIndex == 0) currentPhaseIndex++;
    }

    void RemoveOldestTile()
    {
        if (activeTiles.Count == 0) return;

        GameObject oldest = activeTiles.Dequeue();
        oldest.SetActive(false);
    }
}

