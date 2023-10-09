using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCastle : TowerBase
{
    public GameObject damageEffect;

    public bool IsDestroyed { get; private set; }

    protected override void Update()
    {
        hpbar.fillAmount = curhp / (float)data.maxhp;

        if (curhp <= data.maxhp / 2)
        {
            damageEffect.SetActive(true);
        }
        
        if (curhp <= 0)
        {
            IsDestroyed = true;
        }
    }

    public override void Damage(int damageCount)
    {
        curhp -= damageCount;
    }
}
