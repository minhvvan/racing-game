using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance => _instance;

    private readonly Subject<Unit> _gameStart = new Subject<Unit>();
    private readonly ReactiveProperty<float> _gas = new ReactiveProperty<float>(100f);

    [SerializeField] private RoadManager _roadManager;
    [SerializeField] private UIController _uiController;
    [SerializeField] private GasSpawner _gasSpawner;
    [SerializeField] private InputController _inputController;
    
    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }
        
        _instance = this;
    }
    
    void Start()
    {
        _uiController.InitGame();

        // gas가 0이 되면 게임오버
        _gas.Where(x => x <= 0)
            .Subscribe(_ => GameOver())
            .AddTo(this);
            
        // 게임 시작할 때만 가스가 줄어들도록
        _gameStart.Subscribe(_ =>
            {
                _uiController.UpdateGas(_gas.Value);
                
                Observable.Timer(TimeSpan.FromSeconds(1))
                    .Repeat()
                    .TakeUntil(_gas.Where(x => x <= 0))
                    .Subscribe(__ =>
                    {
                        _gas.Value -= 10f;
                        _uiController.UpdateGas(_gas.Value);
                    })
                    .AddTo(this);
                
                _uiController.StartGame();
            })
            .AddTo(this);
    }

    public void StartGame()
    {
        _roadManager.StartGame();
        _gasSpawner.StartGame();
        _gas.Value = 100f;
        _gameStart.OnNext(Unit.Default);
    }
    
    public void RetryGame()
    {
        StartGame();
        _inputController.Reset();
    }
    
    public void ExitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    private void GameOver()
    {
        _roadManager.StopGame();
        _gasSpawner.StopGame();
        _inputController.StopInput();
        _uiController.GameOver();
    }

    public void AddGas(float amount)
    {
        _gas.Value += amount;
        _uiController.UpdateGas(_gas.Value);
    }
}
