using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : TowerBase
{
    public override void Damage(int damageCount)
    {
        curhp -= damageCount;
    }
}
