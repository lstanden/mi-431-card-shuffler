using System;
using System.Security.Cryptography;
using UnityEngine;


public delegate void MoveDone();

/// <summary>
/// A component which can be added to a GameObject temporarily to move it from point A to point B
/// </summary>
public class CardMover : MonoBehaviour
{
    [Header("Inscribed")]
    public float Duration;

    public Vector3 TargetPosition;
    public MoveDone Callback;
    public bool Flip;

    [Header("Dynamic")]
    public float _timePassed;

    public Vector3 _start;
    public Quaternion startTranform;
    public Quaternion endTransform;
    private Card _card;

    private void Start()
    {
        _timePassed = 0f;
        _start = transform.position;
        _card = GetComponent<Card>();

        if (transform.position != TargetPosition)
            return;

        if (_card.isFlipped != Flip)
        {
            startTranform = transform.rotation;
            endTransform = Quaternion.AngleAxis(180, Vector3.up);
        }

        Callback.Invoke();
        Destroy(this);
    }

    void Update()
    {
        var dt = Time.deltaTime;
        _timePassed += dt;

        if (_timePassed > Duration)
        {
            transform.position = TargetPosition;
            Callback.Invoke();
            Destroy(this);
        }

        transform.position = Vector3.Lerp(_start, TargetPosition, _timePassed / Duration);
    }
}
