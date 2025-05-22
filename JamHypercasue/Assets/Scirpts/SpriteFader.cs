using UnityEngine;
using DG.Tweening;

public class SpriteFader : MonoBehaviour
{
    public float targetAlpha = 0f;
    public float duration = 1f;

    private Material material;

    private void Start()
    {
        // Important : on instancie le matériau pour ne pas affecter tous les objets qui l’utilisent
        material = GetComponent<MeshRenderer>().material;

        // On récupère la couleur de base
        Color startColor = material.color;

        // On lance le tween de l’alpha
        material
            .DOColor(new Color(startColor.r, startColor.g, startColor.b, targetAlpha), duration)
            .SetEase(Ease.InOutSine);
    }
}
