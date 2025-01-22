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

    private CompositeDisposable _inputDisposable = new CompositeDisposable();
    
    void Start()
    {
        _btnLeft.OnPointerDownAsObservable()
            .Subscribe(_ => _isLeftPressed.Value = true)
            .AddTo(_inputDisposable);
        
        _btnLeft.OnPointerUpAsObservable()
            .Subscribe(_ => _isLeftPressed.Value = false)
            .AddTo(_inputDisposable);
            
        _btnRight.OnPointerDownAsObservable()
            .Subscribe(_ => _isRightPressed.Value = true)
            .AddTo(_inputDisposable);
        
        _btnRight.OnPointerUpAsObservable()
            .Subscribe(_ => _isRightPressed.Value = false)
            .AddTo(_inputDisposable);
            
        this.UpdateAsObservable()
            .Subscribe(_ => {
                if(_isLeftPressed.Value) MoveLeft();
                if(_isRightPressed.Value) MoveRight();
            })
            .AddTo(_inputDisposable);
    }

    private void MoveRight()
    {
        _vehicleController.MoveRight();
    }

    private void MoveLeft()
    {
        _vehicleController.MoveLeft();
    }

    public void StopInput()
    {
        _inputDisposable.Clear();
        _isLeftPressed.Value = false;
        _isRightPressed.Value = false;
    }
    
    public void Reset()
    {
        Start();
        _vehicleController.ResetPosition();
    }
}
