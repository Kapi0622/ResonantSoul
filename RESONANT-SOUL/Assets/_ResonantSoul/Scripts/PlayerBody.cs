// _ResonantSoul/Scripts/PlayerBody.cs
using UnityEngine;

// Rigidbody2D と Animator への参照を「手動で」保持するための「目印」クラス
public class PlayerBody : MonoBehaviour
{
    // Inspectorから手動で設定するスロット
    [SerializeField]
    private Rigidbody2D _rigidbody2D;

    [SerializeField]
    private Animator _animator;

    // DIコンテナが、このプロパティを通じて参照を取得できるようにする
    public Rigidbody2D Rigidbody2D => _rigidbody2D;
    public Animator Animator => _animator;
    
    // SpriteRenderer のスロットとプロパティは削除します
}