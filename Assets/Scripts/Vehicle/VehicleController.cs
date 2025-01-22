using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleController : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _minXPos;
    [SerializeField] private float _maxXPos;
    
    public void MoveRight()
    {
        float currentPos = Mathf.Min(transform.position.x + _moveSpeed, _maxXPos);
        transform.position = new Vector3(currentPos, 0, 0f);
    }
    
    public void MoveLeft()
    {
        float currentPos = Mathf.Max(transform.position.x - _moveSpeed, _minXPos);
        transform.position = new Vector3(currentPos, 0, 0f);
    }
}
