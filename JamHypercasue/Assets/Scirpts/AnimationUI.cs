using System;
using UnityEngine;
using DG.Tweening;

public class AnimationUI : MonoBehaviour
{
    [SerializeField] private GameObject tapeHere;
    [SerializeField] private GameObject Congratulation;
    private float yposBaseCongradulation;
    private RectTransform rectTransform;
    public float moveAmount = 50;      // Distance du mouvement haut/bas
    public float duration = 1f; 

    private void Start()
    {
        yposBaseCongradulation = Congratulation.transform.localPosition.y;
        Mouvement();
    }

    private void Mouvement()
    {
        rectTransform = Congratulation.GetComponent<RectTransform>();

        // Tween en Y (local)
        rectTransform
            .DOAnchorPosY(rectTransform.anchoredPosition.y + moveAmount, duration)
            .SetLoops(-1, LoopType.Yoyo)  // Boucle infinie, aller-retour
            .SetEase(Ease.InOutSine);
        Congratulation.transform.DOLocalMoveY(yposBaseCongradulation + 3, 1f).SetEase(Ease.OutBack);
    }
    
}
