using UnityEngine;

public class FlockManagerStayAhead : MonoBehaviour
{
    public Transform player; // R�f�rence au joueur
    public float distanceAhead = 5.0f; // Distance � garder devant le joueur
    public float verticalOffset = 0.0f; // Ajustement en hauteur (si n�cessaire)

    void Update()
    {
        if (player == null)
        {
            Debug.LogError("Aucun joueur assign� au FlockManagerStayAhead !");
            return;
        }

        // Calcul de la position devant le joueur
        Vector3 positionAhead = player.position + player.forward * distanceAhead;
        positionAhead.y += verticalOffset; // Ajuste la hauteur si n�cessaire

        // D�place le FlockManager directement � cette position
        transform.position = positionAhead;

        // Oriente le FlockManager dans la m�me direction que le joueur
        transform.rotation = player.rotation;
    }
}
