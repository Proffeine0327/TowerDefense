using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Tower : TowerBase, IUpgradeable
{
    [SerializeField] private float turnSpeed = 10f;
    [SerializeField] private float maxShotDelay;
    [SerializeField] private Transform partToRotate;
    [SerializeField] private Transform[] firePoint;
    [SerializeField] private GameObject[] levelTower;

    private Enemy[] cloestEnemies;
    private float curShotDelay;

    public override string ExplainContent => 
        $"Max Hp. {data.levelStats[level].maxhp}\n" +
        $"Hp. {curhp}\n" +
        $"DPS. {1 / data.levelStats[level].attackDelay * data.levelStats[level].damage:#.##}\n\n" +
        data.explain;
    public int RequireCost => data.levelStats[level].nextRequireCost;

    protected override void Awake()
    {
        base.Awake();
        StartCoroutine(Detected());
    }

    protected override void Update()
    {
        base.Update();
        for(int i = 0; i < levelTower.Length; i++) levelTower[i].SetActive(i == level);
        UpdateType();
    }

    private void UpdateType()
    {
        if (isGhost) return;
        if (cloestEnemies == null) return;
        if (cloestEnemies.Length == 0) return;
        if (cloestEnemies[0] == null) return;
        var dir = cloestEnemies[0].transform.position - transform.position;
        var lookRotation = Quaternion.LookRotation(dir);
        var rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
        partToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);
        Attack();
    }

    private void Attack()
    {
        if (curShotDelay > 0) curShotDelay -= Time.deltaTime;
        else
        {
            if (cloestEnemies.Length > 0)
            {
                switch (data.type)
                {
                    case Define.BuildType.Basic:
                    case Define.BuildType.Focus:
                        BasicAttack();
                        break;
                    case Define.BuildType.Multiple:
                        MultipleAttack();
                        break;
                }
                curShotDelay = maxShotDelay * (Singleton.Get<GameManager>().ReduceAttackDelayTime > 0 ? 0.5f : 1);
            }
        }
    }

    private IEnumerator Detected()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            cloestEnemies = DetectEnemies();
        }
    }

    private Enemy[] DetectEnemies()
    {
        return Enemy.enemies
                .Where(item => Vector3.Distance(transform.position, item.transform.position) < data.levelStats[level].range)
                .OrderBy(item => Vector3.Distance(item.transform.position, transform.position))
                .ToArray();
    }

    private void BasicAttack()
    {
        cloestEnemies[0].Damage(data.levelStats[level].damage);
        PrefabContainer
            .Instantiate("TowerBulletLine")
            .GetComponent<BulletLine>()
            .Init(firePoint[level].position, cloestEnemies[0].transform.position);
    }

    private void MultipleAttack()
    {
        for (int i = 0; i < Mathf.Min(5, cloestEnemies.Length); i++)
        {
            if (!cloestEnemies[i]) continue;

            cloestEnemies[i].Damage(data.levelStats[level].damage);
            PrefabContainer
                .Instantiate("TowerBulletLine")
                .GetComponent<BulletLine>()
                .Init(firePoint[level].position, cloestEnemies[i].transform.position);
        }
    }

    public void Upgrade()
    {
        if(data.levelStats[level].nextRequireCost != -1)
            level++;
    }
}
