using System;
using NSBLib.Helpers;
using NSBLib.Interfaces;
using UnityEngine;

public class TableEnvelope : MonoBehaviour, IClickable
{
    [SerializeField] private Sprite open;
    [SerializeField] private Sprite closed;

    public bool isClosed = true;
    
    private SpriteRenderer sr;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        SetSprite();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClicked()
    {
        // if closed, open envelope
        if (isClosed)
        {
            isClosed = false;
            SetSprite();
        }
    }

    private void SetSprite()
    {
        sr.sprite = isClosed ? closed : open;
    }
}
