using System;
using Enums;
using NSBLib.Enums;
using ScriptableObjects;
using UnityEngine;

public class Card : MonoBehaviour
{
    [SerializeField] float marketPrice;
    [SerializeField] float askingPrice;
    [SerializeField] ECardRarity rarity;
    [SerializeField] EBuySell buySell;
    
    SpriteRenderer spriteRenderer;

    private void OnEnable()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetRarity(ECardRarity cardRarity)
    {
        spriteRenderer.color = cardRarity switch
        {
            ECardRarity.Uncommon => new Color32(0, 255, 0, 255),
            ECardRarity.Rare => new Color32(0, 0, 255, 255),
            ECardRarity.Common => new Color32(255, 255, 255, 255),
            _ => spriteRenderer.color
        };
        rarity = cardRarity;
    }

    public void SetupCard(CardSO card)
    {
        SetRarity(card.rarity);
        marketPrice = card.marketPrice;
        askingPrice = card.price;
    }

    public void SetupCard(ECardRarity newRarity, float newMarketPrice, float newAskingPrice, EBuySell newBuySell)
    {
        SetRarity(newRarity);
        marketPrice = newMarketPrice;
        askingPrice = newAskingPrice;
        buySell = newBuySell;
    }
}
