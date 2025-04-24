using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public float smoothSpeed = 2f;
    public float height = 1f; // Hauteur fixe
    public float distance = 1f; // Distance derrière le poisson

    void LateUpdate()
    {
        // Position toujours au-dessus du poisson
        Vector3 desiredPosition = player.position + new Vector3(0, height, -distance);
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

        // Toujours regarder le poisson
        transform.LookAt(player);
    }
}