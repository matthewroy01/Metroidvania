using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D _rb;
    [Header("Movement")]
    [SerializeField] private float _movementSpeed = 1.0f;
    [SerializeField] private float _movementInputResponsiveness = 1.0f;
    [SerializeField] private float _movementInputResponsivenessInAir = 1.0f;
    [SerializeField] private float _terminalVelocity = float.MaxValue;
    [Header("Jumping")]
    [SerializeField] private float _jumpForce;
    [SerializeField] private Transform _groundTransform;
    [SerializeField] private LayerMask _groundMask;
    private float _movementInputValue = 0.0f;
    private bool _jumpInputValue = false;
    private bool _grounded = false;

    private void Update()
    {
        Input();
        CheckGround();
        Jumping();
    }

    private void FixedUpdate()
    {
        Movement();
        TerminalVelocity();
    }

    private void Input()
    {
        float responsiveness = _grounded ? _movementInputResponsiveness : _movementInputResponsivenessInAir;
        _movementInputValue = Mathf.Lerp(_movementInputValue, GetMovementInputValue(), responsiveness * Time.deltaTime);
        _jumpInputValue = Keyboard.current.spaceKey.wasPressedThisFrame;
    }

    private float GetMovementInputValue()
    {
        bool aKey = Keyboard.current.aKey.isPressed;
        bool dKey = Keyboard.current.dKey.isPressed;

        if ((aKey && dKey) || (!aKey && !dKey))
        {
            return 0.0f;
        }

        if (aKey)
        {
            return -1.0f;
        }

        if (dKey)
        {
            return 1.0f;
        }

        return 0.0f;
    }

    private void CheckGround()
    {
        RaycastHit2D hit = Physics2D.Raycast(_groundTransform.position, Vector2.down, 0.1f, _groundMask);

        _grounded = hit.transform != null;
    }

    private void Movement()
    {
        _rb.AddForce(new Vector2(_movementInputValue, 0.0f) * _movementSpeed, ForceMode2D.Force);
    }

    private void Jumping()
    {
        if (!_jumpInputValue || !_grounded)
        {
            return;
        }

        _rb.velocity = new Vector2(_rb.velocity.x, 0.0f);
        _rb.AddForce(Vector2.up * _jumpForce, ForceMode2D.Force);
    }

    private void TerminalVelocity()
    {
        if (_rb.velocity.x > _terminalVelocity)
        {
            _rb.velocity = new Vector2(_terminalVelocity, _rb.velocity.y);
        }

        if (_rb.velocity.x < _terminalVelocity * -1.0f)
        {
            _rb.velocity = new Vector2(_terminalVelocity * -1.0f, _rb.velocity.y);
        }
    }
}