using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new WaveData", menuName = "Scriptable/WaveData", order = int.MinValue)]
public class WaveData : ScriptableObject
{
    public int slimeCount;
    public int butterflyCount;
    public int slimeKingCount;
    public int slimeKnightCount;
    public int slimeSpeedCount;
    public int spawnCount;
}
