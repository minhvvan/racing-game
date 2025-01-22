using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gas : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _gasAmount;

    void Update()
    {
        transform.position += Vector3.back * (_moveSpeed * Time.deltaTime);
        if (transform.position.z < -40)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        GameManager.Instance.AddGas(_gasAmount);
        Destroy(gameObject);
    }
}
