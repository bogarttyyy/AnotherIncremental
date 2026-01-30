using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Enums;
using UnityEngine;

public class CustomerManager : MonoBehaviour
{
    private Coroutine sellCoroutine;

    [SerializeField] private Table sellTable;
    [SerializeField] private Table customerTable;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCustomerGeneration();
    }

    public void StartCustomerGeneration()
    {
        sellCoroutine ??= StartCoroutine(GenerateCustomers());
    }

    IEnumerator GenerateCustomers()
    {
        while (sellTable.HasCards() && customerTable.HasEmptySpace())
        {
            yield return new WaitForSeconds(2f);
            var demand = CustomerDemandValue();
            Debug.Log($"CDV: {demand}");
            var card = PickRandomCard(demand);

            if (card != null)
            {
                Debug.Log($"Card picked: {card.demandValue}");
                card.buySell = EBuySell.Sell;
                card.SetAskingPrice(card.marketPrice);
                customerTable.InsertToNextEmptySlot(card);
            }
        }

        sellCoroutine = null;
    }

    private Card PickRandomCard(int dem = 1)
    {
        var cards = sellTable.GetCards();
        var presentCards = cards.Where(t => t is not null);

        var valuedCards = presentCards.Where(f => f.demandValue >= dem).ToList();
        var rand = Random.Range(0, valuedCards.Count);
        Debug.Log($"Random sell index: {valuedCards.Count} {rand}");

        if (valuedCards.Count > 0)
        {
            var pickedCard = valuedCards[rand]; 
            
            sellTable.RemoveCard(pickedCard);
            
            return pickedCard;
        }

        return null;
    }

    private Card PickRandomCard(EDemand? dem = null)
    {
        var cards = sellTable.GetCards();
        var presentCards = cards.Where(t => t);
        
        if (dem.HasValue)
        {
            presentCards = presentCards.Where(f => f.demand == dem.Value);
        }
        
        var pickedCard  = presentCards.ElementAt(Random.Range(0, presentCards.Count()));
        sellTable.RemoveCard(pickedCard);
        
        return pickedCard;
    }

    private Card PickInDemandCard()
    {
        var cards = sellTable.GetCards().Where(t => t);

        // NOTE!! Simplified version of demand determinator
        var random = Random.Range(1, 101);
        EDemand dem = random switch
        {
            >= 50 => EDemand.High,
            >= 30 => EDemand.Normal,
            _ =>  EDemand.Low
        };
        
        return PickRandomCard(dem);
    }

    private int CustomerDemandValue()
    {
        return Random.Range(1, 101);
    }

    private int RandomMarketPrice(int marketPrice)
    {
        return Mathf.RoundToInt((Random.Range(70, 100 + 1) / 100f) * marketPrice);
    }
}
