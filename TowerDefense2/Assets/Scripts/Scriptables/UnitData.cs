using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class LevelUpStat
{
    public int maxhp;
    public int exp;
    public int damage;
    public float attackDelay;
    public float range;
}

[CreateAssetMenu(fileName = "new UnitData", menuName = "Scriptable/UnitData", order = int.MinValue)]
public class UnitData : ScriptableObject
{
    public Define.UnitType type;
    public int gold;
    public int score;
    public LevelUpStat[] stats;
    
    public int MaxLevel => stats.Length - 1;
}
