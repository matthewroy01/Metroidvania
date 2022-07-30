using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShoot : MonoBehaviour
{
    [Header("Positioning")]
    [SerializeField] private Transform _arm;
    [SerializeField] private Transform _shootPoint;
    [Header("Force Field")]
    [SerializeField] private ForceField _forceField;
    private bool _rightClickInputValue = false;
    private bool _rightClickHeldInputValue = false;
    private bool _previousRightClickHeldInputValue = false;
    private bool _leftClickInputValue = false;

    private void Awake()
    {
        _forceField.CallBack();
    }

    private void Update()
    {
        Input();
        LookAtMouse();
        ShootForceField();
        ShootProjectile();
    }

    private void Input()
    {
        _previousRightClickHeldInputValue = _rightClickHeldInputValue;

        _rightClickInputValue = Mouse.current.rightButton.wasPressedThisFrame;
        _rightClickHeldInputValue = Mouse.current.rightButton.isPressed;
        _leftClickInputValue = Mouse.current.leftButton.wasPressedThisFrame;
    }

    private void LookAtMouse()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        _arm.up = mousePos - (Vector2)_arm.position;
    }

    private void ShootForceField()
    {
        if (_rightClickInputValue)
        {
            _forceField.TryDeploy(_shootPoint.position, _arm.up);
            return;
        }

        if (_previousRightClickHeldInputValue == true && _rightClickHeldInputValue == false)
        {
            _forceField.Halt();
            return;
        }

        if (_rightClickHeldInputValue)
        {
            _forceField.Push();
        }
    }

    private void ShootProjectile()
    {
        if (!_leftClickInputValue)
        {
            return;
        }
    }
}