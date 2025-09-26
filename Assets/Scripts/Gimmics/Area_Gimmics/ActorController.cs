using UnityEngine;

public class ActorController : MonoBehaviour
{
    // ダメージ処理のメソッドを定義する
    // Gimmic_DamageBlock.csではこのメソッドが呼ばれています
    public void Damaged(int damageAmount)
    {
        Debug.Log(gameObject.name + "が " + damageAmount + " のダメージを受けました！");

        // --- ここに実際のヘルス減少処理などを実装 ---
        // 例: health -= damageAmount;
        // ------------------------------------------
    }

    // 必要に応じて、他のアクター制御に関するメソッドや変数を追加してください
}