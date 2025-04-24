using System.Collections.Generic;
using UnityEngine;

public class FlockPooler : MonoBehaviour
{
    public GameObject[] fishPrefabs;
    public int numFishPerPrefab = 5;
    public List<GameObject> pooledFish = new List<GameObject>();
    public static FlockPooler Instance;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if (fishPrefabs == null || fishPrefabs.Length == 0)
        {
            Debug.LogError("FlockPooler: Aucun prefab de poisson assigné !");
            return;
        }

        foreach (GameObject prefab in fishPrefabs)
        {
            for (int i = 0; i < numFishPerPrefab; i++)
            {
                GameObject fish = Instantiate(prefab, Vector3.zero, Quaternion.identity);
                fish.SetActive(false);
                pooledFish.Add(fish);
            }
        }
    }

    public GameObject GetInactiveFish()
    {
        foreach (GameObject fish in pooledFish)
        {
            if (!fish.activeInHierarchy)
            {
                return fish;
            }
        }

        // Si aucun poisson n'est disponible, en créer un nouveau
        if (fishPrefabs.Length > 0)
        {
            GameObject newFish = Instantiate(fishPrefabs[Random.Range(0, fishPrefabs.Length)], Vector3.zero, Quaternion.identity);
            newFish.SetActive(false);
            pooledFish.Add(newFish);
            return newFish;
        }

        return null;
    }

    public List<GameObject> GetAllFish()
    {
        return pooledFish; // Retourne la liste des poissons dans le pool
    }
}
