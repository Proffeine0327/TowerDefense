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
    [SerializeField] private float shotDelay;
    [SerializeField] private float attackDelay;

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
        if(isAttacking) return;
        if(nav == null) return;
        if(cloestTower == null) return;

        nav.SetDestination(cloestTower.transform.position);
    }

    private void Attack()
    {
        if(isAttacking) return;
        if(cloestTower == null) return;

        if(Vector3.Distance(cloestTower.transform.position, transform.position) < nav.stoppingDistance + 1)
        {
            switch (data.type)
            {
                case Define.UnitType.None:
                case Define.UnitType.Recon:
                case Define.UnitType.Slime:
                case Define.UnitType.Butterfly:
                case Define.UnitType.SlimeKing:
                case Define.UnitType.SlimeKnight:
                case Define.UnitType.SlimeSpeed:
                case Define.UnitType.Bear:
                case Define.UnitType.BearWalker:
                    isAttacking = true;
                    StartCoroutine(Hit());
                    break;
            }
        }
    }

    private IEnumerator Hit()
    {
        cloestTower.Damage(data.damage);
        yield return new WaitForSeconds(attackDelay);
        isAttacking = false;
    }

    private IEnumerator Shoot()
    {   
        yield return new WaitForSeconds(shotDelay);

        isAttacking = false;
    }

    private void Detected()
    {
        if (data.type == Define.UnitType.Butterfly)
        {
            cloestTower = DetectTower(item => item is Castle);
        }
        else
        {
            //cloestTower = DetectTower(item => item is Castle);
            cloestTower = DetectTower(item => item is Vision);
        }
    }

    private TowerBase DetectTower(System.Func<TowerBase, bool> criteria)
    {
        var detections =
                TowerBase.towers
                .OrderBy(item => Vector3.Distance(item.transform.position, transform.position))
                .ToArray();

        var c_detections =
                detections
                .Where(item => criteria.Invoke(item))
                .ToArray();

        Debug.Log(c_detections.Length);
        Debug.Log(detections.Length);

        if (c_detections.Length > 0) return c_detections[0];
        if (detections.Length > 0) return detections[0];

        return null;
    }

    public override void Damage(int damageCount)
    {
        curHp -= damageCount;
    }
}
