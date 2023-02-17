using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;

public class InputControlChanger : MonoBehaviour
{
    [SerializeField] private MonoBehaviour _playerInput;
    [SerializeField] private TouchSimulation _touchSimulation;

    private void Awake()
    {
        if(SystemInfo.deviceType == DeviceType.Handheld)
        {
            _touchSimulation.enabled = false;
            _playerInput.enabled = true;
        }
        else
        {
            _touchSimulation.enabled = true;
            _playerInput.enabled = false;
        }
    }
}
