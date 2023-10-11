using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Recon : PlayerUnit
{
    [SerializeField] private int damage;
    [SerializeField] private float range;
    [SerializeField] private float attackDelay;
    [TextArea(3, 7)] [SerializeField] private string explain;

    private float curAttackDelay;
    private NavMeshAgent agent;

    public override string ExplainContent => explain;

    private void Start()
    {
        // var rand50p = Random.Range(0, 2) == 0;

        // if(rand50p)
        // {
        //     var randangle = Random.Range(0, 360);
        //     var vector = new Vector3(Mathf.Cos(randangle * Mathf.Deg2Rad), 0, Mathf.Sin(randangle * Mathf.Deg2Rad))
        //     Singleton.Get<GameManager>().MainCastle
        // }
        // else
        // {

        // }

        agent = GetComponent<NavMeshAgent>();
        StartCoroutine(MoveRoutine());
    }

    protected override void Update()
    {
        var detection =
            EnemyBase.enemies
            .Where(item => Vector3.Distance(transform.position, item.transform.position) <= range)
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
                detection[0].Damage(damage);
                curAttackDelay = attackDelay;
                PrefabContainer
                    .Instantiate("TowerBulletLine")
                    .GetComponent<BulletLine>()
                    .Init(transform.position, detection[0].transform.position);
            }
        }
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
}
