using System.Collections;
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
            var card = PickRandomCard();
            card.buySell = EBuySell.Sell;
            card.SetAskingPrice(Mathf.RoundToInt((Random.Range(70, 100 + 1) / 100f) * card.marketPrice));
            customerTable.InsertToNextEmptySlot(card);
        }

        sellCoroutine = null;
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
    
    
}
