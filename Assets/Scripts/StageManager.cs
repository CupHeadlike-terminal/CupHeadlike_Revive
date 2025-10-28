using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
/// <summary>
/// �X�e�[�W�}�l�[�W���N���X
/// </summary>
public class StageManager : MonoBehaviour
{
    [HideInInspector] public ActorController actorController; // �A�N�^�[����N���X
    [HideInInspector] public CameraController cameraController; // �J��������N���X
    public Image bossHPGage; // �{�X�pHP�Q�[�WImage

    [Header("�����G���A��AreaManager")]
    public AreaManager initArea; // �X�e�[�W���̍ŏ��̃G���A(�����G���A)
    [Header("�{�X��pBGM��AudioClip")]
    public AudioClip bossBGMClip;

    // �X�e�[�W���̑S�G���A�̔z��(Start�Ŏ擾)
    private AreaManager[] inStageAreas;

    // �����{�^����GameObject
    public GameObject revivalButtonObject;
    public GameObject NewWeaponObject;
    // �Q�[���I�[�o�[��Tween
    private Tween gameOverTween;
    private bool[] weaponUnlocked;
    // �X�e�[�WID�ɑΉ����镐��ID�i-1�Ȃ����Ȃ��j
    private int[] stageToWeaponMap = { -1, 1, 2, 3, 4, 5, 6 };

    void Awake()
    {
        weaponUnlocked = new bool[(int)ActorController.ActorWeaponType._Max];

        // �ʏ핐��͍ŏ�������
        weaponUnlocked[0] = true;

        // Data.instance ����V�[���ԏ���ǂݍ���
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
        // �Q�Ǝ擾
        actorController = GetComponentInChildren<ActorController>();
        cameraController = GetComponentInChildren<CameraController>();

        // �X�e�[�W���̑S�G���A���擾�E������
        inStageAreas = GetComponentsInChildren<AreaManager>();
        foreach (var targetAreaManager in inStageAreas)
            targetAreaManager.Init(this);

        // �����G���A���A�N�e�B�u��(���̑��̃G���A�͑S�Ė�����)
        initArea.ActiveArea();

        // UI������
        bossHPGage.transform.parent.gameObject.SetActive(false);

        // �����{�^����\��
        revivalButtonObject.SetActive(false);
        NewWeaponObject.SetActive(false);
    }

    /// <summary>
    /// �X�e�[�W���̑S�G���A�𖳌�������
    /// </summary>
    public void DeactivateAllAreas()
    {
        foreach (var targetAreaManager in inStageAreas)
            targetAreaManager.gameObject.SetActive(false);
    }

    /// <summary>
    /// �{�X��pBGM���Đ�����
    /// </summary>
    public void PlayBossBGM()
    {
        // BGM��ύX����
        GetComponent<AudioSource>().clip = bossBGMClip;
        GetComponent<AudioSource>().Play();
    }

    /// <summary>
    /// �X�e�[�W�N���A������
    /// </summary>
    public void StageClear()
    {
        // �X�e�[�W�N���A�t���O�L�^
        Data.instance.stageClearedFlags[Data.instance.nowStageID] = true;

        // ���ꕐ�����i���݂���ꍇ�̂݁j
        int unlockWeaponID = GetWeaponIDForStage(Data.instance.nowStageID);
        if (unlockWeaponID >= 0 && unlockWeaponID < Data.instance.weaponUnlocks.Length)
        {
            if (!Data.instance.weaponUnlocks[unlockWeaponID])
            {
                Data.instance.weaponUnlocks[unlockWeaponID] = true;
                Debug.Log("���ꕐ��ID " + unlockWeaponID + " ����肵�܂����I");
            }
        }

        // �V�[���؂�ւ��͒x���Ăяo��
        DOVirtual.DelayedCall(5.0f, () => { SceneManager.LoadScene(0); });
    }

    // �X�e�[�WID�ɑΉ����镐��ID��Ԃ��֐��i���݂��Ȃ��ꍇ -1 ��Ԃ��j
    private int GetWeaponIDForStage(int stageID)
    {
        if (stageID < 0 || stageID >= stageToWeaponMap.Length) return -1;
        return stageToWeaponMap[stageID];
    }

    /// <summary>
    /// �Q�[���I�[�o�[����
    /// </summary>
    public void GameOver()
    {
        // �����{�^���\��
        revivalButtonObject.SetActive(true);

        // �w��b���o�ߌ�ɏ��������s
        gameOverTween = DOVirtual.DelayedCall(
            30.0f,   // �b�x��
            () => {
                // �V�[���؂�ւ�
                SceneManager.LoadScene(0);
            }
        );
    }
    /// <summary>
	/// �u�L�������ĕ����v�{�^������������
	/// </summary>
	public void OnRevivalButtonPressed()
    {
        // �����{�^����\��
        revivalButtonObject.SetActive(false);

        // �Q�[���I�[�o�[�������~
        if (gameOverTween != null)
        {
            gameOverTween.Kill();
            gameOverTween = null;
        }

        // �A�N�^�[�𕜊�������
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
            3.0f,   // �b�x��
            () => {
                // UI��\��
                NewWeaponObject.SetActive(false);
            }
        );

        Debug.Log("����ID " + weaponID + " ���������܂���");
    }
}