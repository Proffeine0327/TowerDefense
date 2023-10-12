using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Recon : MonoBehaviour, ISelectable, IUpgradeable
{
    public static HashSet<Recon> recons = new();

    [SerializeField] protected LevelStat[] stats;
    [TextArea(3, 7)][SerializeField] protected string explain;

    protected int level;
    protected float curAttackDelay;
    protected NavMeshAgent agent;

    public virtual string ExplainContent =>
    $"DPS. {1 / stats[level].attackDelay * stats[level].damage:0.##}\n" +
    $"Speed. {stats[level].speed}\n" +
    $"Level. {level + 1}\n\n" +
    explain;

    public virtual int RequireCost => stats[level].nextRequireCost;

    protected virtual void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        recons.Add(this);
        StartCoroutine(MoveRoutine());
    }

    protected virtual void Update()
    {
        var detection =
            Enemy.enemies
            .Where(item => Vector3.Distance(transform.position, item.transform.position) <= stats[level].range)
            .OrderBy(item => Vector3.Distance(transform.position, item.transform.position))
            .ToArray();

        agent.enabled = detection?.Length == 0;

        if (curAttackDelay > 0)
        {
            curAttackDelay -= Time.deltaTime;
        }
        else
        {
            if (detection.Length > 0)
            {
                var relative = detection[0].transform.position - transform.position;
                var angle = Mathf.Atan2(relative.x, relative.z) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0, angle, 0);

                detection[0].Damage(stats[level].damage);
                curAttackDelay = stats[level].attackDelay;
                PrefabContainer
                    .Instantiate("TowerBulletLine")
                    .GetComponent<BulletLine>()
                    .Init(transform.position, detection[0].transform.position);
            }
        }

        agent.speed = stats[level].speed;
    }

    protected IEnumerator MoveRoutine()
    {
        while (true)
        {
            var point = Singleton.Get<PathPoint>().GetRandomPoint();

            while (Vector3.Distance(transform.position, point) > 1.5f)
            {
                if (agent.isActiveAndEnabled)
                    agent.SetDestination(point);
                yield return null;
            }

            yield return new WaitForSeconds(2);
        }
    }

    public virtual void Upgrade()
    {
        if (stats[level].nextRequireCost != -1)
            level++;
    }

    public virtual void Select()
    {
        
    }

    public virtual void Unselect()
    {
        
    }
}
