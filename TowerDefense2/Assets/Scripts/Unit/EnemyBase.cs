using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyBase : MonoBehaviour
{
    public static HashSet<EnemyBase> enemies = new();

    [SerializeField] protected UnitData data;
    [SerializeField] protected Image hpbar;

    protected PlayerUnit cloestTower;
    protected NavMeshAgent nav;
    protected bool isAttacking;
    protected int curHp;
    protected string targetTag;

    protected virtual void Awake()
    {
        nav = GetComponent<NavMeshAgent>();
        curHp = data.maxhp;
        enemies.Add(this);
    }

    protected PlayerUnit DetectTower()
    {
        var gm = Singleton.Get<GameManager>();

        var detections =
                PlayerUnit.units
                .OrderBy(item => Vector3.Distance(item.transform.position, transform.position))
                .ToArray();

        if (detections.Length > 0)
        {
            if (ReferenceEquals(detections[0].gameObject, gm.MainCastle))
            {
                if (gm.ExistSubCastles)
                {
                    var detectionSubCastle =
                        PlayerUnit.units
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

    protected PlayerUnit DetectTower(params System.Func<PlayerUnit, bool>[] priorities)
    {
        var gm = Singleton.Get<GameManager>();

        var detections =
                PlayerUnit.units
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
                        PlayerUnit.units
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
            Singleton.Get<GameManager>().Money += data.gold;
            Destroy(gameObject);
            return;
        }
        hpbar.fillAmount = curHp / (float)data.maxhp;
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
