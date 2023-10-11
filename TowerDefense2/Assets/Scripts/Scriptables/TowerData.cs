using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new TowerData", menuName = "Scriptable/TowerData", order = int.MinValue)]
public class TowerData : ScriptableObject
{
    public GameObject prefeb;
    public Define.BuildType type;
    public int cost;
    public bool isDoubleGrid;
    [TextArea(3, 6)] 
    public string explain;
    public LevelStat[] levelStats;
}
