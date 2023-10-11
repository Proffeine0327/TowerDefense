using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vision : TowerBase
{
    public override string ExplainContent =>
        $"Remain. {0:0}:{curhp:00}\n\n" + data.explain;

    protected override void Awake()
    {
        base.Awake();
        StartCoroutine(DestroyCount());
    }

    private IEnumerator DestroyCount()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            curhp -= 1;
        }
    }

    public override void Damage(int damageCount) { }
}
