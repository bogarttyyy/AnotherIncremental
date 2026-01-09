using System;
using NSBLib.Helpers;
using NSBLib.Interfaces;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class TableEnvelope : MonoBehaviour, IClickable
{
    [SerializeField] private Sprite open;
    [SerializeField] private Sprite closed;
    [SerializeField] private GameObject floatingText;

    public bool isClosed = true;

    private Animator animator;
    private SpriteRenderer sr;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        animator = floatingText?.GetComponent<Animator>();
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
            // animator.SetBool("MoveText", true);
        }
    }

    public void OnRightClicked()
    {
        throw new NotImplementedException();
    }

    private void SetSprite()
    {
        sr.sprite = isClosed ? closed : open;
    }
}
