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
    private readonly Animator _animator; 
    
    private readonly Transform _visualRoot;
    

    // --- 設定値 ---
    private readonly float _moveSpeed = 10.0f;
    private readonly float _jumpForce = 15.0f;
    private readonly LayerMask _groundLayerMask;
    private readonly float _groundCheckRaycastDistance = 1.1f;

    // --- 内部状態 ---
    private bool _jumpInputBuffer = false;
    private bool _isGrounded = false;
    private float _currentMoveInputX = 0f; // Move()以外からも参照するため、フィールド変数に変更

    // コンストラクタ: Animator を追加
    [Inject]
    public PlayerMovement(Rigidbody2D rb, PlayerInput input, VesselState playerState, Animator animator) 
    {
        _rb = rb;
        _input = input;
        _playerState = playerState;
        _animator = animator; 
        _visualRoot = animator.transform;

        _groundLayerMask = LayerMask.GetMask("Ground");
    }

    // "Update" のタイミングで呼ばれる
    public void Tick()
    {
        // 入力のキャッチ
        if (_input.IsJumpPressed)
        {
            _jumpInputBuffer = true;
            Debug.Log("Jump Input Buffered!");
        }
        
        
        // 移動入力をフィールドに格納
        _currentMoveInputX = _input.MoveDirection.x;

        // アニメーターのパラメータを更新 (Updateで毎フレーム行う)
        UpdateAnimationParameters();
    }

    // "FixedUpdate" のタイミングで呼ばれる
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
        // X軸の速度を設定
        _rb.linearVelocity = new Vector2(
            _currentMoveInputX * _moveSpeed,
            _rb.linearVelocity.y
        );

        // --- 左右反転ロジック (flipX ではなく localScale を使う) ---
        if (_currentMoveInputX > 0.01f) // 右入力
        {
            _playerState.FacingDirection = -1f;
            _visualRoot.localScale = new Vector3(-1f, 1f, 1f); // 左向き
        }
        else if (_currentMoveInputX < -0.01f) // 右入力
        {
            _playerState.FacingDirection = 1f;
            _visualRoot.localScale = new Vector3(1f, 1f, 1f); 
        }
    }

    private void Jump()
    {
        if (_jumpInputBuffer && _isGrounded)
        {
            _rb.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
            
            _animator.SetTrigger("Jump"); // 👈 ジャンプアニメーションを再生

            Debug.Log("Jump Executed in FixedTick!");
        }
        _jumpInputBuffer = false;
    }
    
    // Animatorに現在の状態を伝えるメソッド
    private void UpdateAnimationParameters()
    {
        // IsRunning パラメータをセット (X軸の移動入力が少しでもあれば true)
        bool isRunning = Mathf.Abs(_currentMoveInputX) > 0.1f;
        _animator.SetBool("IsRunning", isRunning);
        
        // IsGrounded パラメータをセット
        _animator.SetBool("IsGrounded", _isGrounded);
    }
}