// _ResonantSoul/Scripts/PlayerMovement.cs
using UnityEngine;
using VContainer;
using VContainer.Unity;

// ITickable (Update) と IFixedTickable (FixedUpdate) の両方を実装します
public class PlayerMovement : ITickable, IFixedTickable
{
    // --- DIコンテナから注入されるコンポーネント ---
    private readonly Rigidbody2D _rb;
    private readonly PlayerInput _input;

    // --- 設定値 ---
    private readonly float _moveSpeed = 10.0f;
    private readonly float _jumpForce = 15.0f; // ジャンプ力

    // --- 内部状態 ---
    private bool _jumpInputBuffer = false; // 👈 ジャンプ入力バッファ

    // コンストラクタ: DIコンテナが Rigidbody2D と PlayerInput を自動で注入
    [Inject]
    public PlayerMovement(Rigidbody2D rb, PlayerInput input)
    {
        _rb = rb;
        _input = input;
    }
    

    // VContainerが毎フレームの "Update" のタイミングで呼ぶ
    public void Tick()
    {
        // --- 入力のキャッチ ---
        // "Update" (Tick) で押された瞬間の入力をキャッチする
        // 物理演算（FixedTick）で消費されるまで true を保持する
        if (_input.IsJumpPressed)
        {
            _jumpInputBuffer = true;
            Debug.Log("Jump Input Buffered!"); // ログをTick側に移動
        }

        // ダッシュや攻撃の入力もここでバッファリングする
        if (_input.IsDashPressed)
        {
            // TODO: ダッシュ入力バッファ
            Debug.Log("Dash Performed! (Input Buffered)");
        }
        if (_input.IsNormalAttackPressed)
        {
            // TODO: 攻撃入力バッファ
            Debug.Log("Normal Attack Performed! (Input Buffered)");
        }
    }

    // VContainerが毎フレームの "FixedUpdate" のタイミングで呼ぶ
    public void FixedTick()
    {
        // --- 物理演算の実行 ---
        Move();
        Jump(); // ジャンプ処理もここ
    }

    private void Move()
    {
        // 移動は PlayerInput の MoveDirection を直接参照してもOK
        // (WasPressedThisFrame と違って、MoveDirection は状態を保持し続けるため)
        _rb.linearVelocity = new Vector2(
            _input.MoveDirection.x * _moveSpeed,
            _rb.linearVelocity.y
        );
    }

    private void Jump()
    {
        // "FixedUpdate" (FixedTick) で入力バッファを消費する
        if (_jumpInputBuffer)
        {
            // バッファを消費
            _jumpInputBuffer = false;

            // TODO: 現在は接地判定がないため、空中でも無限にジャンプできます
            _rb.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
            Debug.Log("Jump Executed in FixedTick!");
        }
    }

}