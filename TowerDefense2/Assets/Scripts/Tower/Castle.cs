using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Castle : TowerBase
{
    public GameObject damageEffect;

    protected override void Update()
    {
        hpbar.fillAmount = curhp / (float)data.maxhp;

        if (curhp <= data.maxhp / 2)
        {
            damageEffect.SetActive(true);
        }
        if (curhp <= 0)
        {
            gameObject.GetComponent<BoxCollider>().enabled = false;
        }
    }

    public override void Damage(int damageCount)
    {
        curhp -= damageCount; 
    }
}
