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
        Recon, //Á¤Âû À¯´Ö
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
}
