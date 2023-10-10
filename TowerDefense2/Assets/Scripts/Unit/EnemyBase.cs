using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyBase : MonoBehaviour
{
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

        curHp = data.stats[0].maxhp;
    }

    protected PlayerUnit DetectTower()
    {
        var dectations =
                PlayerUnit.units
                .OrderBy(item => Vector3.Distance(item.transform.position, transform.position))
                .ToArray();
        if (dectations.Length > 0) return dectations[0];
        return null;
    }

    protected PlayerUnit DetectTower(params System.Func<PlayerUnit, bool>[] prioritys)
    {
        var detections =
                PlayerUnit.units
                .OrderBy(item => Vector3.Distance(item.transform.position, transform.position))
                .ToArray();

        foreach (var p in prioritys)
        {
            var c_detections =
                    detections
                    .Where(item => p.Invoke(item))
                    .ToArray();

            if (c_detections.Length > 0) return c_detections[0];
        }
        if (detections.Length > 0) return detections[0];

        return null;
    }

    protected virtual void Update()
    {
        if (curHp <= 0)
        {
            DataManager.Instance.Score += data.score;
            Singleton.Get<GameManager>().Money += data.gold;
            Destroy(gameObject);
            return;
        }
        hpbar.fillAmount = curHp / (float)data.stats[0].maxhp;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, data.stats[0].range);
    }

    public virtual void Damage(int damageCount)
    {
        curHp -= damageCount;
        PrefabContainer.Instantiate("HitEffect", transform.position, Quaternion.identity);
    }
}
