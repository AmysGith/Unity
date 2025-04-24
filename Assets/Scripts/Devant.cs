using UnityEngine;

public class FlockManagerStayAhead : MonoBehaviour
{
    public Transform player; // Référence au joueur
    public float distanceAhead = 5.0f; // Distance à garder devant le joueur
    public float verticalOffset = 0.0f; // Ajustement en hauteur (si nécessaire)

    void Update()
    {
        if (player == null)
        {
            Debug.LogError("Aucun joueur assigné au FlockManagerStayAhead !");
            return;
        }

        // Calcul de la position devant le joueur
        Vector3 positionAhead = player.position + player.forward * distanceAhead;
        positionAhead.y += verticalOffset; // Ajuste la hauteur si nécessaire

        // Déplace le FlockManager directement à cette position
        transform.position = positionAhead;

        // Oriente le FlockManager dans la même direction que le joueur
        transform.rotation = player.rotation;
    }
}
