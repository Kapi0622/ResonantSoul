// _ResonantSoul/Scripts/VesselState.cs
using UnityEngine;
using VContainer;

// public class PlayerState
public class VesselState
{
    private readonly Rigidbody2D _rb;

    public Vector2 Position => _rb.position;
    public float FacingDirection { get; set; } = 1f;

    [Inject]
    // public PlayerState(Rigidbody2D rb)
    public VesselState(Rigidbody2D rb)
    {
        _rb = rb;
    }
}