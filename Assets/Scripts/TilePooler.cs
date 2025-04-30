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

    private TileMessageUI messageUI;

    void Start()
    {
        messageUI = FindObjectOfType<TileMessageUI>();
    }

    void Awake()
    {
        if (!ValidateConfiguration()) return;
        nextSpawnZ = player.position.z - 10;
        SpawnInitialTiles();
    }

    bool ValidateConfiguration()
    {
        // Vérification du nombre de phases (minimum 3)
        if (phases.Length < 3)
        {
            Debug.LogError("Configurez au moins 3 phases!");
            return false;
        }

        // La première phase doit avoir exactement 2 prefabs
        if (phases[0].tilePrefabs.Length != 2)
        {
            Debug.LogError("La première phase doit avoir exactement 2 prefabs!");
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

        return true;
    }

    void Update()
    {
        if (currentPhaseIndex >= phases.Length || activeTiles.Count == 0) return;

        float playerProgress = (player.position.z - activeTiles.Peek().transform.position.z) / tileLength;

        if (!hasSpawnedNext && playerProgress > spawnThreshold)
        {
            SpawnNextTile();
            hasSpawnedNext = true;
        }

        if (playerProgress > 2f && activeTiles.Count > 2)
        {
            RemoveOldestTile();
            hasSpawnedNext = false;
        }
    }

    void SpawnInitialTiles()
    {
        for (int i = 0; i < 2; i++)
        {
            SpawnNextTile();
        }
    }

    void SpawnNextTile()
    {
        if (currentPhaseIndex >= phases.Length) return;

        // Logique différente pour la première phase
        if (currentPhaseIndex == 0)
        {
            // Phase 0: alternance stricte entre 2 tuiles
            currentVariantIndex = (currentVariantIndex + 1) % 2;

            // Si on revient à l'index 0, on passe à la phase suivante
            if (currentVariantIndex == 0) currentPhaseIndex++;
        }
        else
        {
            // Phases 1 et 2: on utilise tous les prefabs disponibles
            currentVariantIndex = (currentVariantIndex + 1) % phases[currentPhaseIndex].tilePrefabs.Length;

            // On ne passe à la phase suivante que si ce n'est pas la dernière phase
            if (currentVariantIndex == 0 && currentPhaseIndex < phases.Length - 1)
            {
                currentPhaseIndex++;
            }
        }

        GameObject newTile = Instantiate(
            phases[currentPhaseIndex].tilePrefabs[currentVariantIndex],
            new Vector3(0, 0, nextSpawnZ),
            Quaternion.identity
        );

        activeTiles.Enqueue(newTile);
        nextSpawnZ += tileLength;

        if (messageUI != null)
        {
            messageUI.ShowNextMessage();
        }
    }

    void RemoveOldestTile()
    {
        if (activeTiles.Count == 0) return;

        GameObject oldest = activeTiles.Dequeue();
        oldest.SetActive(false);
    }
}