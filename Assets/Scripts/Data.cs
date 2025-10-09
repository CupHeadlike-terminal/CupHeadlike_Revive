using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// データマネージャー(シングルトン)
/// </summary>
public class Data : MonoBehaviour
{
    #region シングルトン維持用処理(変更不要)
    // シングルトン維持用
    public static Data instance { get; private set; }

    // Awake
    private void Awake()
    {
        // シングルトン用処理
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

        // その他起動時処理
        InitialProcess();
    }
    #endregion

    // シーン間で保持するデータデータ
    public bool[] stageClearedFlags; // ステージ別クリアフラグ
    public int nowStageID; // 現在攻略中のステージID
    public bool[] weaponUnlocks; // 特殊武器の開放データ

    // 定数定義
    public const int StageNum_Normal = 7; // 通常ステージ数 
    public const int StageNum_All = 8;  // ラスボス面も含めたステージ数
    public const int LastStageID = 7; // ラスボス面のステージ番号

    /// <summary>
    /// ゲーム開始時(インスタンス生成完了時)に一度だけ実行される処理
    /// </summary>
    private void InitialProcess()
    {
        // 乱数シード値初期化
        Random.InitState(System.DateTime.Now.Millisecond);

        // ステージクリアフラグ初期化
        stageClearedFlags = new bool[StageNum_All];

        // 特殊武器開放データ初期化
        //後で追加weaponUnlocks = new bool[(int)ActorController.ActorWeaponType._Max];
        weaponUnlocks[0] = true; // 1個目の初期武器は自動開放
    }
}