using System;
using System.Collections;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Enums;
using NSBLib.Helpers;
using UnityEditor.Rendering;
using UnityEngine;

public class Envelope : MonoBehaviour
{
    [SerializeField] private EEnvelopeType envelopeType;
    [SerializeField] private Transform destTransform;
    [SerializeField] private float tweenDuration = 0.3f;
    [SerializeField] private Ease tweenEase = Ease.Flash;
    [SerializeField] private float lifetime = 1f;
    
    public Spawner spawner;
    private Vector2 destCoords =  Vector2.zero;
    private SpriteRenderer spriteRenderer;
    private Action<Envelope> destroyAction;
    
    private TweenerCore<Vector3, Vector3, VectorOptions> envelopeTween;

    private void OnEnable()
    {
        if (destTransform != null)
        {
            destCoords = destTransform.position;
        }
        spriteRenderer = GetComponent<SpriteRenderer>();
        switch (envelopeType)
        {
            case EEnvelopeType.Golden:
                spriteRenderer.color = Color.gold;
                break;
            case EEnvelopeType.Special:
                spriteRenderer.color = new Color(0.8f, 0f,0f);
                break;
            case EEnvelopeType.Normal:
            default:
                spriteRenderer.color = Color.white;
                break;
        }
        
        GoToDestination();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        

        // StartCoroutine(DelayedDestroy());
    }

    public void Init(Action<Envelope> action)
    {
        destroyAction = action;
    }

    IEnumerator DelayedDestroy()
    {
        yield return new WaitForSeconds(lifetime);
        destroyAction(this);
    }

    public void ScheduleDestroy()
    {
        StartCoroutine(DelayedDestroy());
    }

    private void GoToDestination()
    {
        envelopeTween = transform.DOMove(destCoords, tweenDuration).SetEase(tweenEase);
        envelopeTween.OnComplete(() =>
            {
                destroyAction(this);
            }
        );
    }

    public void SetEnvelopeType(EEnvelopeType envelopeType)
    {
        this.envelopeType = envelopeType;
    }

    public void SetEaseType(Ease ease)
    {
        tweenEase = ease;
    }

    public void SetDuration(float f)
    {
        tweenDuration = f;
    }

    public void SetLifetime(float envelopeLifetime)
    {
        lifetime = envelopeLifetime;
    }

    public void SetDesination(Transform destination)
    {
        destTransform = destination;
        destCoords = destTransform.position;
    }

    public void SetPosition(Vector3 randomizePosition)
    {
        transform.position = randomizePosition;
    }
}
