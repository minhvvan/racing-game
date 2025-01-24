using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Random = UnityEngine.Random;

public class GasSpawner : MonoBehaviour
{
    [SerializeField] private float _minInterval;
    [SerializeField] private float _maxInterval;
    
    [SerializeField] private GameObject gasPrefab;

    private BoxCollider _boxCollider;
    
    private CompositeDisposable _spawnDisposable = new CompositeDisposable();

    public GameObject gas;

    public bool hasGas = false;
    
    private void Awake()
    {
        _boxCollider = GetComponent<BoxCollider>();
    }

    public void StartGame()
    {
        float interval = Random.Range(_minInterval, _maxInterval);

        Observable.Timer(TimeSpan.FromSeconds(interval))
            .Repeat()
            .Where(_=> gas == null)
            .Subscribe(__ =>
            {
                var pos = GetRandomPosition();
                gas = Instantiate(gasPrefab, pos, Quaternion.identity);
                gas.OnDestroyAsObservable().Subscribe(_ => gas = null);
                hasGas = true;
            })
            .AddTo(_spawnDisposable);
    }

    public void StopGame()
    {
        _spawnDisposable.Clear(); // 모든 구독 제거
    }
    
    private Vector3 GetRandomPosition()
    {
        Bounds bounds = _boxCollider.bounds;
        return new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            0,
            Random.Range(bounds.min.z, bounds.max.z)
        );
    }
}
