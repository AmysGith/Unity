using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    [System.Serializable]
    public class PooledObject
    {
        public GameObject prefab;
        public int poolSize = 5;
    }

    public PooledObject[] objectsToPool;

    private Dictionary<GameObject, List<GameObject>> poolDictionary;

    void Start()
    {
        poolDictionary = new Dictionary<GameObject, List<GameObject>>();

        foreach (PooledObject item in objectsToPool)
        {
            List<GameObject> objectPool = new List<GameObject>();

            for (int i = 0; i < item.poolSize; i++)
            {
                GameObject obj = Instantiate(item.prefab);
                obj.SetActive(false);
                objectPool.Add(obj);
            }

            poolDictionary[item.prefab] = objectPool;
        }
    }

    public GameObject GetPooledObject()
    {
        if (objectsToPool.Length == 0) return null;

        // Choisir un type de déchet au hasard
        int randomIndex = Random.Range(0, objectsToPool.Length);
        GameObject selectedPrefab = objectsToPool[randomIndex].prefab;

        // Chercher un objet inactif dans ce pool
        List<GameObject> selectedPool = poolDictionary[selectedPrefab];

        foreach (GameObject obj in selectedPool)
        {
            if (!obj.activeInHierarchy)
                return obj;
        }

        // Aucun objet disponible dans ce pool
        return null;
    }
}
