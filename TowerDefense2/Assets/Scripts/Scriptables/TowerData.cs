using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "new TowerData", menuName = "Scriptable/TowerData", order = int.MinValue)]
public class TowerData : ScriptableObject
{
    public GameObject prefeb;
    public Define.BuildType type;
    public string targetTag;
    public int maxhp;
    public int damage;
    public int cost;
    public bool isDoubleGrid;
}
