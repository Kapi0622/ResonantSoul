// _ResonantSoul/Scripts/PlayerCombat.cs
using UnityEngine;
using VContainer;
using VContainer.Unity;

// ITickable を実装し、Updateタイミングで入力を監視する
public class PlayerCombat : ITickable
{
    // --- DIコンテナから注入されるコンポーネント ---
    private readonly PlayerInput _input;
    private readonly VesselState _playerState;
    private readonly IObjectResolver _resolver;
    private readonly GameObject _hitboxPrefab;

    // --- 内部状態 ---
    private bool _attackInputBuffer = false; // 攻撃入力バッファ

    // コンストラクタ: 必要なものをすべて注入
    [Inject]
    public PlayerCombat(
        PlayerInput input,
        VesselState playerState,
        IObjectResolver resolver,
        GameObject hitboxPrefab
    )
    {
        _input = input;
        _playerState = playerState;
        _resolver = resolver;
        _hitboxPrefab = hitboxPrefab;
    }

    // "Update" のタイミングで呼ばれる
    public void Tick()
    {
        // 入力バッファリング
        if (_input.IsNormalAttackPressed)
        {
            _attackInputBuffer = true;
        }

        // （Tick()の最後、またはFixedTick()でバッファを消費する）
        // 今回は攻撃に物理演算が絡まないため、Tick()内で即時実行してOK
        PerformNormalAttack();
    }

    private void PerformNormalAttack()
    {
        // 攻撃入力バッファがあるか確認
        if (_attackInputBuffer)
        {
            // バッファを消費
            _attackInputBuffer = false;

            Debug.Log("Normal Attack Executed!");

            // --- 攻撃判定を生成 ---

            // 1. プレイヤーの向き（FacingDirection）に応じて、オフセット（生成位置）を決める
            // (プレイヤーの右、または左の 0.7 ユニット先に生成)
            var offset = new Vector2(_playerState.FacingDirection * 0.7f, 0f);

            // 2. プレイヤーの現在位置 + オフセット の場所にプレハブを生成
            var spawnPosition = _playerState.Position + offset;

            // 3. DIコンテナの IObjectResolver を使ってプレハブを生成
            _resolver.Instantiate(_hitboxPrefab, spawnPosition, Quaternion.identity);
        }
    }
}