using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �f�[�^�}�l�[�W���[(�V���O���g��)
/// </summary>
public class Data : MonoBehaviour
{
    #region �V���O���g���ێ��p����(�ύX�s�v)
    // �V���O���g���ێ��p
    public static Data instance { get; private set; }

    // Awake
    private void Awake()
    {
        // �V���O���g���p����
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

        // ���̑��N��������
        InitialProcess();
    }
    #endregion

    // �V�[���Ԃŕێ�����f�[�^�f�[�^
    public bool[] stageClearedFlags; // �X�e�[�W�ʃN���A�t���O
    public int nowStageID; // ���ݍU�����̃X�e�[�WID
    public bool[] weaponUnlocks; // ���ꕐ��̊J���f�[�^

    // �萔��`
    public const int StageNum_Normal = 7; // �ʏ�X�e�[�W�� 
    public const int StageNum_All = 8;  // ���X�{�X�ʂ��܂߂��X�e�[�W��
    public const int LastStageID = 7; // ���X�{�X�ʂ̃X�e�[�W�ԍ�

    /// <summary>
    /// �Q�[���J�n��(�C���X�^���X����������)�Ɉ�x�������s����鏈��
    /// </summary>
    private void InitialProcess()
    {
        // �����V�[�h�l������
        Random.InitState(System.DateTime.Now.Millisecond);

        // �X�e�[�W�N���A�t���O������
        stageClearedFlags = new bool[StageNum_All];

        // ���ꕐ��J���f�[�^������
        //��Œǉ�weaponUnlocks = new bool[(int)ActorController.ActorWeaponType._Max];
        weaponUnlocks[0] = true; // 1�ڂ̏�������͎����J��
    }
}