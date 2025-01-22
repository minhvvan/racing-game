using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;

public class InputController : MonoBehaviour
{
    [SerializeField] private Button _btnLeft;
    [SerializeField] private Button _btnRight;
    
    [SerializeField] private VehicleController _vehicleController;
    
    private readonly ReactiveProperty<bool> _isLeftPressed = new ReactiveProperty<bool>(false);
    private readonly ReactiveProperty<bool> _isRightPressed = new ReactiveProperty<bool>(false);
    
    void Start()
    {
        _btnLeft.OnPointerDownAsObservable()
            .Subscribe(_ => _isLeftPressed.Value = true)
            .AddTo(this);
        
        _btnLeft.OnPointerUpAsObservable()
            .Subscribe(_ => _isLeftPressed.Value = false)
            .AddTo(this);
            
        _btnRight.OnPointerDownAsObservable()
            .Subscribe(_ => _isRightPressed.Value = true)
            .AddTo(this);
        
        _btnRight.OnPointerUpAsObservable()
            .Subscribe(_ => _isRightPressed.Value = false)
            .AddTo(this);
            
        this.UpdateAsObservable()
            .Subscribe(_ => {
                if(_isLeftPressed.Value) MoveLeft();
                if(_isRightPressed.Value) MoveRight();
            })
            .AddTo(this);
    }

    private void MoveRight()
    {
        _vehicleController.MoveRight();
    }

    private void MoveLeft()
    {
        _vehicleController.MoveLeft();
    }

    void Update()
    {
        
    }
}
