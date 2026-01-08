using System.Collections.Generic;
using Enums;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public static CardManager Instance { get; private set; }
    [SerializeField] private Card cardTemplate;

    [SerializeField] private List<Card> cardList = new();
    
    
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

    public Card CreateCard(ECardRarity rarity)
    {
        var newCard = Instantiate(cardTemplate);
        newCard.SetRarity(rarity);
        return newCard;
    }
}
