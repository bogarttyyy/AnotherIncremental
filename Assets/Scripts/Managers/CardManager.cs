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
        [SerializeField] private Table buyTable;
        private List<Transform> buySlots;
        [SerializeField] private Table sellTable;
        private List<Transform> sellSlots;
    
        [SerializeField] private Card cardTemplate;
        [SerializeField] private int cardsToBuySize = 3;
        [SerializeField] private List<Card> cardsToBuy;
        [SerializeField] private int cardsToSellSize = 10;
        [SerializeField] private List<Card> cardsToSell;
        [SerializeField] private List<Card> cardInventory = new();
        
        private Coroutine buyCoroutine;
    
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
            buyCoroutine = StartCoroutine(GenerateBuyCards());
        }

        IEnumerator GenerateBuyCards()
        {
            while (buyTable.GetCards().Any(t => !t))
            {
                yield return new WaitForSeconds(1f);
                var newCard = CreateBuyCard((ECardRarity)Random.Range(0, 4));
                buyTable.InsertToNextEmptySlot(newCard);
            }

            buyCoroutine = null;
        }

        public Card CreateBuyCard(ECardRarity rarity)
        {
            var newCard = Instantiate(cardTemplate);
            var marketPrice = RandomizeMarketPrice(rarity);
            newCard.SetupCard(rarity, marketPrice, marketPrice,EBuySell.Buy);
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
        }
    }
}
