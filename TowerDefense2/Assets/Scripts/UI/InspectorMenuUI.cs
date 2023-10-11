using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InspectorMenuUI : DownMenuAttachUI
{
    protected override bool OpenCriteria => downMenuUI.State == DownMenuState.inspect;
    
    [SerializeField] private TextMeshProUGUI text;

    protected override void Update()
    {
        base.Update();

        text.text = Singleton.Get<SelectManager>().SelectedObject?.ExplainContent;
    }
}
