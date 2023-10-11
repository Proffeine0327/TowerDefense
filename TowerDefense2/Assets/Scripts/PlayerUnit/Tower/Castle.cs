using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Castle : TowerBase
{
    public GameObject damageEffect;

    protected override void Awake()
    {
        base.Awake();
        units.Add(this);
    }

    protected override void Update()
    {
        hpbar.fillAmount = curhp / (float)data.maxhp;

        if (curhp <= data.maxhp / 2)
        {
            damageEffect.SetActive(true);
        }
        if (curhp <= 0)
        {
            units.Remove(this);
            Destroy(gameObject);
        }
    }
}
