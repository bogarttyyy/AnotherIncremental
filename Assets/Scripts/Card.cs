using System;
using Enums;
using EventChannels;
using NSBLib.Enums;
using NSBLib.Helpers;
using NSBLib.Interfaces;
using ScriptableObjects;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Card : MonoBehaviour, IClickable
{
    public int marketPrice;
    public int askingPrice;
    public ECardRarity rarity;
    public EBuySell buySell;
    public int? tableIndex;

    [SerializeField] private TMP_Text marketPriceText;
    [SerializeField] private TMP_Text askingPriceText;

    [SerializeField] private CardEventChannel SelectCard;
    
    SpriteRenderer spriteRenderer;

    private void OnEnable()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetRarity(ECardRarity cardRarity)
    {
        spriteRenderer.color = cardRarity switch
        {
            ECardRarity.Uncommon => new Color32(0, 128, 0, 255),
            ECardRarity.Rare => new Color32(0, 0, 128, 255),
            ECardRarity.Common => new Color32(128, 128, 128, 255),
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

    public void SetupCard(ECardRarity newRarity, int newMarketPrice, int newAskingPrice, EBuySell newBuySell)
    {
        SetRarity(newRarity);
        SetMarketPriceText(newMarketPrice);
        SetAskingPriceText(newAskingPrice);
        buySell = newBuySell;
    }

    public void SetAskingPriceText(int newAskingPrice)
    {
        askingPrice = newAskingPrice;
        askingPriceText.text = $"Asking\n${newAskingPrice}";
    }

    public void SetMarketPriceText(int newMarketPrice)
    {
        marketPrice = newMarketPrice;
        marketPriceText.text = $"Market\n${newMarketPrice}";
    }

    public void OnClicked()
    {
        // NSBLogger.Log($"Clicked card {this.name}");
        SelectCard?.Invoke(this);
    }

    public bool IsBuy()
    {
        return buySell == EBuySell.Buy;
    }

    public bool IsSell()
    {
        return buySell == EBuySell.Sell;
    }
}
