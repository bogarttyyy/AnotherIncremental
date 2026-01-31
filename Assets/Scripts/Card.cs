using System;
using System.Diagnostics;
using Enums;
using EventChannels;
using NSBLib.Interfaces;
using ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Collider2D))]
public class Card : MonoBehaviour, IClickable, IRightClickable
{
    public int marketPrice;
    public int askingPrice;
    public int boughtPrice;
    public ECardRarity rarity;
    public EDemand demand;
    public int demandValue;
    
    public EBuySell buySell;
    public int? tableIndex;
    
    
    [SerializeField] private float currentTime;
    [SerializeField] private float duration = 60f;
    [SerializeField] float percentTimeLeft = 0f;
    [SerializeField] private bool hasStarted = false;
    [SerializeField] private Image cardTimer;

    [SerializeField] private TMP_Text marketPriceText;
    [SerializeField] private TMP_Text askingPriceText;
    [SerializeField] private TMP_Text boughtPriceText;

    [SerializeField] private CardEventChannel SelectCard;
    [SerializeField] private CardEventChannel RejectCard;

    [SerializeField] private SpriteRenderer demandSpriteRenderer;
    [SerializeField] private Sprite lowDemandSprite;
    [SerializeField] private Sprite highDemandSprite;
    [SerializeField] private Sprite normalDemanSprite;
    
    private SpriteRenderer spriteRenderer;

    private void OnEnable()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentTime = duration;
        hasStarted = true;
    }

    private void FixedUpdate()
    {
        // if (hasStarted)
        //     UpdateTimer();
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

    public void SetupCard(ECardRarity newRarity, int newMarketPrice, int newAskingPrice, EBuySell newBuySell, int demandVal = 50, float newDuration = 2f, int newBoughtPrice = 0)
    {
        SetRarity(newRarity);
        SetMarketPrice(newMarketPrice);
        SetAskingPrice(newAskingPrice);
        SetBoughtPrice(newBoughtPrice);
        SetBuySell(newBuySell);
        SetDuration(newDuration);
        SetDemand(demandVal);
    }

    public void SetAskingPrice(int newAskingPrice, bool isSelling = false)
    {
        askingPrice = newAskingPrice;
        askingPriceText.text = $"A ${newAskingPrice}";
        // Not working
        // if (isSelling)
        // {
        //     if (askingPrice > boughtPrice)
        //     {
        //         askingPriceText.color = new Color32(0, 255, 0, 255);
        //     }
        //     else if (askingPrice < boughtPrice)
        //     {
        //         askingPriceText.faceColor = new Color32(255,0, 0, 255);
        //     }
        //     else
        //     {
        //         askingPriceText.faceColor = new Color32(255, 255, 0, 255);
        //     }
        // }
        
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

    public void SetDuration(float newDuration)
    {
        duration = newDuration;
    }

    public void HasStarted(bool start)
    {
        hasStarted = start;
    }

    public void SetDemand(int demandVal)
    {
        demandValue = demandVal;
        demand = demandVal switch
        {
            >= 70 => EDemand.High,
            >= 35 => EDemand.Normal,
            _ => EDemand.Low
        };

        demandSpriteRenderer.sprite = demand switch
        {
            EDemand.High => highDemandSprite,
            EDemand.Normal => normalDemanSprite,
            _ => lowDemandSprite,
        };
    }
    
    // private void UpdateTimer()
    // {
    //     
    //     if (currentTime > 0)
    //     {
    //         currentTime -= Time.deltaTime;
    //         percentTimeLeft = currentTime / duration;
    //         cardTimer.fillAmount = percentTimeLeft;
    //
    //         if (currentTime <= 0)
    //         {
    //             currentTime = 0;
    //             // Call something, cancel card maybe?
    //             RejectCard?.Invoke(this);
    //         }
    //     }
    // }
    public void ShowDemandIndicator(bool showDemandIndicator)
    {
        demandSpriteRenderer.gameObject.SetActive(showDemandIndicator);
    }
}
