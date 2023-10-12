using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public static HashSet<Enemy> enemies = new();

    [SerializeField] protected UnitData data;
    [SerializeField] protected Image hpbar;

    protected TowerBase cloestTower;
    protected NavMeshAgent nav;
    protected bool isAttacking;
    protected int curHp;
    protected string targetTag;

    protected virtual void Awake()
    {
        nav = GetComponent<NavMeshAgent>();
        curHp = data.maxhp;
        nav.speed = data.speed;
        enemies.Add(this);
    }

    protected TowerBase DetectTower(params System.Func<TowerBase, bool>[] priorities)
    {
        var gm = Singleton.Get<GameManager>();

        var detections =
                TowerBase.towers
                .OrderBy(item => Vector3.Distance(item.transform.position, transform.position))
                .ToArray();

        foreach (var p in priorities)
        {
            var c_detections =
                    detections
                    .Where(item => p.Invoke(item))
                    .ToArray();

            if (c_detections.Length > 0) return c_detections[0];
        }

        if (detections.Length > 0)
        {
            if (ReferenceEquals(detections[0].gameObject, gm.MainCastle))
            {
                if (gm.ExistSubCastles)
                {
                    var detectionSubCastle =
                        TowerBase.towers
                        .Where(item => item is Castle)
                        .OrderBy(item => Vector3.Distance(item.transform.position, transform.position))
                        .ToArray();

                    return detectionSubCastle[0];
                }
            }
            return detections[0];
        }
        return null;
    }

    protected virtual void Update()
    {
        if (curHp <= 0)
        {
            enemies.Remove(this);
            DataManager.Instance.Score += data.score;
            Singleton.Get<GameManager>().Money += data.gold * (Singleton.Get<GameManager>().GainAdditiveGoldTime > 0 ? 2 : 1);
            Destroy(gameObject);
            return;
        }
        hpbar.fillAmount = curHp / (float)data.maxhp;
        nav.speed = data.speed * (Singleton.Get<GameManager>().EnemySlowTime > 0 ? 0.5f : 1);

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
                nav.enabled = Vector3.Distance(transform.position, hit.point) >= data.range - 0.1f;
        }

        if (nav.isActiveAndEnabled) nav.SetDestination(cloestTower.transform.position);
    }

    private void Attack()
    {
        if (isAttacking) return;
        if (cloestTower == null) return;
        if (Singleton.Get<GameManager>().StopEnemyAttackTime > 0) return;

        if (Physics.Linecast(transform.position, cloestTower.transform.position, out var hit, LayerMask.GetMask("Tower")))
        {
            if (ReferenceEquals(cloestTower.transform, hit.transform))
            {
                if (Vector3.Distance(transform.position, hit.point) < data.range)
                {
                    isAttacking = true;
                    StartCoroutine(Hit());
                }
            }
        }
    }

    private IEnumerator Hit()
    {
        cloestTower.Damage(data.damage);

        if (data.range > 1.5f)
        {
            PrefabContainer
                .Instantiate("EnemyBulletLine")
                .GetComponent<BulletLine>()
                .Init(transform.position, cloestTower.transform.position);
        }
        yield return new WaitForSeconds(data.attackDelay);
        isAttacking = false;
    }

    private void Detected()
    {
        if (data.type == Define.UnitType.Butterfly)
        {
            cloestTower = DetectTower(
                item => item is Castle,
                item => item is MainCastle);
        }
        else
        {
            cloestTower = DetectTower(item => item is Vision);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, data.range);
    }

    public virtual void Damage(int damageCount)
    {
        curHp -= damageCount;
        PrefabContainer.Instantiate("HitEffect", transform.position, Quaternion.identity);
    }
}
