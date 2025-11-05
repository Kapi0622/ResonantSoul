// _ResonantSoul/Scripts/Hitbox.cs
using UnityEngine;
using Cysharp.Threading.Tasks;
using System;

public class Hitbox : MonoBehaviour
{
    // ヒットボックスが消えるまでの時間（秒）
    private readonly float _lifeTime = 0.2f;

    async void Start()
    {
        // UniTaskを使って、指定時間後にオブジェクトを破棄する
        try
        {
            // _lifeTime秒 待機する
            await UniTask.Delay(TimeSpan.FromSeconds(_lifeTime), cancellationToken: this.GetCancellationTokenOnDestroy());

            // 待機後、自分自身（このHitboxオブジェクト）を破棄する
            Destroy(gameObject);
        }
        catch (OperationCanceledException)
        {
            // オブジェクトがDelay中に（例:シーン切り替えなどで）破棄された場合は何もしない
        }
    }

    // 敵との当たり判定（次のステップで実装）
    private void OnTriggerEnter2D(Collider2D other)
    {
        // TODO: もし "Enemy" タグを持つオブジェクトに当たったら...
        Debug.Log($"Hitboxが {other.name} にヒット！");
    }
}