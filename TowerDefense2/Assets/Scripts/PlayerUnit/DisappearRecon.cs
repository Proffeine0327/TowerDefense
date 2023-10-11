using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DisappearRecon : MonoBehaviour, ISelectable
{
    [SerializeField] private LevelStat stat;
    [TextArea(3, 7)][SerializeField] private string explain;

    private int remainTime = 60;
    private int level;
    private float curAttackDelay;
    private NavMeshAgent agent;

    public virtual string ExplainContent =>
        $"Remain. {remainTime / 60:0}:{remainTime:00}\n" +
        $"DPS. {1 / stat.attackDelay * stat.damage:0.##}\n" +
        $"Speed. {stat.speed}\n\n" +
        explain;

    public int RequireCost => stat.nextRequireCost;

    public void Init()
    {
        var gm = Singleton.Get<GameManager>();
        var rand50p = Random.Range(0, 2) == 0;
        if (gm.ExistSubCastles && rand50p)
        {
            var randangle = Random.Range(0, 360);
            var pos = 
                new Vector3(Mathf.Cos(randangle * Mathf.Deg2Rad), 0, Mathf.Sin(randangle * Mathf.Deg2Rad)) * 1.75f + 
                gm.SubCastles[Random.Range(0, gm.SubCastles.Length)].transform.position;
            pos.y = 10;
            transform.position = pos;
            Debug.Log(pos);
        }
        else
        {
            var randangle = Random.Range(0, 360);
            var pos = 
                new Vector3(Mathf.Cos(randangle * Mathf.Deg2Rad), 0, Mathf.Sin(randangle * Mathf.Deg2Rad)) * 1.75f + 
                gm.MainCastle.transform.position;
            pos.y = 10;
            transform.position = pos;
            Debug.Log(pos);
        }

        agent = GetComponent<NavMeshAgent>();
        agent.enabled = true;
        StartCoroutine(MoveRoutine());
        StartCoroutine(DisappearRoutine());
    }

    private IEnumerator DisappearRoutine()
    {
        while(remainTime-- > 0) yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }

    protected virtual void Update()
    {
        var detection =
            Enemy.enemies
            .Where(item => Vector3.Distance(transform.position, item.transform.position) <= stat.range)
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
                detection[0].Damage(stat.damage);
                curAttackDelay = stat.attackDelay;
                PrefabContainer
                    .Instantiate("TowerBulletLine")
                    .GetComponent<BulletLine>()
                    .Init(transform.position, detection[0].transform.position);
            }
        }

        agent.speed = stat.speed;
    }

    private IEnumerator MoveRoutine()
    {
        yield return null;

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
        if (stat.nextRequireCost != -1)
            level++;
    }

    public void Select()
    {

    }

    public void Unselect()
    {

    }
}
