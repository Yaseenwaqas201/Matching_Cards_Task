using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class CardHolder : MonoBehaviour
{
    public int cardValue;
    [FormerlySerializedAs("assignCard")] public CardData assignCardData;
    public Sprite cardBack;
    public Image cardImg;

    private bool _isOpened;
    public void ShowCard()
    {
        _isOpened = true;
        transform.DOLocalRotate(new Vector3(0, 90, 0), 0.2f).OnComplete(() =>
        {
            // Here we change Sprite 
            cardImg.sprite = assignCardData.cardSprite;
            transform.DOLocalRotate(Vector3.zero, 0.2f);
        });
        CardsManager.instanCM.SelectTheCard(this);
    }
    
    public void DisplayCardAtStart()
    {
        _isOpened = true;
        transform.DOLocalRotate(new Vector3(0, 90, 0), 0.2f).OnComplete(() =>
        {
            // Here we change Sprite 
            cardImg.sprite = assignCardData.cardSprite;
            transform.DOLocalRotate(Vector3.zero, 0.2f);
        });
    }
    public void HideCard()
    {
        _isOpened = false;
        transform.DOLocalRotate(new Vector3(0, 90, 0), 0.2f).OnComplete(() =>
        {
            // Here we change Sprite 
            cardImg.sprite = cardBack;
            transform.DOLocalRotate(Vector3.zero, 0.2f);
        });
    }

    public void ResetTheCard()
    {
        cardImg.sprite = cardBack;
        transform.DOLocalRotate(Vector3.zero, 0f);
        cardImg.enabled = true;
        gameObject.SetActive(true);
    }
}
