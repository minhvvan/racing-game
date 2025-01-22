using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Random = UnityEngine.Random;

public class GasSpawner : MonoBehaviour
{
    [SerializeField] private float _minInterval;
    [SerializeField] private float _maxInterval;
    
    [SerializeField] private GameObject gasPrefab;

    private BoxCollider _boxCollider;
    
    private CompositeDisposable _spawnDisposable = new CompositeDisposable();
    
    private void Awake()
    {
        _boxCollider = GetComponent<BoxCollider>();
    }

    public void StartGame()
    {
        float interval = Random.Range(_minInterval, _maxInterval);

        Observable.Timer(TimeSpan.FromSeconds(interval))
            .Repeat()
            .Subscribe(__ =>
            {
                var pos = GetRandomPosition();
                Instantiate(gasPrefab, pos, Quaternion.identity);
            })
            .AddTo(_spawnDisposable);
    }

    public void StopGame()
    {
        _spawnDisposable.Clear(); // 모든 구독 제거
    }
    
    public Vector3 GetRandomPosition()
    {
        Bounds bounds = _boxCollider.bounds;
        return new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            0,
            Random.Range(bounds.min.z, bounds.max.z)
        );
    }
}
