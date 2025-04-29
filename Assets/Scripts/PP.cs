using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PauseToggle : MonoBehaviour
{
    [Header("Références UI")]
    public GameObject pauseButton;
    public GameObject playButton;
    public GameObject replayButton;
    public GameObject quitButton;

    private bool isPaused = false;

    void Start()
    {
        pauseButton.SetActive(true);
        playButton.SetActive(false);
        Time.timeScale = 1f;
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0f : 1f;
        pauseButton.SetActive(!isPaused);
        playButton.SetActive(isPaused);
        EventSystem.current.SetSelectedGameObject(null);
    }
    public void ReplayScene()
    {
        Debug.Log("REPLAY SCENE CALLED");
        Time.timeScale = 1f; 
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void quit()
    {
        SceneManager.LoadScene("Menu");
    }

}