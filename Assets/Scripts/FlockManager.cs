using UnityEngine;
using System.Collections.Generic;

public class FlockManager : MonoBehaviour
{
    // Singleton pour permettre un accès global
    public static FlockManager FM;

    public Transform playerTransform; // Référence vers le joueur


    // Références aux poissons et aux limites de nage
    public GameObject[] allFish;
    public Vector3 swimLimits = new Vector3(5.0f, 5.0f, 5.0f);
    public Vector3 goalPos = Vector3.zero;

    [Header("Fish Settings")]
    [Range(0.1f, 8.0f)] public float minSpeed;
    [Range(0.0f, 8.0f)] public float maxSpeed;
    [Range(1.0f, 10.0f)] public float neighbourDistance;
    [Range(1.0f, 5.0f)] public float rotationSpeed;


    // Initialisation
    void Awake()
    {
        FM = this;
    }

    void Start()
    {
        // Initialisation des poissons depuis le pool
        allFish = new GameObject[FlockPooler.Instance.pooledFish.Count];

        for (int i = 0; i < allFish.Length; i++)
        {
            GameObject fish = FlockPooler.Instance.GetInactiveFish();

            if (fish != null)
            {
                // Position aléatoire à l'intérieur des limites de nage
                Vector3 pos = this.transform.position + new Vector3(
                    Random.Range(-swimLimits.x, swimLimits.x),
                    Random.Range(-swimLimits.y, swimLimits.y),
                    Random.Range(-swimLimits.z, swimLimits.z));

                fish.transform.position = pos;
                fish.transform.rotation = Quaternion.identity;
                fish.SetActive(true);
                allFish[i] = fish;
            }
        }

        // Position initiale de l'objectif
        goalPos = this.transform.position;
    }

    void Update()
    {
        // Modifier aléatoirement la position de l'objectif
        if (Random.Range(0, 100) < 10)
        {
            goalPos = this.transform.position + new Vector3(
                Random.Range(-swimLimits.x, swimLimits.x),
                Random.Range(-swimLimits.y, swimLimits.y),
                Random.Range(-swimLimits.z, swimLimits.z));
        }
    }

    // Méthode pour désactiver des poissons actifs
    public void DisableActiveFish(int count)
    {
        // Liste des poissons actifs non en train de disparaître
        List<GameObject> activeFish = new List<GameObject>();

        foreach (var fish in allFish)
        {
            if (fish != null && fish.activeInHierarchy)
            {
                Flock flock = fish.GetComponent<Flock>();
                if (flock != null && !flock.isDisappearing)
                {
                    activeFish.Add(fish);
                }
                
            }
        }

        // Mélange aléatoire des poissons actifs (algorithme Fisher-Yates)
        for (int i = 0; i < activeFish.Count; i++)
        {
            int randomIndex = Random.Range(i, activeFish.Count);
            GameObject temp = activeFish[i];
            activeFish[i] = activeFish[randomIndex];
            activeFish[randomIndex] = temp;
        }

        // Désactiver les premiers 'count' poissons
        int disabledCount = 0;

        for (int i = 0; i < Mathf.Min(count, activeFish.Count); i++)
        {
            activeFish[i].GetComponent<Flock>().StartDisappearing();
            disabledCount++;
        }

        // Avertissement si le nombre de poissons désactivés est inférieur au nombre demandé
        if (disabledCount < count)
        {
            Debug.LogWarning($"Only {disabledCount} fish disabled (requested: {count})");
        }
    }
}
