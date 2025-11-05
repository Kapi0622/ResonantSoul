// _ResonantSoul/Scripts/PlayerInput.cs
using System;
using UnityEngine;
using UnityEngine.InputSystem; // InputSystemの名前空間
using VContainer;
using VContainer.Unity;

// R3の代わりに ITickable を実装する
public class PlayerInput : IInitializable, ITickable, IDisposable
{
    private readonly PlayerInputActions _inputActions;

    // --- 公開するプロパティ (他のクラスはこれを見に来る) ---
    public Vector2 MoveDirection { get; private set; }
    public bool IsJumpPressed { get; private set; }
    public bool IsDashPressed { get; private set; }
    public bool IsNormalAttackPressed { get; private set; }
    public bool IsSpecialAttackPressed { get; private set; }
    
    [Inject]
    public PlayerInput(PlayerInputActions inputActions)
    {
        _inputActions = inputActions;
    }

    public void Initialize()
    {
        _inputActions.Player.Enable(); // アクションマップを有効化
    }

    // "Update"のタイミングで呼ばれる
    public void Tick()
    {
        // 毎フレーム、入力の状態を読み取ってプロパティを更新する
        MoveDirection = _inputActions.Player.Move.ReadValue<Vector2>();
        IsJumpPressed = _inputActions.Player.Jump.WasPressedThisFrame();
        IsDashPressed = _inputActions.Player.Dash.WasPressedThisFrame();
        IsNormalAttackPressed = _inputActions.Player.NormalAttack.WasPressedThisFrame();
        IsSpecialAttackPressed = _inputActions.Player.SpecialAttack.WasPressedThisFrame();
    }
    
    public void Dispose()
    {
        _inputActions?.Player.Disable(); // アクションマップを無効化
    }
}