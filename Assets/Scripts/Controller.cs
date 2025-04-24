using UnityEngine;

public class MoveForward : MonoBehaviour
{
    public float baseSpeed = 5.0f;
    private float speed;
    public bool isSlowedDown = false;
    public float extremeSlowSpeed = 2.0f;
    public float totalDistance = 0f;

    private float verticalInput = 0f;
    private float horizontalInput = 0f;
    private float minX;
    private float maxX;

    // Optimisation : cache le terrain courant
    private Terrain _currentTerrain;
    private Vector3 _terrainSize;

    void Start()
    {
        // Ne fait rien pour UpdateTerrainBounds ici, on gère ça dans Update proprement
    }

    void Update()
    {
        // Si pas encore de terrain trouvé, essaye d'en trouver un
        if (_currentTerrain == null)
        {
            UpdateTerrainBounds();
            if (_currentTerrain == null)
                return; // Tant qu'il n'y a pas de terrain actif, on ne fait rien
        }

        UpdateSpeed();
        ProcessInputs();
        MovePlayer();
        UpdateTerrainBounds(); // Vérifie dynamiquement si on change de terrain
    }

    void UpdateSpeed()
    {
        if (isSlowedDown)
        {
            speed = extremeSlowSpeed;
            return;
        }

        float pollutionPercentage = (GameManager.Instance.pollution / GameManager.Instance.maxPollution) * 100;
        int pollutionStage = Mathf.FloorToInt(pollutionPercentage / 30);
        speed = baseSpeed * (1f - pollutionStage * 0.2f);
    }

    void ProcessInputs()
    {
        // Gestion clavier
        float keyboardVertical = Input.GetAxis("Vertical");
        float keyboardHorizontal = Input.GetAxis("Horizontal");

        if (Mathf.Abs(keyboardVertical) > 0.1f)
            verticalInput = keyboardVertical;

        if (Mathf.Abs(keyboardHorizontal) > 0.1f)
            horizontalInput = keyboardHorizontal;
    }

    void MovePlayer()
    {
        Vector3 movement = new Vector3(horizontalInput, 0, verticalInput) * speed * Time.deltaTime;
        Vector3 newPosition = transform.position + movement;

        newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX); // Clamp entre min et max du terrain
        transform.position = newPosition;

        totalDistance += movement.magnitude;
    }

    void UpdateTerrainBounds()
    {
        Terrain newTerrain = null;

        foreach (Terrain t in Terrain.activeTerrains)
        {
            Vector3 terrainPos = t.transform.position;
            Vector3 size = t.terrainData.size;

            if (transform.position.z >= terrainPos.z && transform.position.z <= terrainPos.z + size.z)
            {
                newTerrain = t;
                break;
            }
        }

        // Mise à jour si on trouve un nouveau terrain
        if (newTerrain != null)
        {
            _currentTerrain = newTerrain;
            Vector3 terrainPos = _currentTerrain.transform.position;
            _terrainSize = _currentTerrain.terrainData.size;
            minX = terrainPos.x;
            maxX = terrainPos.x + _terrainSize.x;
        }
        // Sinon, garde les anciennes limites (pas de reset)
    }

    public void SetVerticalInput(int direction) => verticalInput = direction;
    public void SetHorizontalInput(int direction) => horizontalInput = direction;
}
