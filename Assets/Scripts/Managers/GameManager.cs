using System;
using Enums;
using EventChannels;
using Managers;
using NSBLib.EventChannelSystem;
using NSBLib.Helpers;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private int cash;

    [SerializeField] private IntEventChannel updateCash;
    [SerializeField] private CardEventChannel addToInventory;

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
        updateCash?.Invoke(cash);
    }

    public void OnCardClicked(Card card)
    {
        NSBLogger.Log($"Card clicked {card.askingPrice}");
        switch (card.buySell)
        {
            case EBuySell.Buy:
                BuyLogic(card);
                break;
            case EBuySell.Sell:
                SellLogic(card);
                break;
        }
    }

    private void SellLogic(Card card)
    {
        throw new NotImplementedException();
    }

    private void BuyLogic(Card card)
    {
        NSBLogger.Log($"Asking for {card.askingPrice}");
        NSBLogger.Log($"Cash: {cash}");
        if (card.askingPrice < cash)
        {
            cash -= card.askingPrice;
            updateCash?.Invoke(cash);
            addToInventory?.Invoke(card);
        }
        else
        {
            NSBLogger.Log($"Not enough cash to buy {card.askingPrice}");
        }
    }
}
