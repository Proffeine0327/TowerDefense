using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Castle : TowerBase
{
    public GameObject damageEffect;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
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
            Destroy(gameObject);
        }
    }
}
