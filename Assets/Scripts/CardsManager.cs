using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class CardsManager : MonoBehaviour
{
    [Header(" UI Text Of Counting Card Match And Turn ")]
    public Text matchCardsAre;
    public int matchCardsCount=0;
    public Text totalNumberOfTurns;
    public Text totalNoOfTurnsInCongratPanel;
    public int turnsCounter=0;
    [Header(" Maximum Number of Cards Available Data")]
    public List<CardData> listOfCardsData=new List<CardData>();
    public RectTransform cardsContainer;
    public GridLayoutGroup cardsContainerGrid;
    public CardHolder cardTemplate;
    List<CardHolder> cardsInt=new List<CardHolder>();

    private CardHolder firstSelectedCard;
    private CardHolder secondSelectedCard;

    
    public EventSystem eventSys;
    public static CardsManager instanCM;

    private void Awake()
    {
        if (instanCM == null)
        {
            instanCM = this;
        }
    }


    private int matchesForLevelComplete;
    public void AssignCards(int gameMode)
    {
        switch (gameMode)
        {
            case 0:// 4x3
                AssignRandomCards(4,3);
                break;
            case 1:// 4x4
                AssignRandomCards(4,4);
                break;
            case 2:// 6x5
                AssignRandomCards(6,5);
                break;
        }
    }

    public void AssignRandomCards(int dimensionX,int dimensionY)
    {
        matchesForLevelComplete = (dimensionX * dimensionY) / 2;
        // Here Make cell size on base of container width
        float cellWidth = cardsContainer.rect.width  / (float)dimensionX;
        float cellHeight = cardsContainer.rect.height  / (float)dimensionY;
        float cellSize = Mathf.Min(cellWidth, cellHeight);

        cellSize =cellSize-cardsContainerGrid.spacing.x;
        cardsContainerGrid.cellSize=new Vector2(cellSize,cellSize);
        int loopCount = dimensionX * dimensionY;
        
        if(cardsInt.Count<=0)
        {
             for (int i = 0; i < 30; i++)
             {
                CardHolder cardInt= Instantiate(cardTemplate, cardsContainer.transform);
                cardInt.gameObject.SetActive(false);
                cardsInt.Add(cardInt);
             } 
        }
        List<CardData> randomCardsDataList=new List<CardData>();
        int randomNo = 0;
        for (int i = 0; i < loopCount/2; i++)
        {
            while (true)
            {
                randomNo = Random.Range(0, listOfCardsData.Count);
                if (!randomCardsDataList.Contains(listOfCardsData[randomNo]))
                {
                    randomCardsDataList.Add(listOfCardsData[randomNo]);
                    break;
                }
            }
        }
        int countRandom=0;
        for (int i = 0; i < loopCount;)
        {
            cardsInt[i].ResetTheCard();
            cardsInt[i].assignCardData = randomCardsDataList[countRandom];
            cardsInt[i+1].ResetTheCard();
            cardsInt[i+1].assignCardData = randomCardsDataList[countRandom];
            i=i + 2;
            countRandom++;
        }
        
        // Shuffle the card values
        for (int i = 0; i < loopCount; i++)
        {
            int temp = cardsInt[i].transform.GetSiblingIndex();
            int randomIndex = Random.Range(i, cardsInt.Count);
            cardsInt[i].transform.SetSiblingIndex(randomIndex);
            cardsInt[randomIndex].transform.SetSiblingIndex(temp);
        }
        
        
        // Show the Cards for Upto Some Time

        StartCoroutine(ShowCardsAtStart());
        IEnumerator ShowCardsAtStart()
        {
            eventSys.enabled = false;
            for (int i = 0; i < loopCount; i++)
            {
                cardsInt[i].DisplayCardAtStart();
            }
            yield return new WaitForSeconds(1.5f);
            for (int i = 0; i < loopCount; i++)
            {
                cardsInt[i].HideCard();
            }
            yield return new WaitForSeconds(0.3f);
            eventSys.enabled = true;
        }
    }
    
    public void SelectTheCard(CardHolder selectedCard)
    {
        if (firstSelectedCard == null)
        {
            firstSelectedCard = selectedCard;
            GameSoundManager.InsSoundManager.PlaySound("CardClickSound");
        }
        else if (secondSelectedCard == null)
        {
            secondSelectedCard = selectedCard;
            eventSys.enabled = false;
            StartCoroutine(CheckForMatch());
        }
    }
    
    IEnumerator CheckForMatch()
    {
        yield return new WaitForSeconds(0.5f);

        if (firstSelectedCard.assignCardData.cardId == secondSelectedCard.assignCardData.cardId )
        {
            // Cards match
            
            firstSelectedCard.cardImg.enabled = false;
            secondSelectedCard.cardImg.enabled = false;
            firstSelectedCard = null;
            secondSelectedCard = null;
            matchCardsCount++;
            matchCardsAre.text = matchCardsCount.ToString();
            turnsCounter++;
            totalNumberOfTurns.text = turnsCounter.ToString();
            if (matchCardsCount >= matchesForLevelComplete)
            {
                totalNoOfTurnsInCongratPanel.text=turnsCounter.ToString();
                
                GameplayManager.instanceGM.ShowLevelCompletePanel();
            }
            GameSoundManager.InsSoundManager.PlaySound("RightMove");
        }
        else
        {
            // Cards do not match
            firstSelectedCard.HideCard();
            secondSelectedCard.HideCard();
            firstSelectedCard = null;
            secondSelectedCard = null;
            turnsCounter++;
            totalNumberOfTurns.text = turnsCounter.ToString();
            GameSoundManager.InsSoundManager.PlaySound("WrongMove");

        }
        yield return new WaitForSeconds(0.2f);
        eventSys.enabled = true;

    }

    public void RestartTheGamePlay(int gameMode)
    {
        matchCardsAre.text = "0";
        matchCardsCount = 0;
        totalNumberOfTurns.text = "0";
        turnsCounter=0;
        AssignCards(gameMode);
    }

    public void ThingsDoneBackToHome()
    {
        foreach (CardHolder cardHolder in cardsInt)
        {
            cardHolder.gameObject.SetActive(false);
        }
    }
    
}

[System.Serializable]
public class CardData
{
    public int cardId;
    public Sprite cardSprite;
}
