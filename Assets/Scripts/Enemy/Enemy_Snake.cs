
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �ʓG�N���X�FSnake
/// 
/// �A�N�^�[���߂��ɂ���Ɛڋ߂���
/// �U�����Ă��Ȃ����̓�����͂��Ă���
/// </summary>
public class Enemy_Snake : EnemyBase
{
    // �ݒ荀��
    [Header("�ړ����x")]
    public float movingSpeed;
    [Header("�ő�ړ����x")]
    public float maxSpeed;
    [Header("�ړ�����(�A�N�^�[�Ƃ̋��������̒l�ȉ��Ȃ�ړ�)")]
    public float awakeDistance;
    [Header("��ړ���������")]
    public float brakeRatio;

    // �e��ϐ�
    private bool isBreaking;    // �u���[�L�쓮�t���O true�Ō�������

    /// <summary>
    /// ���̃����X�^�[�̋���G���A�ɃA�N�^�[���i���������̋N��������(�G���A�A�N�e�B�u��������)
    /// </summary>
    public override void OnAreaActivated()
    {
        // ���X�̋N�������������s
        base.OnAreaActivated();
    }


    // Update
    void Update()
    {
        // ���Œ��Ȃ珈�����Ȃ�
        if (isVanishing)
            return;

        // �A�N�^�[���߂��ɂ���Ɛڋ߂��鏈��
        float speed = 0.0f; // x�����ړ����x
        Vector2 ePos = transform.position;  // �G�l�~�[���W
        Vector2 aPos = actorTransform.position;   // �A�N�^�[���W

        // �A�N�^�[�Ƃ̋���������Ă���ꍇ�̓u���[�L�t���O�𗧂ďI��(�ړ����Ȃ�)
        if (Vector2.Distance(ePos, aPos) > awakeDistance)
        {
            isBreaking = true;
            return;
        }
        isBreaking = false; // ����ĂȂ��Ȃ�u���[�L�t���Ofalse

        // �A�N�^�[�Ƃ̈ʒu�֌W�������������
        if (ePos.x > aPos.x)
        {// ������
            speed = -movingSpeed;
            SetFacingRight(false);
        }
        else
        {// �E����
            speed = movingSpeed;
            SetFacingRight(true);
        }

        // �ړ�����
        Vector2 vec = rigidbody2D.linearVelocity;   // ���x�x�N�g��
        vec.x += speed * Time.deltaTime;
        // x�����̑��x�̍ő�l��ݒ�
        if (vec.x > 0.0f) // �E����
            vec.x = Mathf.Clamp(vec.x, 0.0f, maxSpeed);
        else // ������
            vec.x = Mathf.Clamp(vec.x, -maxSpeed, 0.0f);
        // ���x�x�N�g�����Z�b�g
        rigidbody2D.linearVelocity = vec;
    }

    // FixedUpdate
    void FixedUpdate()
    {
        // �A�N�^�[���߂��ɋ��Ȃ����̃u���[�L����
        if (isBreaking)
        {
            Vector2 vec = rigidbody2D.linearVelocity;   // �G�l�~�[���x
            vec.x *= brakeRatio; // x�����̂݌���
            rigidbody2D.linearVelocity = vec;
        }
    }
}
