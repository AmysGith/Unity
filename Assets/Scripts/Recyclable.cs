using UnityEngine;

public class Recyclable : MonoBehaviour
{
    public float distanceAvantRecycle = 25.0f;
    private Transform player;
    public float delayBeforeDestruction = 2.0f;
    private bool hasHitPlayer = false; // Nouvelle variable pour suivre si l'objet a déjà touché le joueur

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        float distance = Vector3.Distance(player.position, transform.position);

        if (distance > distanceAvantRecycle + 2)
        {
            Debug.Log(gameObject.name + Vector3.Distance(transform.position, player.position));
            gameObject.SetActive(false);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && !hasHitPlayer)
        {
            hasHitPlayer = true; // Marquer que l'objet a touché le joueur
            GameManager.Instance.IncreasePollution(5f);
            Invoke("DeactivateObject", delayBeforeDestruction);
        }
    }

    void DeactivateObject()
    {
        gameObject.SetActive(false);
    }

    // Réinitialiser le statut lorsque l'objet est réactivé
    void OnEnable()
    {
        hasHitPlayer = false;
    }
}