using System;
using Unity.Cinemachine;
using UnityEngine;

public class UIPlayerHealth : MonoBehaviour
{
    [SerializeField] private CinemachineCamera _camera;
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 _offset;
    
    void Update()
    {
        transform.rotation = _camera.transform.rotation;
        transform.position = target.transform.position + _offset;
    }
}

