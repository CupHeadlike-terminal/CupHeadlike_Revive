using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// �ʓG�N���X(�{�X)�FLion
/// 
/// ���s�E��]�^�b�N���E�{�胂�[�h
/// </summary>
public class Boss_Lion : EnemyBase
{
    // �摜�f��
    [Header("�摜�f��")]
    [SerializeField] private Sprite[] spriteList_Move = null; // �ړ��A�j���[�V����
    [SerializeField] private Sprite[] spriteList_MoveAnger = null; // �ړ�(�{)�A�j���[�V����
    [SerializeField] private Sprite[] spriteList_Roll = null; // ��]�A�j���[�V����
    [SerializeField] private Sprite sprite_Anger = null; // �{��

    // �ݒ荀��
    [Header("�ړ����x")]
    public float movingSpeed;
    [Header("�ړ����x(�{)")]
    public float movingSpeed_Anger;
    [Header("�ړ�����")]
    public float movingTime;
    [Header("�{�胂�[�V��������")]
    public float angerTime;
    [Header("�{��ڍsHP���C��")]
    public float angerHP;
    [Header("��]�U�����x")]
    public Vector2 rollSpeed;
    [Header("��]�U�����x(�{)")]
    public Vector2 rollSpeed_Anger;
    [Header("��]�U������")]
    public float rollTime;

    // �e��ϐ�
    private float timeCount; // �e���[�h�ł̌o�ߎ���
    private bool isAnger; // �{�胂�[�h

    // �s���p�^�[��
    private ActionMode nowMode;
    private enum ActionMode
    {
        Moving, // ���s
        Anger, // �{�胂�[�h��
        Roll, // ��]�^�b�N��
        _MAX,
    }

    /// <summary>
    /// ���̃����X�^�[�̋���G���A�ɃA�N�^�[���i���������̏���(�G���A�A�N�e�B�u��������)
    /// </summary>
    public override void OnAreaActivated()
    {
        base.OnAreaActivated();

        // �ϐ�������
        nowMode = ActionMode.Moving;
        timeCount = 0.0f;
        SetFacingRight(false);
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
    }

    // Update
    void Update()
    {
        // ���Œ��Ȃ珈�����Ȃ�
        if (isVanishing)
        {
            rigidbody2D.linearVelocity = Vector2.zero;
            transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
            return;
        }

        // �{�胂�[�h�ڍs����
        if (!isAnger && nowHP <= angerHP)
        {
            isAnger = true;
            nowMode = ActionMode.Anger;
            timeCount = 0.0f;
            rigidbody2D.linearVelocity = Vector2.zero;

            // �A�N�^�[�Ƃ̈ʒu�֌W�������������
            if (transform.position.x > actorTransform.position.x)
            {// ������
                SetFacingRight(false);
            }
            else
            {// �E����
                SetFacingRight(true);
            }
        }

        // ���Ԍo��
        timeCount += Time.deltaTime;

        // �e�p�^�[���ʂ̍s��
        int animationFrame;
        switch (nowMode)
        {
            case ActionMode.Moving: // �ړ���
                                    // ���ړ�
                float xSpeed = movingSpeed;
                if (isAnger)
                    xSpeed = movingSpeed_Anger;
                if (!rightFacing)
                    xSpeed *= -1.0f;
                rigidbody2D.linearVelocity = new Vector2(xSpeed, rigidbody2D.linearVelocity.y);

                // �X�v���C�g�K�p
                animationFrame = (int)(timeCount * 2.0f);
                animationFrame %= spriteList_Move.Length;
                if (animationFrame < 0)
                    animationFrame = 0;
                if (!isAnger)
                    spriteRenderer.sprite = spriteList_Move[animationFrame];
                else
                    spriteRenderer.sprite = spriteList_MoveAnger[animationFrame];
                // �����[�h�ؑ�
                if (timeCount >= movingTime)
                {
                    timeCount = 0.0f;
                    nowMode = ActionMode.Roll;

                    // ��]�U���J�n
                    Vector2 startVelocity = rollSpeed;
                    if (isAnger)
                        startVelocity = rollSpeed_Anger;
                    if (!rightFacing)
                        startVelocity.x *= -1.0f;
                    rigidbody2D.linearVelocity = startVelocity;
                }
                break;

            case ActionMode.Roll: // ��]��
                                  // �X�v���C�g�K�p
                animationFrame = (int)(timeCount * 6.0f);
                animationFrame %= spriteList_Roll.Length;
                if (animationFrame < 0)
                    animationFrame = 0;
                spriteRenderer.sprite = spriteList_Roll[animationFrame];
                // �����[�h�ؑ�
                if (timeCount >= rollTime)
                {
                    timeCount = 0.0f;
                    nowMode = ActionMode.Moving;
                }
                break;

            case ActionMode.Anger: // �{�胂�[�V������
                                   // �X�v���C�g�K�p
                spriteRenderer.sprite = sprite_Anger;
                // �����[�h�ؑ�
                if (timeCount >= angerTime)
                {
                    timeCount = 0.0f;
                    nowMode = ActionMode.Moving;
                }
                break;
        }
    }

    // FixedUpdate
    void FixedUpdate()
    {
        // ���Œ��Ȃ珈�����Ȃ�
        if (isVanishing)
        {
            return;
        }

        // ���[�h�ؑ֒���Ȃ�I��
        if (timeCount <= 0.0f)
            return;
        // �{�胂�[�V�������Ȃ�I��
        if (nowMode == ActionMode.Anger)
            return;

        // �ǂɂԂ�����������ύX(��]���Ȃ�ĉ���)
        Vector2 velocity = rigidbody2D.linearVelocity;
        if (rightFacing && CheckRaycastToWall(Vector2.right))
        {// �E�̕ǂɂԂ�����
            SetFacingRight(false);
            // ��]���̓x�N�g�����]
            if (nowMode == ActionMode.Roll)
            {
                if (!isAnger)
                    velocity.x = -rollSpeed.x;
                else
                    velocity.x = -rollSpeed_Anger.x;
                rigidbody2D.linearVelocity = velocity;
            }
        }// ���̕ǂɂԂ�����
        else if (!rightFacing && CheckRaycastToWall(Vector2.left))
        {
            SetFacingRight(true);
            // ��]���̓x�N�g�����]
            if (nowMode == ActionMode.Roll)
            {
                if (!isAnger)
                    velocity.x = rollSpeed.x;
                else
                    velocity.x = rollSpeed_Anger.x;
                rigidbody2D.linearVelocity = velocity;
            }
        }
    }

    /// <summary>
    /// ���g�̂��΂ɕǂ����݂��邩���`�F�b�N����
    /// </summary>
    /// <param name="angle">�m�F�������</param>
    /// <returns>true:�w������ɕǂ����݂���</returns>
    private bool CheckRaycastToWall(Vector2 angle)
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, angle, 1.5f);
        foreach (var hitCollider in hits)
        {
            if (hitCollider.collider.gameObject.tag == "Ground")
                return true;
        }
        return false;
    }
}
