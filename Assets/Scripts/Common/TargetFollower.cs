using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class TargetFollower : MonoBehaviour
{
    [SerializeField] private float _cameraOffset;
    [SerializeField] private float _offsetChangingSpeed;
    [SerializeField] private Robot _robot;

    private CinemachineVirtualCamera _cinemachineVirutalCamera;
    private Cinemachine3rdPersonFollow _cinemachinePersonFollow;

    private void Awake()
    {
        _cinemachineVirutalCamera = GetComponent<CinemachineVirtualCamera>();
        _cinemachinePersonFollow = _cinemachineVirutalCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
    }

    private void OnEnable()
    {
        _robot.Body.Died += OnChangeOffset;
    }

    private void Start()
    {
    }

    private void OnChangeOffset()
    {
        _cinemachinePersonFollow.CameraDistance = 2f;
    }

    private IEnumerator ChangeOffset()
    {
        while (_cinemachinePersonFollow.CameraDistance != _cameraOffset)
        {
            _cinemachinePersonFollow.CameraDistance = Mathf.MoveTowards(_cinemachinePersonFollow.CameraDistance, _cameraOffset, _offsetChangingSpeed * Time.deltaTime);
            yield return null;
        }
    }
}
