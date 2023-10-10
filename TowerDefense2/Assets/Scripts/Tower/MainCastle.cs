using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCastle : TowerBase
{
    public GameObject damageEffect;

    public bool IsDestroyed { get; private set; }

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
            IsDestroyed = true;
        }
    }
}
