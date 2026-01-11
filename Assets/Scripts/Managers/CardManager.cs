using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Enums;
using NSBLib.Helpers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Managers
{
    public class CardManager : MonoBehaviour
    {
        public static CardManager Instance { get; private set; }
        [SerializeField] Vector3 originalScale;
        public Table buyTable;
        private List<Transform> buySlots;
        public Table sellTable;
        [SerializeField] private Table customerTable;
        private List<Transform> sellSlots;


        [SerializeField] private Card cardTemplate;
        [SerializeField] private List<Card> cardsToBuy;
        [SerializeField] private List<Card> cardsToSell;
        [SerializeField] private List<Card> cardInventory = new();

        private Coroutine buyCoroutine;
        private Coroutine sellCoroutine;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        private void Start()
        {
            InitializeBuySellList();
            buyCoroutine = StartCoroutine(GenerateBuyCards());
            sellCoroutine = StartCoroutine(GenerateCustomers());
        }

        IEnumerator GenerateBuyCards()
        {
            if (buyTable != null)
            {
                while (buyTable.GetCards().Any(t => !t))
                {
                    yield return new WaitForSeconds(1f);
                    var newCard = CreateBuyCard((ECardRarity)Random.Range(0, 4));
                    buyTable.InsertToNextEmptySlot(newCard);
                }

                buyCoroutine = null;
            }
        }

        IEnumerator GenerateCustomers()
        {
            if (sellTable != null)
            {
                while (sellTable.GetCards().Any(t => t) && customerTable.GetCards().Any(t => !t))
                {
                    yield return new WaitForSeconds(2f);
                    var card = sellTable.PickRandomCard();
                    card.buySell = EBuySell.Sell;
                    card.SetAskingPrice(Mathf.RoundToInt((Random.Range(70, 100 + 1) / 100f) * card.marketPrice));
                    customerTable.InsertToNextEmptySlot(card);
                }

                sellCoroutine = null;
            }
        }

        public Card CreateBuyCard(ECardRarity rarity)
        {
            var newCard = Instantiate(cardTemplate);
            var marketPrice = RandomizeMarketPrice(rarity);
            var askingPrice = Mathf.RoundToInt((Random.Range(60, 85 + 1) / 100f) * marketPrice);
            newCard.SetupCard(rarity, marketPrice, askingPrice, EBuySell.Buy);
            return newCard;
        }

        private int RandomizeMarketPrice(ECardRarity rarity)
        {
            var marketPrice = rarity switch
            {
                ECardRarity.Rare => Random.Range(80, 100),
                ECardRarity.Uncommon => Random.Range(25, 80),
                _ => Random.Range(1, 25)
            };
            return marketPrice;
        }

        private void InitializeBuySellList()
        {
            if (buyTable != null)
                buySlots = buyTable.GetSlotList();
            else
                NSBLogger.Log("Buy slots not found");

            if (sellTable != null)
                sellSlots = sellTable.GetSlotList();
            else
                NSBLogger.Log("Sell slots not found");
        }

        public void AddToInventory(Card card)
        {
            buyTable.RemoveCard(card);
            sellTable.InsertToNextEmptySlot(card);

            buyCoroutine ??= StartCoroutine(GenerateBuyCards());
            sellCoroutine ??= StartCoroutine(GenerateCustomers());
        }

        public void SellCard(Card card)
        {
            customerTable.RemoveCard(card);
            sellCoroutine ??= StartCoroutine(GenerateCustomers());
        }

        public void RejectCard(Card card)
        {
            switch (card.buySell)
            {
                case EBuySell.Buy:
                    buyTable.RemoveCard(card);
                    buyCoroutine ??= StartCoroutine(GenerateBuyCards());
                    break;
                case EBuySell.Sell:
                    customerTable.RemoveCard(card);
                    sellTable.InsertToNextEmptySlot(card);
                    sellCoroutine ??= StartCoroutine(GenerateCustomers());
                    break;
            }
        }
    }
}