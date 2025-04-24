using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public Button startButton; // R�f�rence au bouton "Commencer"
    public Button quitButton;  // R�f�rence au bouton "Quitter"
    public Button tutorialButton;
    public Button closeTab;
    public GameObject Ecran;

    void Start()
    {
        // Assurez-vous d'avoir assign� les boutons dans l'Inspector Unity
        startButton.onClick.AddListener(StartGame);  // Action au clic sur "Commencer"
        quitButton.onClick.AddListener(QuitGame);    // Action au clic sur "Quitter"
        tutorialButton.onClick.AddListener(Text);
        closeTab.onClick.AddListener(Close);
    }

    void StartGame()
    {
        
        SceneManager.LoadScene("Swimming");  
    }

    void QuitGame()
    {
        
        Application.Quit();  
       
    }

    void Text()
    {
        Ecran.SetActive(true);
    }

    void Close()
    {
        Ecran.SetActive(false);
    }
}
