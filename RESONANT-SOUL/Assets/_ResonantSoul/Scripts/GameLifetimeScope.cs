// _ResonantSoul/Scripts/GameLifetimeScope.cs
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class GameLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        // --- 1. 入力アセット (PlayerInputActions) の登録 ---
        var inputActions = new PlayerInputActions();
        builder.RegisterInstance(inputActions);

        // --- 2. プレイヤーの物理実体 (Rigidbody2D) の登録 ---
        var playerBody = FindObjectOfType<PlayerBody>(); // シーンからPlayerBodyスクリプトを持つオブジェクトを探す
        
        // 🔽🔽 --- ここからが修正点 --- 🔽🔽
        if (playerBody != null)
        {
            // PlayerBodyのAwake()を待たずに、ここで直接GetComponentする
            var playerRb = playerBody.GetComponent<Rigidbody2D>();
            builder.RegisterInstance(playerRb); // Rigidbody2D自体を登録
        }
        else
        {
            // もしPlayerBodyが見つからなかった場合のエラー
            Debug.LogError("GameLifetimeScope: PlayerBody がシーン内に見つかりません！");
        }
        // 🔼🔼 --- ここまでが修正点 --- 🔼🔼


        // --- 3. 入力処理クラス (PlayerInput) の登録 ---
        builder.Register<PlayerInput>(Lifetime.Singleton)
            .AsImplementedInterfaces()
            .AsSelf();

        // --- 4. 移動処理クラス (PlayerMovement) の登録 ---
        builder.Register<PlayerMovement>(Lifetime.Singleton)
            .AsImplementedInterfaces()
            .AsSelf();
    }
}