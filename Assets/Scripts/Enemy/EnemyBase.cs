using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// �S�G�l�~�[���ʏ����N���X
/// </summary>
public class EnemyBase : MonoBehaviour
{
    // �I�u�W�F�N�g�E�R���|�[�l���g
    [HideInInspector] public AreaManager areaManager; // �G���A�}�l�[�W��
    protected Rigidbody2D rigidbody2D; // RigidBody2D
    protected SpriteRenderer spriteRenderer;// �G�X�v���C�g
    protected Transform actorTransform; // ��l��(�A�N�^�[)��Transform

    // �摜�f��
    public Sprite sprite_Defeat; // �팂�j���X�v���C�g(�����)

    // �e��ϐ�
    // ��b�f�[�^(�C���X�y�N�^�������)
    [Header("�ő�̗�(�����̗�)")]
    public int maxHP;
    [Header("�ڐG���A�N�^�[�ւ̃_���[�W")]
    public int touchDamage;
    [Header("�{�X�G�t���O(ON�Ń{�X�G�Ƃ��Ĉ����B�P�X�e�[�W�ɂP�̂̂�)")]
    public bool isBoss;
    // ���̑��f�[�^
    [HideInInspector] public int nowHP; // �c��HP
    [HideInInspector] public bool isVanishing; // ���Œ��t���O true�ŏ��Œ��ł���
    [HideInInspector] public bool isInvis; // ���G���[�h
    [HideInInspector] public bool rightFacing; // �E�����t���O(false�ō�����)

    // DoTween�p
    private Tween damageTween;  // ��_���[�W�����oTween

    // �萔��`
    private readonly Color COL_DEFAULT = new Color(1.0f, 1.0f, 1.0f, 1.0f);    // �ʏ펞�J���[
    private readonly Color COL_DAMAGED = new Color(1.0f, 0.1f, 0.1f, 1.0f);    // ��_���[�W���J���[
    private const float KNOCKBACK_X = 1.8f; // ��_���[�W���m�b�N�o�b�N��(x����)
    private const float KNOCKBACK_Y = 0.3f; // ��_���[�W���m�b�N�o�b�N��(y����)

    // �������֐�(AreaManager.cs����ďo)
    public void Init(AreaManager _areaManager)
    {
        // �Q�Ǝ擾
        areaManager = _areaManager;
        actorTransform = areaManager.stageManager.actorController.transform;
        rigidbody2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // �ϐ�������
        rigidbody2D.freezeRotation = true;
        nowHP = maxHP;
        if (transform.localScale.x > 0.0f)
            rightFacing = true;

        // �G���A���A�N�e�B�u�ɂȂ�܂ŉ������������ҋ@
        gameObject.SetActive(false);
    }
    /// <summary>
    /// ���̃����X�^�[�̋���G���A�ɃA�N�^�[���i���������̏���(�G���A�A�N�e�B�u��������)
    /// </summary>
    public virtual void OnAreaActivated()
    {
        // ���̃����X�^�[���A�N�e�B�u��
        gameObject.SetActive(true);
    }

    /// <summary>
    /// �_���[�W���󂯂�ۂɌĂяo�����
    /// </summary>
    /// <param name="damage">�_���[�W��</param>
    /// <returns>�_���[�W�����t���O true�Ő���</returns>
    public bool Damaged(int damage)
    {
        // �_���[�W����
        nowHP -= damage;

        if (nowHP <= 0)
        {// HP0�̏ꍇ
         // ��_���[�WTween������
            if (damageTween != null)
                damageTween.Kill();
            damageTween = null;

            // ���Œ��t���O���Z�b�g
            isVanishing = true;
            // ���Œ��͕������Z�Ȃ�
            rigidbody2D.linearVelocity = Vector2.zero;
            rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
            // �_�Ō�ɏ��ŏ������Ăяo��(DoTween�g�p)
            spriteRenderer.DOFade(0.0f, 0.15f) // 0.15�b*���[�v�񐔕��̍Đ�����
                .SetEase(Ease.Linear)          // �ω��̎d�����w��
                .SetLoops(7, LoopType.Yoyo)    // 7�񃋁[�v�Đ�(������͋t�Đ�)
                .OnComplete(Vanish);   // �Đ����I�������Vanish()���Ăяo���ݒ�

            // �팂�j���X�v���C�g������Ε\��
            if (sprite_Defeat != null)
                spriteRenderer.sprite = sprite_Defeat;

            // ���̑����j������
            if (isBoss)
            {// �{�X���j��
            }
            else
            {// �U�R���j��
            }
        }
        else
        {// �܂�HP���c���Ă���ꍇ
         // ��_���[�WTween������
            if (damageTween != null)
                damageTween.Kill();
            damageTween = null;
            // ��_���[�W���o�Đ�
            // (��u�����X�v���C�g��ԐF�ɕύX����)
            if (!isInvis)
            {
                spriteRenderer.color = COL_DAMAGED; // �ԐF�ɕύX
                damageTween = spriteRenderer.DOColor(COL_DEFAULT, 1.0f);   // DoTween�ŏ��X�ɖ߂�
            }
        }

        return true;
    }
    /// <summary>
    /// �G�l�~�[�����ł���ۂɌĂяo�����
    /// </summary>
    private void Vanish()
    {
        // �I�u�W�F�N�g����
        Destroy(gameObject);
    }

    /// <summary>
    /// �A�N�^�[�ɐڐG�_���[�W��^���鏈��
    /// </summary>
    public void BodyAttack(GameObject actorObj)
    {
        // ���g�����Œ��Ȃ疳��
        if (isVanishing)
            return;

        // �A�N�^�[�̃R���|�[�l���g���擾
        ActorController actorCtrl = actorObj.GetComponent<ActorController>();
        if (actorCtrl == null)
            return;

        // �A�N�^�[�ɐڐG�_���[�W��^����
        actorCtrl.Damaged(touchDamage);
    }

    /// <summary>
    /// �I�u�W�F�N�g�̌��������E�Ō��肷��
    /// </summary>
    /// <param name="isRight">�E�����t���O</param>
    public void SetFacingRight(bool isRight)
    {
        if (!isRight)
        {// ������
         // �X�v���C�g��ʏ�̌����ŕ\��
            spriteRenderer.flipX = false;
            // �E�����t���Ooff
            rightFacing = false;
        }
        else
        {// �E����
         // �X�v���C�g�����E���]���������ŕ\��
            spriteRenderer.flipX = true;
            // �E�����t���Oon
            rightFacing = true;
        }
    }
}