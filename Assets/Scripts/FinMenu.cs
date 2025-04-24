using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FinMenu: MonoBehaviour
{
    public Button replayButton; // Bouton pour rejouer une scène
    public Button quitButton;   // Bouton pour quitter le jeu

    void Start()
    {
        // Assurez-vous d'assigner les boutons dans l'Inspector Unity
        replayButton.onClick.AddListener(ReplayScene);  // Action au clic sur "Rejouer"
        quitButton.onClick.AddListener(QuitGame);       // Action au clic sur "Quitter"
    }

    void ReplayScene()
    {
        // Rejoue une scène spécifique (par exemple "Swimming")
        SceneManager.LoadScene("Swimming");
    }

    void QuitGame()
    {
        // Quitte l'application
        Application.Quit();
    }
}
