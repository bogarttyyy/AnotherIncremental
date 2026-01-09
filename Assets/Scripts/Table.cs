using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NSBLib.Helpers;
using UnityEngine;
using Random = UnityEngine.Random;

public class Table : MonoBehaviour
{
    [SerializeField] List<Card> cards;
    [SerializeField] private float scaleCard = 1f;
    [SerializeField] private Card prefab;
    [SerializeField] private Transform slotGroup;
    [SerializeField] private bool hasGuide;
    private List<Transform> slotList;

    private void OnEnable()
    {
        SetupSlots();
        cards = new List<Card>();
        foreach (var slot in slotList)
        {
            cards.Add(null);
        }
    }

    private void SetupSlots()
    {
        SetupVisualGuide();
        
        var transforms = slotGroup.GetComponentsInChildren<Transform>();
        var slotTransforms = transforms.ToList();
        // Remove first transform since it's the transform of the table
        slotTransforms.RemoveAt(0);
        slotList = slotTransforms;

        NSBLogger.Log($"{slotList.Count}");
    }

    private void SetupVisualGuide()
    {
        if (!hasGuide)
        {
            var renderers = slotGroup.GetComponentsInChildren<SpriteRenderer>();
            foreach (var r in renderers)
            {
                r.enabled = false;
            }
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // StartCoroutine(SpawnTableEnvelopes());
    }

    IEnumerator SpawnTableEnvelopes()
    {
        for (int i = 0; i < cards.Count; i++)
        {
            yield return new WaitForSeconds(0.1f);
            var newEnv =  Instantiate(prefab, slotList[i], true);
            newEnv.transform.position = new  Vector3(slotList[i].transform.position.x, slotList[i].transform.position.y, slotList[i].transform.position.z -1);
            
            cards[i] = newEnv;
        }
    }

    public List<Transform> GetSlotList()
    {
        return slotList;
    }

    public List<Card> GetCards()
    {
        return cards;
    }
    
    public void InsertToNextEmptySlot(Card card, bool isTemporary = false)
    {
        if (!card.gameObject.activeSelf)
        {
            card.gameObject.SetActive(true);
        }
        
        var emptySlotIndex = cards.FindIndex(t => !t);

        if (!isTemporary)
        {
            card.tableIndex = emptySlotIndex;
        }
        
        card.transform.SetParent(slotList[emptySlotIndex]);
        card.transform.localScale *= scaleCard;
        card.transform.position = new  Vector3(slotList[emptySlotIndex].transform.position.x, slotList[emptySlotIndex].transform.position.y, slotList[emptySlotIndex].transform.position.z -1);
        cards[emptySlotIndex] = card;
    }

    public void RemoveCard(Card card)
    {
        NSBLogger.Log($"card index: {card.tableIndex}");
        if (card.tableIndex.HasValue)
        {
            cards[card.tableIndex.Value] = null;
            card.gameObject.SetActive(false);
        }
    }

    public Card PickRandomCard()
    {
        var presentCards = cards.Where(t => t);
        var pickedCard  = presentCards.ElementAt(Random.Range(0, presentCards.Count()));
        RemoveCard(pickedCard);
            
        return pickedCard;
    }
}
