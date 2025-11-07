// _ResonantSoul/Scripts/PlayerCombat.cs
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class PlayerCombat : ITickable
{
    // --- DIコンテナから注入されるコンポーネント ---
    private readonly PlayerInput _input;
    private readonly VesselState _playerState;
    private readonly IObjectResolver _resolver;
    private readonly GameObject _hitboxPrefab;
    
    private readonly Animator _animator; 

    // --- 内部状態 ---
    private bool _attackInputBuffer = false;

    // コンストラクタ: Animator を追加
    [Inject]
    public PlayerCombat(
        PlayerInput input,
        VesselState playerState,
        IObjectResolver resolver,
        GameObject hitboxPrefab,
        Animator animator 
    )
    {
        _input = input;
        _playerState = playerState;
        _resolver = resolver;
        _hitboxPrefab = hitboxPrefab;
        _animator = animator;
    }

    public void Tick()
    {
        if (_input.IsNormalAttackPressed)
        {
            _attackInputBuffer = true;
        }

        PerformNormalAttack();
    }

    private void PerformNormalAttack()
    {
        if (_attackInputBuffer)
        {
            _attackInputBuffer = false;
            
           
            
            Debug.Log("Normal Attack Triggered!");

            // 1. Animatorで攻撃アニメーションを再生
            _animator.SetTrigger("Attack");

            // 2. 攻撃判定（Hitbox）の生成は、Animator側（アニメーションイベント）で行うのが
            //    本来の形です。
            //    SPUMがアニメーションイベントに対応しているか後で確認しましょう。
            //    ひとまず、プレハブ生成の古いコードはコメントアウトします。
            
            /*
            var offset = new Vector2(_playerState.FacingDirection * 0.7f, 0f);
            var spawnPosition = _playerState.Position + offset;
            _resolver.Instantiate(_hitboxPrefab, spawnPosition, Quaternion.identity);
            */
        }
    }
}