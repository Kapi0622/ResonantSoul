// _ResonantSoul/Scripts/GameLifetimeScope.cs
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class GameLifetimeScope : LifetimeScope
{
    [SerializeField]
    private GameObject _attackHitboxPrefab;

    protected override void Configure(IContainerBuilder builder)
    {
        // --- 1. 入力アセット (PlayerInputActions) の登録 ---
        var inputActions = new PlayerInputActions();
        builder.RegisterInstance(inputActions);

        // --- 2. プレイヤーのコンポーネント登録 ---
        var playerBody = FindObjectOfType<PlayerBody>();
        if (playerBody != null)
        {
            var playerRb = playerBody.Rigidbody2D;
            var playerAnimator = playerBody.Animator;
            
            if (playerRb != null && playerAnimator != null) 
            {
                builder.RegisterInstance(playerRb);
                builder.RegisterInstance(playerAnimator);
            }
            else
            {
                Debug.LogError("GameLifetimeScope: PlayerBodyのInspectorで Rigidbody 2D または Animator が手動設定されていません！", playerBody);
            }
        }
        else
        {
            Debug.LogError("GameLifetimeScope: PlayerBody がシーン内に見つかりません！");
        }
        
        // --- 3. プレイヤーの状態 (VesselState) の登録 ---
        builder.Register<VesselState>(Lifetime.Singleton).AsSelf();

        // --- 4. 入力処理クラス (PlayerInput) の登録 ---
        builder.Register<PlayerInput>(Lifetime.Singleton)
               .AsImplementedInterfaces()
               .AsSelf();

        // --- 5. 移動処理クラス (PlayerMovement) の登録 ---
        builder.Register<PlayerMovement>(Lifetime.Singleton)
               .AsImplementedInterfaces()
               .AsSelf();
                   
        // --- 6. 攻撃判定プレハブの登録 ---
        builder.RegisterInstance(_attackHitboxPrefab);

        // --- 7. 戦闘処理クラス (PlayerCombat) の登録 ---
        builder.Register<PlayerCombat>(Lifetime.Singleton)
               .AsImplementedInterfaces()
               .AsSelf();
    }
}