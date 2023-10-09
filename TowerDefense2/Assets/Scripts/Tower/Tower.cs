using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Tower : TowerBase
{
    [SerializeField] private int maxCount;
    [SerializeField] private float turnSpeed = 10f;
    [SerializeField] private float maxShotDelay;
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform partToRotate;
    [SerializeField] private Transform[] firePoint;
    [SerializeField] private GameObject[] LevelTower;

    private EnemyBase[] cloestEnemies;
    private float curShotDelay;

    protected override void Awake()
    {
        base.Awake();
        StartCoroutine(Detected());
    }

    protected override void Update()
    {
        base.Update();
        UpdateType();
    }

    private void UpdateType()
    {
        if (isGhost) return;
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
                curShotDelay = maxShotDelay;
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

    private EnemyBase[] DetectEnemies()
    {
        return Physics.OverlapSphere(transform.position, range)
                .Where(item => item.CompareTag(data.targetTag))
                .Select(item => item.GetComponent<EnemyBase>())
                .OrderBy(item => Vector3.Distance(item.transform.position, transform.position))
                .ToArray();
    }

    private void BasicAttack()
    {
        cloestEnemies[0].Damage(data.damage);
    }

    private void MultipleAttack()
    {
        for (int i = 0; i < Mathf.Min(5, cloestEnemies.Length); i++)
        {
            if (!cloestEnemies[i]) continue;

            cloestEnemies[i].Damage(data.damage);
        }
    }

    public override void Damage(int amount)
    {
        curhp -= amount;
    }
}
