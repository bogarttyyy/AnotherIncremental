using Enums;
using UnityEngine;

public class Envelope : MonoBehaviour
{
    [SerializeField] private EEnvelopeType envelopeType;
    
    private SpriteRenderer spriteRenderer;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
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
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
