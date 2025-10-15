using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �A�N�^�[�˕��e�N���X
/// </summary>

public class ActorWindblowShot : ActorNormalShot
{
    /// <summary>
    /// (�p�����Ďg�p)���̒e���G�Ƀ_���[�W��^�������̒ǉ�����
    /// </summary>
    protected override void OnDamagedEnemy(EnemyBase enemyBase)
    {
        Vector2 blowVector = new Vector2(10.0f, 7.0f);
        // �e�����ɐi��ł���Ȃ�x�N�g�������E���]
        if (angle > 90)
            blowVector.x *= -1.0f;

        // �G�𐁂���΂�(�΃U�R�̂�)
        if (!enemyBase.isBoss)
            enemyBase.GetComponent<Rigidbody2D>().linearVelocity += blowVector;
    }
}
