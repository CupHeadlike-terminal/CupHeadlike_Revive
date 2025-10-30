using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// ステージセレクト画面管理クラス
/// </summary>
public class StageSelectManager : MonoBehaviour
{
    // ステージ選択ボタンリスト
    [SerializeField] private List<Image> stageSelectButtonImages = null;
    // タイトルImageオブジェクト
    [SerializeField] private GameObject titlePictureObject = null;

    // Start
    void Start()
    {
        // タイトル画面タップ済みフラグが有効ならタイトル画面を表示しない
        if (Data.instance.isTitleDisplayed)
            titlePictureObject.SetActive(false);
        // ステージセレクトボタンの色変更
        for (int i = 0; i < stageSelectButtonImages.Count; i++)
        {
            if (Data.instance.stageClearedFlags[i])
                stageSelectButtonImages[i].color = Color.gray;
        }

        // ラスボスステージ選択ボタン有効化・無効化
        bool isReady = true;
        for (int i = 0; i < Data.StageNum_Normal; i++)
        {
            if (!Data.instance.stageClearedFlags[i])
            {
                isReady = false;
                break;
            }
        }
        stageSelectButtonImages[Data.StageNum_All - 1].gameObject.SetActive(isReady);
    }

    /// <summary>
    /// ステージ選択ボタン押下時処理
    /// </summary>
    /// <param name="bossID">該当ボスID</param>
    public void OnStageSelectButtonPressed(int bossID)
    {
        // ステージID記憶
        Data.instance.nowStageID = bossID;
        // シーン切り替え
        SceneManager.LoadScene(bossID + 1);
    }

    /// <summary>
    /// タイトル画像オブジェクトタップ時に呼び出し
    /// </summary>
    public void OnTitlePictureTapped()
    {
        // タイトル画像オブジェクトを無効化
        titlePictureObject.SetActive(false);
        // タイトル画面タップ済みフラグをセット
        Data.instance.isTitleDisplayed = true;
    }
}