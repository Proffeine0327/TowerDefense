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
        towers.Add(this);
    }

    protected override void Update()
    {
        hpbar.fillAmount = curhp / (float)data.levelStats[0].maxhp;
        
        if (curhp <= data.levelStats[0].maxhp / 2)
        {
            damageEffect.SetActive(true);
        }
        if (curhp <= 0)
        {
            towers.Remove(this);
            IsDestroyed = true;
        }
    }
}
