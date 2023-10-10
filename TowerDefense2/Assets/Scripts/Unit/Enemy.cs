using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : EnemyBase
{
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform firePoint;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Update()
    {
        base.Update();

        Detected();
        Move();
        Attack();
    }

    private void Move()
    {
        if (isAttacking) return;
        if (nav == null) return;
        if (cloestTower == null) return;

        if (Physics.Linecast(transform.position, cloestTower.transform.position, out var hit, LayerMask.GetMask("Tower")))
        {
            if (ReferenceEquals(cloestTower.transform, hit.transform))
                nav.enabled = Vector3.Distance(transform.position, hit.point) >= data.stats[0].range - 0.1f;
        }

        if (nav.isActiveAndEnabled) nav.SetDestination(cloestTower.transform.position);
    }

    private void Attack()
    {
        if (isAttacking) return;
        if (cloestTower == null) return;

        if (Physics.Linecast(transform.position, cloestTower.transform.position, out var hit, LayerMask.GetMask("Tower")))
        {
            if (ReferenceEquals(cloestTower.transform, hit.transform))
                if (Vector3.Distance(transform.position, hit.point) < data.stats[0].range)
                {
                    isAttacking = true;
                    StartCoroutine(Hit());
                }
        }
    }

    private IEnumerator Hit()
    {
        cloestTower.Damage(data.stats[0].damage);

        if (data.stats[0].range > 1.5f)
        {
            PrefabContainer
                .Instantiate("EnemyBulletLine")
                .GetComponent<BulletLine>()
                .Init(transform.position, cloestTower.transform.position);
        }
        yield return new WaitForSeconds(data.stats[0].attackDelay);
        isAttacking = false;
    }

    private void Detected()
    {
        if (data.type == Define.UnitType.Butterfly)
        {
            cloestTower = DetectTower(
                item => item is Tower,
                item => item is Castle,
                item => item is MainCastle);
        }
        else
        {
            cloestTower = DetectTower(
                item => item is Vision,
                item => item is Tower,
                item => item is Castle,
                item => item is MainCastle);
        }
    }

    public override void Damage(int damageCount)
    {
        curHp -= damageCount;
    }
}
