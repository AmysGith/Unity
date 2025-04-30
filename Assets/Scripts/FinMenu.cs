using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FinMenu: MonoBehaviour
{
    public Button replayButton; 
    public Button quitButton;   

    void Start()
    {
        replayButton.onClick.AddListener(ReplayScene); 
        quitButton.onClick.AddListener(QuitGame);       
    }

    void ReplayScene()
    {
        SceneManager.LoadScene("Swimming");
    }

    void QuitGame()
    {
        Application.Quit();
    }
}
