using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
/// <summary>
/// ステージマネージャクラス
/// </summary>
public class StageManager : MonoBehaviour
{
    [HideInInspector] public ActorController actorController; // アクター制御クラス
    [HideInInspector] public CameraController cameraController; // カメラ制御クラス
    public Image bossHPGage; // ボス用HPゲージImage

    [Header("初期エリアのAreaManager")]
    public AreaManager initArea; // ステージ内の最初のエリア(初期エリア)
    [Header("ボス戦用BGMのAudioClip")]
    public AudioClip bossBGMClip;

    // ステージ内の全エリアの配列(Startで取得)
    private AreaManager[] inStageAreas;

    // 復活ボタンのGameObject
    public GameObject revivalButtonObject;
    public GameObject NewWeaponObject;
    // ゲームオーバー時Tween
    private Tween gameOverTween;
    private bool[] weaponUnlocked;
    // ステージIDに対応する武器ID（-1なら解放なし）
    private int[] stageToWeaponMap = { -1, 1, 2, 3, 4, 5, 6 };

    void Awake()
    {
        weaponUnlocked = new bool[(int)ActorController.ActorWeaponType._Max];

        // 通常武器は最初から解放
        weaponUnlocked[0] = true;

        // Data.instance からシーン間情報を読み込む
        for (int i = 0; i < Data.instance.weaponUnlocks.Length; i++)
        {
            if (Data.instance.weaponUnlocks[i])
                weaponUnlocked[i] = true;
        }
    }

    public bool IsWeaponUnlocked(int weaponID)
    {
        if (weaponID < 0 || weaponID >= weaponUnlocked.Length) return false;
        return weaponUnlocked[weaponID];
    }

    // Start
    void Start()
    {
        // 参照取得
        actorController = GetComponentInChildren<ActorController>();
        cameraController = GetComponentInChildren<CameraController>();

        // ステージ内の全エリアを取得・初期化
        inStageAreas = GetComponentsInChildren<AreaManager>();
        foreach (var targetAreaManager in inStageAreas)
            targetAreaManager.Init(this);

        // 初期エリアをアクティブ化(その他のエリアは全て無効化)
        initArea.ActiveArea();

        // UI初期化
        bossHPGage.transform.parent.gameObject.SetActive(false);

        // 復活ボタン非表示
        revivalButtonObject.SetActive(false);
        NewWeaponObject.SetActive(false);
    }

    /// <summary>
    /// ステージ内の全エリアを無効化する
    /// </summary>
    public void DeactivateAllAreas()
    {
        foreach (var targetAreaManager in inStageAreas)
            targetAreaManager.gameObject.SetActive(false);
    }

    /// <summary>
    /// ボス戦用BGMを再生する
    /// </summary>
    public void PlayBossBGM()
    {
        // BGMを変更する
        GetComponent<AudioSource>().clip = bossBGMClip;
        GetComponent<AudioSource>().Play();
    }

    /// <summary>
    /// ステージクリア時処理
    /// </summary>
    public void StageClear()
    {
        // ステージクリアフラグ記録
        Data.instance.stageClearedFlags[Data.instance.nowStageID] = true;

        // 特殊武器解放（存在する場合のみ）
        int unlockWeaponID = GetWeaponIDForStage(Data.instance.nowStageID);
        if (unlockWeaponID >= 0 && unlockWeaponID < Data.instance.weaponUnlocks.Length)
        {
            if (!Data.instance.weaponUnlocks[unlockWeaponID])
            {
                Data.instance.weaponUnlocks[unlockWeaponID] = true;
                Debug.Log("特殊武器ID " + unlockWeaponID + " を入手しました！");
            }
        }

        // シーン切り替えは遅延呼び出し
        DOVirtual.DelayedCall(5.0f, () => { SceneManager.LoadScene(0); });
    }

    // ステージIDに対応する武器IDを返す関数（存在しない場合 -1 を返す）
    private int GetWeaponIDForStage(int stageID)
    {
        if (stageID < 0 || stageID >= stageToWeaponMap.Length) return -1;
        return stageToWeaponMap[stageID];
    }

    /// <summary>
    /// ゲームオーバー処理
    /// </summary>
    public void GameOver()
    {
        // 復活ボタン表示
        revivalButtonObject.SetActive(true);

        // 指定秒数経過後に処理を実行
        gameOverTween = DOVirtual.DelayedCall(
            30.0f,   // 秒遅延
            () => {
                // シーン切り替え
                SceneManager.LoadScene(0);
            }
        );
    }
    /// <summary>
	/// 「広告を見て復活」ボタン押下時処理
	/// </summary>
	public void OnRevivalButtonPressed()
    {
        // 復活ボタン非表示
        revivalButtonObject.SetActive(false);

        // ゲームオーバー処理を停止
        if (gameOverTween != null)
        {
            gameOverTween.Kill();
            gameOverTween = null;
        }

        // アクターを復活させる
        actorController.RevivalActor();
    }
    public void UnlockWeapon(int weaponID)
    {
        if (weaponID < 0 || weaponID >= weaponUnlocked.Length) return;

        weaponUnlocked[weaponID] = true;
        if (weaponID < Data.instance.weaponUnlocks.Length)
            Data.instance.weaponUnlocks[weaponID] = true;
        if (Data.instance.weaponUnlocks[weaponID])

            NewWeaponObject.SetActive(true);
        gameOverTween = DOVirtual.DelayedCall(
            3.0f,   // 秒遅延
            () => {
                // UI非表示
                NewWeaponObject.SetActive(false);
            }
        );

        Debug.Log("武器ID " + weaponID + " が解放されました");
    }
}