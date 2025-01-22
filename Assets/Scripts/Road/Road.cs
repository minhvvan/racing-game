using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Road : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    public Action<Road> onExit; 

    void Update()
    {
        transform.position += Vector3.back * (_moveSpeed * Time.deltaTime);
        if (transform.position.z < -40)
        {
            onExit.Invoke(this);
        }
    }
}
