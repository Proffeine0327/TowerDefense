using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new UnitData", menuName = "Scriptable/UnitData", order = int.MinValue)]
public class UnitData : ScriptableObject
{
    public Define.UnitType type;
    public int gold;
    public int score;
    public int maxhp;
    public int damage;
    public float attackDelay;
    public float range;
}
