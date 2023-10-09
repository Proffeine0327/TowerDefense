using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "new UnitData", menuName = "Scriptable/UnitData", order = int.MinValue)]
public class UnitData : ScriptableObject
{
    public Define.UnitType type;
    public int hp;
    public int maxhp;
    public int gold;
    public int exp;
    public int damage;
    public int score;
}
