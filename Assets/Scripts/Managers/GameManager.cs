using System;
using Enums;
using EventChannels;
using NSBLib.EventChannelSystem;
using NSBLib.Helpers;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private float duration = 60f;
    [SerializeField] private float currentTime;
    [SerializeField] private int cash;

    [SerializeField] private IntEventChannel updateCash;
    [SerializeField] private CardEventChannel addToInventory;
    [SerializeField] private CardEventChannel sellCard;
    [SerializeField] private FloatEventChannel updateTime;
    
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
        currentTime = duration;
    }

    private void FixedUpdate()
    {
        UpdateTimer();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        cash = 1000;
        currentTime = duration;
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
        NSBLogger.Log($"SOLD! ${card.askingPrice}");
        cash += card.askingPrice;
        updateCash?.Invoke(cash);
        sellCard?.Invoke(card);
        
    }

    private void BuyLogic(Card card)
    {
        NSBLogger.Log($"Asking for {card.askingPrice}");
        NSBLogger.Log($"Cash: {cash}");
        if (card.askingPrice < cash)
        {
            cash -= card.askingPrice;
            updateCash?.Invoke(cash);
            
            UpdateBoughtCard(card);
            addToInventory?.Invoke(card);
        }
        else
        {
            NSBLogger.Log($"Not enough cash to buy {card.askingPrice}");
        }
    }

    private void UpdateBoughtCard(Card card)
    {
        card.SetBoughtPrice(card.askingPrice);
        card.SetAskingPrice(0);
        card.SetBuySell(EBuySell.Unknown);
    }

    private void UpdateTimer()
    {
        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            updateTime?.Invoke(currentTime / duration);

            if (currentTime <= 0)
            {
                currentTime = 0;
                updateTime?.Invoke(0f);
                // Trigger Game Over or Time Up logic here
            }
        }
    }

    public void OnReset(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            // NSBLogger.Log("Reset");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
