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
    // R3を使わないので、シンプルなC#のプロパティにする
    public Vector2 MoveDirection { get; private set; }
    public bool IsJumpPressed { get; private set; }
    public bool IsDashPressed { get; private set; }
    public bool IsNormalAttackPressed { get; private set; }
    public bool IsSpecialAttackPressed { get; private set; }

    // コンストラクタ: DIコンテナが PlayerInputActions を自動で注入
    [Inject]
    public PlayerInput(PlayerInputActions inputActions)
    {
        _inputActions = inputActions;
    }

    // VContainerがシーン開始時に1回だけ呼ぶ
    public void Initialize()
    {
        _inputActions.Player.Enable(); // アクションマップを有効化
    }

    // VContainerが毎フレームのUpdateのタイミングで呼ぶ
    public void Tick()
    {
        // 毎フレーム、入力の状態を読み取ってプロパティを更新する

        // Move (Vector2)
        MoveDirection = _inputActions.Player.Move.ReadValue<Vector2>();

        // Jump (Button)
        // WasPressedThisFrame() は「このフレームで押された瞬間」をtrueで返す
        IsJumpPressed = _inputActions.Player.Jump.WasPressedThisFrame();

        // Dash (Button)
        IsDashPressed = _inputActions.Player.Dash.WasPressedThisFrame();

        // NormalAttack (Button)
        IsNormalAttackPressed = _inputActions.Player.NormalAttack.WasPressedThisFrame();

        // SpecialAttack (Button)
        IsSpecialAttackPressed = _inputActions.Player.SpecialAttack.WasPressedThisFrame();
    }

    // VContainerがシーン終了時に1回だけ呼ぶ
    public void Dispose()
    {
        _inputActions?.Player.Disable(); // アクションマップを無効化
    }
}