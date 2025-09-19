using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// アクター操作・制御クラス
/// </summary>
public class ActorController : MonoBehaviour
{
    // オブジェクト・コンポーネント参照
    private Rigidbody2D rigidbody2D;
    private SpriteRenderer spriteRenderer;
    private ActorGroundSensor groundSensor; // アクター接地判定クラス
    private ActorSprite actorSprite; // アクタースプライト設定クラス
    public CameraController cameraController; // カメラ制御クラス

    // 移動関連変数
    [HideInInspector] public float xSpeed; // X方向移動速度
    [HideInInspector] public bool rightFacing; // 向いている方向(true.右向き false:左向き)
    private float remainJumpTime;   // 空中でのジャンプ入力残り受付時間

    // Start（オブジェクト有効化時に1度実行）
    void Start()
    {
        // コンポーネント参照取得
        rigidbody2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        groundSensor = GetComponentInChildren<ActorGroundSensor>();
        actorSprite = GetComponent<ActorSprite>();

        // 配下コンポーネント初期化
        actorSprite.Init(this);

        // カメラ初期位置
        cameraController.SetPosition(transform.position);

        // 変数初期化
        rightFacing = true; // 最初は右向き
    }

    // Update（1フレームごとに1度ずつ実行）
    void Update()
    {
        // 左右移動処理
        MoveUpdate();
        // ジャンプ入力処理
        JumpUpdate();

        // 坂道で滑らなくする処理
        rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation; // Rigidbodyの機能のうち回転だけは常に停止
        if (groundSensor.isGround && !Input.GetKey(KeyCode.UpArrow))
        {
            // 坂道を登っている時上昇力が働かないようにする処理
            if (rigidbody2D.linearVelocity.y > 0.0f)
            {
                rigidbody2D.linearVelocity = new Vector2(rigidbody2D.linearVelocity.x, 0.0f);
            }
            // 坂道に立っている時滑り落ちないようにする処理
            if (Mathf.Abs(xSpeed) < 0.1f)
            {
                // Rigidbodyの機能のうち移動と回転を停止
                rigidbody2D.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
            }
        }

        // カメラに自身の座標を渡す
        cameraController.SetPosition(transform.position);
    }

    /// <summary>
    /// Updateから呼び出される左右移動入力処理
    /// </summary>
    private void MoveUpdate()
    {
        // X方向移動入力
        if (Input.GetKey(KeyCode.RightArrow))
        {// 右方向の移動入力
         // X方向移動速度をプラスに設定
            xSpeed = 6.0f;

            // 右向きフラグon
            rightFacing = true;

            // スプライトを通常の向きで表示
            spriteRenderer.flipX = false;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {// 左方向の移動入力
         // X方向移動速度をマイナスに設定
            xSpeed = -6.0f;

            // 右向きフラグoff
            rightFacing = false;

            // スプライトを左右反転した向きで表示
            spriteRenderer.flipX = true;
        }
        else
        {// 入力なし
         // X方向の移動を停止
            xSpeed = 0.0f;
        }
    }

    /// <summary>
    /// Updateから呼び出されるジャンプ入力処理
    /// </summary>
    private void JumpUpdate()
    {
        // 空中でのジャンプ入力受付時間減少
        if (remainJumpTime > 0.0f)
            remainJumpTime -= Time.deltaTime;

        // ジャンプ操作
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {// ジャンプ開始
         // 接地していないなら終了
            if (!groundSensor.isGround)
                return;

            // ジャンプ力を計算
            float jumpPower = 10.0f;
            // ジャンプ力を適用
            rigidbody2D.linearVelocity = new Vector2(rigidbody2D.linearVelocity.x, jumpPower);

            // 空中でのジャンプ入力受け付け時間設定
            remainJumpTime = 0.25f;
        }
        else if (Input.GetKey(KeyCode.UpArrow))
        {// ジャンプ中（ジャンプ入力を長押しすると継続して上昇する処理）
         // 空中でのジャンプ入力受け付け時間が残ってないなら終了
            if (remainJumpTime <= 0.0f)
                return;
            // 接地しているなら終了
            if (groundSensor.isGround)
                return;

            // ジャンプ力加算量を計算
            float jumpAddPower = 30.0f * Time.deltaTime; // Update()は呼び出し間隔が異なるのでTime.deltaTimeが必要
                                                         // ジャンプ力加算を適用
            rigidbody2D.linearVelocity += new Vector2(0.0f, jumpAddPower);
        }
        else if (Input.GetKeyUp(KeyCode.UpArrow))
        {// ジャンプ入力終了
            remainJumpTime = -1.0f;
        }
    }

    // FixedUpdate（一定時間ごとに1度ずつ実行・物理演算用）
    private void FixedUpdate()
    {
        // 移動速度ベクトルを現在値から取得
        Vector2 velocity = rigidbody2D.linearVelocity;
        // X方向の速度を入力から決定
        velocity.x = xSpeed;

        // 計算した移動速度ベクトルをRigidbody2Dに反映
        rigidbody2D.linearVelocity = velocity;
    }
}