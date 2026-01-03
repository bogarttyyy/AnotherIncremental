using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Enums;
using NSBLib.Helpers;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Envelope prefab;
    [SerializeField] private float spawnRate = 1f;
    [SerializeField] private float tweenDuration = .5f;
    [SerializeField] private float envelopeLifetime = 1f;
    [SerializeField] private Vector2 spawnPointRange;
    [SerializeField] private Ease tweenEase = Ease.Flash;
    public Transform target;
    [SerializeField] private List<Envelope> objects = new();


    private ObjectPool<Envelope> pool;
    
    private void Start()
    {
        pool = new ObjectPool<Envelope>(() =>
        {
            return Instantiate(prefab, RandomizePosition(), Quaternion.identity, transform);
        }, env =>
        {
            env.gameObject.SetActive(true);
        }, env =>
        {
            env.gameObject.SetActive(false);
        }, env =>
        {
            Destroy(env.gameObject);
        }, false, 100, 10000);
        StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnRate);
            // Envelope envelope = Instantiate(prefab, RandomizePosition(), Quaternion.identity, transform);
            Envelope envelope = pool.Get();
            RandomizeEnvelope(envelope);
            NSBLogger.Log($"Spawned at: {envelope.transform.position}");
            // AddObject(envelope);
        }
    }

    private void RandomizeEnvelope(Envelope envelope)
    {
        envelope.SetEnvelopeType((EEnvelopeType)Random.Range(0, 3));
        envelope.SetEaseType(tweenEase);
        envelope.SetDuration(tweenDuration);
        envelope.SetLifetime(envelopeLifetime);
        envelope.SetDesination(target);
        envelope.ScheduleDestroy();
    }

    private Vector3 RandomizePosition()
    {
        var xPos =  Random.Range(0f, spawnPointRange.x);
        var yPos = Random.Range(0f, spawnPointRange.y);

        xPos = SetPolarity(xPos);
        yPos = SetPolarity(yPos);
        
        return  new Vector3(xPos + transform.position.x, yPos + transform.position.y, 0f);
    }

    private float SetPolarity(float pos)
    {
        switch (Random.Range(1, 3))
        {
            case 1:
                pos *= -1f;
                break;
            case 2:
                pos *= 1f;
                break;
        }
        return pos;
    }

    private void AddObject(Envelope obj)
    {
        objects.Add(obj);
    }

    public void RemoveEnvelope(Envelope envelope)
    {
        objects.Remove(envelope);
    }
}
