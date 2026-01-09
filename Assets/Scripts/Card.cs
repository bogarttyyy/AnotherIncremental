using Enums;
using EventChannels;
using NSBLib.Interfaces;
using ScriptableObjects;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Card : MonoBehaviour, IClickable
{
    public int marketPrice;
    public int askingPrice;
    public int boughtPrice;
    public ECardRarity rarity;
    public EBuySell buySell;
    public int? tableIndex;

    [SerializeField] private TMP_Text marketPriceText;
    [SerializeField] private TMP_Text askingPriceText;
    [SerializeField] private TMP_Text boughtPriceText;

    [SerializeField] private CardEventChannel SelectCard;
    [SerializeField] private CardEventChannel RejectCard;
    
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

    public void SetupCard(ECardRarity newRarity, int newMarketPrice, int newAskingPrice, EBuySell newBuySell, int newBoughtPrice = 0)
    {
        SetRarity(newRarity);
        SetMarketPrice(newMarketPrice);
        SetAskingPrice(newAskingPrice);
        SetBoughtPrice(newBoughtPrice);
        SetBuySell(newBuySell);
    }

    public void SetAskingPrice(int newAskingPrice)
    {
        askingPrice = newAskingPrice;
        askingPriceText.text = $"A ${newAskingPrice}";
        askingPriceText.gameObject.SetActive(askingPrice > 0);
    }

    public void SetMarketPrice(int newMarketPrice)
    {
        marketPrice = newMarketPrice;
        marketPriceText.text = $"M ${newMarketPrice}";
    }

    public void SetBoughtPrice(int newBoughtPrice)
    {
        boughtPrice = newBoughtPrice;
        boughtPriceText.text = $"B ${newBoughtPrice}";
        boughtPriceText.gameObject.SetActive(boughtPrice > 0);
    }

    public void OnClicked()
    {
        // NSBLogger.Log($"Clicked card {this.name}");
        SelectCard?.Invoke(this);
    }

    public void OnRightClicked()
    {
        RejectCard?.Invoke(this);
    }

    public bool IsBuy()
    {
        return buySell == EBuySell.Buy;
    }

    public bool IsSell()
    {
        return buySell == EBuySell.Sell;
    }

    public void SetBuySell(EBuySell newBuySell)
    {
        buySell = newBuySell;
    }
}
