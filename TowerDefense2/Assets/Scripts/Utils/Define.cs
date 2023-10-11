using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{
    public enum ObjectType
    {
        None,
        Enemy,
        Top,
    }

    public enum UnitType
    {
        None,
        Slime,
        Butterfly,
        SlimeKing,
        SlimeKnight,
        SlimeSpeed,
        Bear,
        BearWalker,
    }

    public enum BuildType
    {
        None,
        MainCastle,
        SubCastle,
        Basic,
        Focus,
        Multiple,
        Obstacle,
        Vision,
    }

    public enum ItemType
    {
        TowerHeal,
        SlowEnemy,
        GainAdditiveGold,
        ReduceAttackDelay,
        StopEnemyAttack,
        SpawnRecon
    }
}
