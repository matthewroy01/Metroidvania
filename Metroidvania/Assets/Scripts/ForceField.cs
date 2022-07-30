using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceField : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private BoxCollider2D _collider;
    [Header("Collision")]
    [SerializeField] private LayerMask _groundMask;
    [SerializeField] private Collider2D _forwardCollider;
    [Header("Push")]
    [SerializeField] private float _speed;
    private ContactFilter2D _contactFilter;
    private bool _deployed = false;

    private void Awake()
    {
        _contactFilter.layerMask = _groundMask;
        _contactFilter.useLayerMask = true;
    }

    public void TryDeploy(Vector2 deployPosition, Vector2 deployDirection)
    {
        if (_deployed)
        {
            CallBack();
        }
        else
        {
            Deploy(deployPosition, deployDirection);
        }
    }

    private void Deploy(Vector2 deployPosition, Vector2 deployDirection)
    {
        transform.position = deployPosition;
        transform.up = deployDirection;

        if (WillOverlapWithGround(Vector2.zero))
        {
            return;
        }

        gameObject.SetActive(true);

        _deployed = true;
    }

    public void CallBack()
    {
        gameObject.SetActive(false);

        _deployed = false;
    }

    public void Push()
    {
        Vector2 velocity = transform.up * _speed;

        if (WillOverlapWithGround(velocity))
        {
            _rb.velocity = Vector2.zero;
            return;
        }

        _rb.velocity = velocity;
    }

    public void Halt()
    {
        _rb.velocity = Vector2.zero;
    }

    private bool WillOverlapWithGround(Vector2 velocity)
    {
        _forwardCollider.transform.position = (Vector2)transform.position + (velocity * Time.deltaTime);
        _forwardCollider.transform.eulerAngles = new Vector3(0.0f, 0.0f, transform.eulerAngles.z);

        List<Collider2D> results = new List<Collider2D>();
        int num = Physics2D.OverlapCollider(_forwardCollider, _contactFilter, results);

        if (num <= 0)
        {
            return false;
        }

        return true;
    }
}