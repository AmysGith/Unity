using UnityEngine;

public class Flock : MonoBehaviour
{
    public float speed;
    float nextRandomRotationTime;
    Vector3 randomDirection;

    [Header("Height Settings")]
    public float minHeight = 1f;
    public float maxHeight = 10f;

    [Header("Disappearance Settings")]
    public float disappearSpeed = 5f;
    public float disappearOffset = 5f; // Distance après sortie de la caméra avant disparition
    public bool isDisappearing = false;

    void OnEnable()
    {
        InitializeFish();
    }

    void InitializeFish()
    {
        if (FlockManager.FM == null)
        {
            Debug.LogError("FlockManager.FM n'est pas initialisé !");
            return;
        }

        speed = Random.Range(FlockManager.FM.minSpeed, FlockManager.FM.maxSpeed);
        nextRandomRotationTime = Time.time + Random.Range(1f, 10f);
        randomDirection = GenerateRandomDirection();
        isDisappearing = false;

        // Position initiale avec une hauteur aléatoire entre minHeight et maxHeight
        float initialHeight = Random.Range(minHeight, maxHeight);
        transform.position = new Vector3(transform.position.x, initialHeight, transform.position.z);
    }

    void Update()
    {
        if (isDisappearing)
        {
            // Déplacement vers la direction définie vers la caméra
            transform.Translate(Vector3.forward * disappearSpeed * Time.deltaTime);

            // Vérifier si le poisson est hors de la caméra **ET** suffisamment loin derrière elle
            if (!IsVisibleFromCamera(Camera.main) && transform.position.z < Camera.main.transform.position.z - disappearOffset)
            {
                gameObject.SetActive(false);
            }
            return;
        }

        // Comportement normal
        if (IsOutOfBounds())
        {
            Vector3 directionToCenter = (FlockManager.FM.transform.position - transform.position).normalized;
            directionToCenter.y = 0;
            RotateTowards(directionToCenter);
        }
        else
        {
            if (Time.time >= nextRandomRotationTime)
            {
                randomDirection = GenerateRandomDirection();
                nextRandomRotationTime = Time.time + Random.Range(1f, 10f);
            }
            RotateTowards(randomDirection);
        }

        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        // Contrôle de la hauteur
        if (transform.position.y < minHeight || transform.position.y > maxHeight)
        {
            float correctedHeight = Mathf.Clamp(transform.position.y, minHeight, maxHeight);
            transform.position = new Vector3(transform.position.x, correctedHeight, transform.position.z);
        }
    }

    public void StartDisappearing()
    {
        isDisappearing = true;

        // Définir une direction vers la position actuelle de la caméra avec une légère variation
        Vector3 cameraPosition = Camera.main.transform.position;
        Vector3 targetPosition = cameraPosition + new Vector3(
            Random.Range(-3f, 3f),
            0,
            Random.Range(-3f, 3f)
        );

        Vector3 directionToTarget = (targetPosition - transform.position).normalized;
        RotateTowards(directionToTarget);
    }

    bool IsOutOfBounds()
    {
        Vector3 relativePos = transform.position - FlockManager.FM.transform.position;
        return Mathf.Abs(relativePos.x) > FlockManager.FM.swimLimits.x ||
               Mathf.Abs(relativePos.z) > FlockManager.FM.swimLimits.z;
    }

    Vector3 GenerateRandomDirection()
    {
        return new Vector3(
            Random.Range(-1f, 1f),
            0,
            Random.Range(0.5f, 1f)
        ).normalized;
    }

    void RotateTowards(Vector3 direction)
    {
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                FlockManager.FM.rotationSpeed * Time.deltaTime);
        }
    }

    bool IsVisibleFromCamera(Camera cam)
    {
        Vector3 screenPoint = cam.WorldToViewportPoint(transform.position);
        return screenPoint.x > 0 && screenPoint.x < 1 &&
               screenPoint.y > 0 && screenPoint.y < 1 &&
               screenPoint.z > 0;
    }
}
