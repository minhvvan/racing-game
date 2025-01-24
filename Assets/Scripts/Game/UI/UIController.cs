using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private RectTransform GameStartUI;
    [SerializeField] private RectTransform GameOverUI;
    [SerializeField] private RectTransform InGameUI;

    [SerializeField] private Button _buttonStartGame;
    [SerializeField] private Button _buttonRetry;
    [SerializeField] private Button _buttonExit;
    
    [SerializeField] private TMP_Text _textGas;

    private void Start()
    {
        _buttonStartGame.OnClickAsObservable().Subscribe(unit => GameManager.Instance.StartGame()).AddTo(this);
        _buttonRetry.OnClickAsObservable().Subscribe(unit =>
        {
            GameManager.Instance.RetryGame();
            GameOverUI.gameObject.SetActive(false);
        }).AddTo(this);
        _buttonExit.OnClickAsObservable().Subscribe(unit => GameManager.Instance.ExitGame()).AddTo(this);
    }

    public void InitGame()
    {
        GameStartUI.gameObject.SetActive(true);
    }

    public void StartGame()
    {
        GameStartUI.gameObject.SetActive(false);
        GameOverUI.gameObject.SetActive(false);

        InGameUI.gameObject.SetActive(true);
    }
    
    public void GameOver()
    {
        InGameUI.gameObject.SetActive(false);
        GameOverUI.gameObject.SetActive(true);
    }

    public void UpdateGas(float gas)
    {
        _textGas.text = $"{gas}";
    }
}
