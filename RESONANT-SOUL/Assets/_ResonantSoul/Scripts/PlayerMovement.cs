// _ResonantSoul/Scripts/PlayerMovement.cs
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class PlayerMovement : ITickable, IFixedTickable
{
    // --- DIコンテナから注入されるコンポーネント ---
    private readonly Rigidbody2D _rb;
    private readonly PlayerInput _input;
    private readonly VesselState _playerState; 
    private readonly Transform _transform; 

    // --- 設定値 ---
    private readonly float _moveSpeed = 10.0f;
    private readonly float _jumpForce = 15.0f;
    private readonly LayerMask _groundLayerMask;
    private readonly float _groundCheckRaycastDistance = 1.1f;

    // --- 内部状態 ---
    private bool _jumpInputBuffer = false;
    private bool _isGrounded = false;

    // コンストラクタ: VesselState を追加
    [Inject]
    public PlayerMovement(Rigidbody2D rb, PlayerInput input, VesselState vesselState)
    {
        _rb = rb;
        _input = input;
        _playerState = vesselState;
        _transform = rb.transform;

        _groundLayerMask = LayerMask.GetMask("Ground");
    }

    public void Tick()
    {
        if (_input.IsJumpPressed)
        {
            _jumpInputBuffer = true;
            Debug.Log("Jump Input Buffered!");
        }
        
    }

    public void FixedTick()
    {
        CheckGrounded();
        Move(); 
        Jump();
    }

    private void CheckGrounded()
    {
        var hit = Physics2D.Raycast(
            _rb.position,
            Vector2.down,
            _groundCheckRaycastDistance,
            _groundLayerMask
        );
        _isGrounded = hit.collider != null;
        Debug.DrawRay(_rb.position, Vector2.down * _groundCheckRaycastDistance, _isGrounded ? Color.green : Color.red);
    }

    private void Move()
    {
        float moveInputX = _input.MoveDirection.x;

        // X軸の速度を設定
        _rb.linearVelocity = new Vector2(
            moveInputX * _moveSpeed,
            _rb.linearVelocity.y
        );

        // --- 左右反転ロジック ---
        // 入力がある場合のみ向きを変える
        if (moveInputX > 0f)
        {
            _playerState.FacingDirection = 1f;
        }
        else if (moveInputX < 0f)
        {
            _playerState.FacingDirection = -1f;
        }

        // プレイヤーのTransformのlocalScale.x を FacingDirection に合わせる
        _transform.localScale = new Vector3(
            _playerState.FacingDirection, 
            _transform.localScale.y, 
            _transform.localScale.z
        );
    }

    private void Jump()
    {
        if (_jumpInputBuffer && _isGrounded)
        {
            _rb.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
            Debug.Log("Jump Executed in FixedTick!");
        }
        _jumpInputBuffer = false;
    }
}