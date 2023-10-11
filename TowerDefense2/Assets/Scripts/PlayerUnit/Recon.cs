using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Recon : MonoBehaviour, ISelectable, IUpgradeable
{
    [SerializeField] private LevelStat[] stats;
    [TextArea(3, 7)][SerializeField] private string explain;

    private int level;
    private float curAttackDelay;
    private NavMeshAgent agent;

    public virtual string ExplainContent =>
    $"DPS. {1 / stats[level].attackDelay * stats[level].damage:0.##}\n" +
    $"Speed. {stats[level].speed}\n" +
    $"Level. {level + 1}\n\n" +
    explain;

    public int RequireCost => stats[level].nextRequireCost;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        StartCoroutine(MoveRoutine());
    }

    protected virtual void Update()
    {
        var detection =
            Enemy.enemies
            .Where(item => Vector3.Distance(transform.position, item.transform.position) <= stats[level].range)
            .OrderBy(item => Vector3.Distance(transform.position, item.transform.position))
            .ToArray();

        agent.enabled = detection.Length == 0;

        if (curAttackDelay > 0)
        {
            curAttackDelay -= Time.deltaTime;
        }
        else
        {
            if (detection.Length > 0)
            {
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

    private IEnumerator MoveRoutine()
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

    public void Upgrade()
    {
        if (stats[level].nextRequireCost != -1)
            level++;
    }

    public void Select()
    {
        
    }

    public void Unselect()
    {
        
    }
}
