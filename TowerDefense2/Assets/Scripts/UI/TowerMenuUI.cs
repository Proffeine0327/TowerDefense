using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerMenuUI : DownMenuAttachUI
{
    protected override bool OpenCriteria => downMenuUI.State == DownMenuState.tower;
}
