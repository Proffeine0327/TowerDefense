using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new TowerData", menuName = "Scriptable/TowerData", order = int.MinValue)]
public class TowerData : ScriptableObject
{
    public GameObject prefeb;
    public Define.BuildType type;
    public int maxhp;
    public int damage;
    public int cost;
    public float range;
    public bool isDoubleGrid;
}
