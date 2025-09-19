using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ステージ内の各エリア管理クラス
/// </summary>
public class AreaManager : MonoBehaviour
{
    // オブジェクト・コンポーネント
    [HideInInspector] public StageManager stageManager; // ステージ管理クラス
    private CameraMovingLimitter movingLimitter; // このエリアのカメラ移動範囲

    // 初期化関数(StageManager.csから呼出)
    public void Init(StageManager _stageManager)
    {
        // 参照取得
        stageManager = _stageManager;
        movingLimitter = GetComponentInChildren<CameraMovingLimitter>();

        // アクターが進入するまでこのエリアを無効化
        gameObject.SetActive(false);
    }

    /// <summary>
    /// このエリアをアクティブ化する
    /// </summary>
    public void ActiveArea()
    {
        // 一旦全エリアを非アクティブ化
        stageManager.DeactivateAllAreas();

        // オブジェクト有効化
        gameObject.SetActive(true);

        // カメラ移動範囲を変更
        stageManager.cameraController.ChangeMovingLimitter(movingLimitter);
    }
}
