using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Enums;
using NSBLib.Helpers;
using UnityEngine;
using Random = UnityEngine.Random;

public class CardManager : MonoBehaviour
{
    public static CardManager Instance { get; private set; }
    [SerializeField] private Table buyTable;
    private List<Transform> buySlots;
    [SerializeField] private Table sellTable;
    private List<Transform> sellSlots;
    
    [SerializeField] private Card cardTemplate;
    [SerializeField] private List<Card> cardList = new();
    [SerializeField] private int cardsToBuySize = 3;
    [SerializeField] private List<Card> cardsToBuy;
    [SerializeField] private int cardsToSellSize = 10;
    [SerializeField] private List<Card> cardsToSell;
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        InitializeBuySellList();
        StartCoroutine(GenerateBuyCards());
    }

    IEnumerator GenerateBuyCards()
    {
        while (cardsToBuy.Any(t => !t))
        {
            yield return new WaitForSeconds(1f);
            var newCard = CreateBuyCard((ECardRarity)Random.Range(0, 4));
            InsertToSlot(newCard);
        }
    }

    public Card CreateBuyCard(ECardRarity rarity)
    {
        var newCard = Instantiate(cardTemplate);
        var marketPrice = RandomizeMarketPrice(rarity);
        newCard.SetupCard(rarity, marketPrice, marketPrice,EBuySell.Buy);
        return newCard;
    }

    private void InsertToSlot(Card card)
    {
        var emptySlotIndex = cardsToBuy.FindIndex(t => !t);
        
        card.transform.SetParent(buySlots[emptySlotIndex]);
        card.transform.position = new  Vector3(buySlots[emptySlotIndex].transform.position.x, buySlots[emptySlotIndex].transform.position.y, buySlots[emptySlotIndex].transform.position.z -1);
        cardsToBuy[emptySlotIndex] = card;
    }

    private float RandomizeMarketPrice(ECardRarity rarity)
    {
        var marketPrice = rarity switch
        {
            ECardRarity.Rare => Mathf.Round(Random.Range(80f, 100f)),
            ECardRarity.Uncommon => Mathf.Round(Random.Range(25f, 80f)),
            _ => Mathf.Round(Random.Range(1f, 25f))
        };
        return marketPrice;
    }

    private void InitializeBuySellList()
    {
        cardsToBuy ??= new();
        if (cardsToBuySize > 0)
        {
            for (int i = 0; i < cardsToBuySize; i++)
            {
                cardsToBuy.Add(null);
            }
        }
        
        cardsToSell ??= new();
        if (cardsToSellSize > 0)
        {
            for (int i = 0; i < cardsToSellSize; i++)
            {
                cardsToSell.Add(null);
            }
        }
        
        if (buyTable != null)
            buySlots = buyTable.GetSlotList();
        else
            NSBLogger.Log("Buy slots not found");
        
        if (sellTable != null)
            sellSlots = sellTable.GetSlotList();
        else
            NSBLogger.Log("Sell slots not found");
    }
}
