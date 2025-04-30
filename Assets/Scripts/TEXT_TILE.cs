using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TileMessageUI : MonoBehaviour
{
    public Image notificationImage; // Image de la notification
    public Sprite[] notificationSprites; // Tableau des sprites à afficher
    public float slideDuration = 1f; // Durée du glissement de la notification

    private int currentSpriteIndex = 0;
    private bool isShowingMessage = false;

    public void ShowNextMessage()
    {
        if (notificationSprites.Length == 0 || isShowingMessage) return;

        StartCoroutine(ShowMessageRoutine());
    }

    private IEnumerator ShowMessageRoutine()
    {
        isShowingMessage = true;

        // Charger et afficher le sprite
        notificationImage.sprite = notificationSprites[currentSpriteIndex];
        notificationImage.gameObject.SetActive(true);

        // Position initiale de la notification (à gauche hors de l'écran)
        notificationImage.rectTransform.anchoredPosition = new Vector2(-Screen.width, 0);

        // Glisser l'image de la notification vers la droite
        float elapsedTime = 0f;
        Vector2 startPosition = notificationImage.rectTransform.anchoredPosition;
        Vector2 targetPosition = new Vector2(0, 0); // Centré à l'écran

        while (elapsedTime < slideDuration)
        {
            notificationImage.rectTransform.anchoredPosition = Vector2.Lerp(startPosition, targetPosition, elapsedTime / slideDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        notificationImage.rectTransform.anchoredPosition = targetPosition;

        // Attendre quelques secondes avant de cacher l'image
        yield return new WaitForSeconds(5f);

        // Glisser l'image hors de l'écran à gauche
        elapsedTime = 0f;
        startPosition = notificationImage.rectTransform.anchoredPosition;
        targetPosition = new Vector2(-Screen.width, 0); // Hors de l'écran à gauche

        while (elapsedTime < slideDuration)
        {
            notificationImage.rectTransform.anchoredPosition = Vector2.Lerp(startPosition, targetPosition, elapsedTime / slideDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        notificationImage.rectTransform.anchoredPosition = targetPosition;

        notificationImage.gameObject.SetActive(false);

        yield return new WaitForSeconds(2f); 

        // Passer au prochain sprite
        currentSpriteIndex = (currentSpriteIndex + 1) % notificationSprites.Length;
        isShowingMessage = false;
    }
}
