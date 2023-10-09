using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public abstract class EnemyBase : MonoBehaviour
{
    [SerializeField] protected UnitData data;
    [SerializeField] protected Image hpbar;
    [SerializeField] protected float range;

    protected TowerBase cloestTower;
    protected NavMeshAgent nav;
    protected bool isAttacking;
    protected float curHp;
    protected string targetTag;

    protected virtual void Awake()
    {
        nav = GetComponent<NavMeshAgent>();

        curHp = data.maxhp;
    }

    protected virtual void Update()
    {
        if(curHp <= 0)
        {
            DataManager.Instance.Score += data.score;
            Singleton.Get<GameManager>().Money += data.gold;
            Destroy(gameObject);
            return;
        }
        hpbar.fillAmount = data.hp / (float)data.maxhp;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, range);
    }

    public abstract void Damage(int damageCount);
}
