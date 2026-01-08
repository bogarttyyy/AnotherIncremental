using System;
using Enums;
using UnityEngine;

public class Card : MonoBehaviour
{
    [SerializeField] float marketPrice;
    [SerializeField] float price;
    [SerializeField] ECardRarity rarity;
    
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
}
