using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMenuUI : DownMenuAttachUI
{
    protected override bool OpenCriteria => downMenuUI.State == DownMenuState.item;
}
