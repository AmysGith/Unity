using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    // Pollution System
    public float pollution = 0f;
    public float maxPollution = 100f;
    public Slider pollutionSlider;
    private List<int> pollutionMilestonesReached = new List<int>();

    // Scene Transition
    public string apocalypseSceneName = "ApocalypseScene";
    public Image fadeOverlay;
    public float fadeDuration = 2f;
    private bool isTransitioning = false;
    private float fadeTimer = 0f;

    // Extreme Phase
    public float distanceThreshold = 10f;
    public bool extremePhaseTriggered = false;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        if (fadeOverlay != null)
        {
            fadeOverlay.color = new Color(0, 0, 0, 0);
            fadeOverlay.raycastTarget = false;
        }

        if (pollutionSlider != null)
        {
            pollutionSlider.minValue = 0;
            pollutionSlider.maxValue = maxPollution;
            pollutionSlider.value = pollution;
        }
    }

    void Update()
    {
        if (isTransitioning)
        {
            fadeTimer += Time.unscaledDeltaTime;
            float fadeProgress = Mathf.Clamp01(fadeTimer / fadeDuration);
            fadeOverlay.color = new Color(0, 0, 0, fadeProgress);

            if (fadeTimer >= fadeDuration)
            {
                SceneManager.LoadScene(apocalypseSceneName);
                Time.timeScale = 1f;
            }
            return;
        }

        if (!extremePhaseTriggered)
        {
            MoveForward playerMovement = GameObject.FindGameObjectWithTag("Player")?.GetComponent<MoveForward>();
            if (playerMovement != null && playerMovement.totalDistance >= distanceThreshold)
            {
                ActivateExtremePhase();
            }
        }

        CheckPollutionMilestone();

        if (pollution >= maxPollution && !isTransitioning)
        {
            StartApocalypseTransition();
        }
    }

    public void IncreasePollution(float amount)
    {
        pollution = Mathf.Clamp(pollution + amount, 0, maxPollution);
        UpdateUI();

        if (pollution >= maxPollution)
        {
            StartApocalypseTransition();
        }
    }

    void CheckPollutionMilestone()
    {
        int percentage = Mathf.FloorToInt((pollution / maxPollution) * 100);
        if (percentage > 0 && percentage % 20 == 0 && !pollutionMilestonesReached.Contains(percentage))
        {
            pollutionMilestonesReached.Add(percentage);
            FlockManager.FM.DisableActiveFish(3);
        }
    }

    void UpdateUI()
    {
        if (pollutionSlider != null)
        {
            pollutionSlider.value = pollution;
        }
    }

    void StartApocalypseTransition()
    {
        if (isTransitioning) return;

        isTransitioning = true;
        fadeOverlay.raycastTarget = true;

        var player = GameObject.FindGameObjectWithTag("Player");
        if (player != null) player.GetComponent<MoveForward>().enabled = false;
    }

    void ActivateExtremePhase()
    {
        extremePhaseTriggered = true;
        MoveForward playerMovement = GameObject.FindGameObjectWithTag("Player")?.GetComponent<MoveForward>();
        if (playerMovement != null)
        {
            playerMovement.isSlowedDown = true;
        }

        SpawnManager spawnManager = FindObjectOfType<SpawnManager>();
        if (spawnManager != null)
        {
            spawnManager.extremePhaseTriggered = true;
        }
    }
}
