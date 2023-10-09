using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InspectorMenuUI : DownMenuAttachUI
{
    protected override bool OpenCriteria => downMenuUI.State == DownMenuState.inspect;
}
