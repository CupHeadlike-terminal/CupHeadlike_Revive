using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// ステージセレクト画面管理クラス
/// </summary>
public class StageSelectManager : MonoBehaviour
{
    /// <summary>
    /// ステージ選択ボタン押下時処理
    /// </summary>
    /// <param name="bossID">該当ボスID</param>
    public void OnStageSelectButtonPressed(int bossID)
    {
        // シーン切り替え
        SceneManager.LoadScene(bossID + 1);
    }
}