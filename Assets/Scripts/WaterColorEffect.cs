using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;

public class WaterColorEffect : MonoBehaviour
{
    public PostProcessVolume postProcessVolume; // Référence au PostProcessVolume
    private ColorGrading colorGrading; // Référence à l'effet Color Grading

    public Color cleanWaterColor = Color.white; // Couleur de l'eau propre (bleu clair)
    public Color pollutedWaterColor = new Color(0.8f, 1f, 0.8f); // Couleur de l'eau polluée (vert)

    public float changeInterval = 5f; // Intervalle de temps en secondes entre chaque changement de couleur

    private float timer = 0f; // Timer pour suivre le temps
    private float pollutionRatio = 0f; // Ratio de pollution, de 0 à 1

    void Start()
    {
        // Récupère l'effet Color Grading
        postProcessVolume.profile.TryGetSettings(out colorGrading);
    }

    void Update()
    {
        if (colorGrading == null) return;

        // Incrémenter le timer
        timer += Time.deltaTime;

        // Toutes les 5 secondes, augmenter le ratio de pollution
        if (timer >= changeInterval)
        {
            timer = 0f; // Réinitialiser le timer
            pollutionRatio += 0.1f; // Augmenter progressivement le ratio de pollution (ajustable)
            pollutionRatio = Mathf.Clamp(pollutionRatio, 0f, 1f); // Limiter le ratio entre 0 et 1
        }

        // Changer la couleur de l'eau de manière progressive
        colorGrading.colorFilter.value = Color.Lerp(cleanWaterColor, pollutedWaterColor, pollutionRatio);
    }
}
