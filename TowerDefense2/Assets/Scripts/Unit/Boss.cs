using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Boss : EnemyBase
{
    public float curAttackDelay;
    public float maxAttackDelay;

    protected override void Update()
    {
        base.Update();

        if (data.type == Define.UnitType.BearWalker)
        {
            if (curHp <= 0)
            {
                Destroy(gameObject);
            }
        }
        Detected();
        if (cloestTower == null) return;

        Move();
        Attack();
    }

    private void Move()
    {
        if (isAttacking || nav == null) return;

        if (!nav.isActiveAndEnabled)
        {
            nav.enabled = true;
        }

        nav.SetDestination(cloestTower.transform.position);
    }

    private void Attack()
    {
        if (nav == null) return;

        if (nav.isActiveAndEnabled && nav.remainingDistance <= nav.stoppingDistance && !nav.pathPending)
        {
            curAttackDelay += Time.deltaTime;
            Hit();
        }
    }

    private void Hit()
    {
        isAttacking = true;
        if (curAttackDelay < maxAttackDelay) return;
        cloestTower.Damage(data.stats[0].damage);
        curAttackDelay = 0f;
        isAttacking = false;
    }

    private void Detected()
    {
        cloestTower = DetectTower();
    }

    public override void Damage(int damageCount)
    {
        curHp -= damageCount;
    }
}
